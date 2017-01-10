using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk;

namespace CRMSvcUtilExtensions
{
    public class OrganizationServiceFactory
    {
        public IOrganizationService Create()
        {
            string authType = Parameters.GetParameter("authtype") ?? "Office365";
            string connectionString = (Parameters.GetParameter("connectionstring") ??
                                       Parameters.GetParameter("connstr")) ??
                                       $"AuthType={authType};Url={Parameters.GetParameter("url")}; Username={Parameters.GetParameter("username")}; Password={Parameters.GetParameter("password")};";

            var connection = new CrmServiceClient(connectionString);
            var service = (IOrganizationService)connection.OrganizationWebProxyClient ?? (IOrganizationService)connection.OrganizationServiceProxy;

            return service;
        }
    }
}