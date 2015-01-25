using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;

namespace CRMSvcUtilExtensions
{
    public class OrganizationServiceFactory
    {
        public IOrganizationService Create()
        {
            var connectionString = string.Format("Url={0}; Username={1}; Password={2};", Parameters.GetParameter("url"), Parameters.GetParameter("username"), Parameters.GetParameter("password"));
            var connection = CrmConnection.Parse(connectionString);
            var service = new OrganizationService(connection);

            return service;
        }
    }
}