using System;
using System.Linq;
using Microsoft.Crm.Services.Utility;
using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace CRMSvcUtilExtensions
{
    public class SolutionFilter : ICodeWriterFilterService
    {
        private readonly ICodeWriterFilterService defaultService;
        private readonly IEnumerable<EntityMetadata> solutionEntities;

        public SolutionFilter(ICodeWriterFilterService defaultService)
        {
            this.defaultService = defaultService;
            var uniqueSolutionName = Parameters.GetParameter("solution");

            var service = new OrganizationServiceFactory().Create();
            solutionEntities = getSolutionEntities(uniqueSolutionName, service);
        }

        public bool GenerateAttribute(AttributeMetadata attributeMetadata, IServiceProvider services)
        {
            return defaultService.GenerateAttribute(attributeMetadata, services);
        }

        public bool GenerateEntity(EntityMetadata entityMetadata, IServiceProvider services)
        {
            if(solutionEntities.Any(a => a.LogicalName == entityMetadata.LogicalName))
            {
                return defaultService.GenerateEntity(entityMetadata, services);
            }

            return false;
        }

        public bool GenerateOption(OptionMetadata optionMetadata, IServiceProvider services)
        {
            return defaultService.GenerateOption(optionMetadata, services);
        }

        public bool GenerateOptionSet(OptionSetMetadataBase optionSetMetadata, IServiceProvider services)
        {
            return defaultService.GenerateOptionSet(optionSetMetadata, services);
        }

        public bool GenerateRelationship(RelationshipMetadataBase relationshipMetadata, EntityMetadata otherEntityMetadata, IServiceProvider services)
        {
            return defaultService.GenerateRelationship(relationshipMetadata, otherEntityMetadata, services);
        }

        public bool GenerateServiceContext(IServiceProvider services)
        {
            return defaultService.GenerateServiceContext(services);
        }

        private IEnumerable<EntityMetadata> getSolutionEntities(string solutionUniqueName, IOrganizationService service)
        {
            var solutionNames = solutionUniqueName.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries).Cast<object>();

            QueryExpression componentsQuery = new QueryExpression
            {
                EntityName = "solutioncomponent",
                ColumnSet = new ColumnSet("objectid"),
                Criteria = new FilterExpression(),
            };
            LinkEntity solutionLink = new LinkEntity("solutioncomponent", "solution", "solutionid", "solutionid", JoinOperator.Inner);
            solutionLink.LinkCriteria = new FilterExpression();
            var condition = new ConditionExpression("uniquename", ConditionOperator.In, (object[])solutionNames);
            solutionLink.LinkCriteria.AddCondition(condition);
            componentsQuery.LinkEntities.Add(solutionLink);
            componentsQuery.Criteria.AddCondition(new ConditionExpression("componenttype", ConditionOperator.Equal, 1));
            EntityCollection componentCollection = service.RetrieveMultiple(componentsQuery);
            
            RetrieveAllEntitiesRequest allEntitiesrequest = new RetrieveAllEntitiesRequest()
            {
                EntityFilters = EntityFilters.Entity,
                RetrieveAsIfPublished = true
            };
            RetrieveAllEntitiesResponse allEntitiesResponse = (RetrieveAllEntitiesResponse)service.Execute(allEntitiesrequest);
            
            var entitiesInSolution = allEntitiesResponse.EntityMetadata.Join(componentCollection.Entities.Select(x => x.Attributes["objectid"]), x => x.MetadataId, y => y, (x, y) => x).ToList();
            if(entitiesInSolution.Any(a => a.IsActivity.HasValue && a.IsActivity.Value))
            {
                entitiesInSolution.Add(getEntityMetadata("activityparty", service));
            }

            if (entitiesInSolution.Any(a => a.LogicalName == "service"))
            {
                entitiesInSolution.Add(getEntityMetadata("calendarrule", service));
            }

            return entitiesInSolution;
        }

        private EntityMetadata getEntityMetadata(string entityLogicalName, IOrganizationService service)
        {
            var request = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Entity,
                LogicalName = entityLogicalName,
                RetrieveAsIfPublished = true
            };

            var response = (RetrieveEntityResponse)service.Execute(request);

            return response.EntityMetadata;
        }
    }
}
