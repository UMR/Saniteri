using System;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using UMR.Saniteri.Data;
using System.Configuration;

namespace SaniteriWebService
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes();
            InitializeConnectionInfo();
        }

        private void InitializeConnectionInfo()
        {
            DBManager.MainServerName = ConfigurationManager.AppSettings["MainServerName"];
            DBManager.MainDBName = ConfigurationManager.AppSettings["MainDBName"];
            DBManager.MainIntegratedSecurity = Convert.ToBoolean(ConfigurationManager.AppSettings["MainIntegratedSecurity"]);
            DBManager.MainUserName = ConfigurationManager.AppSettings["MainUserName"];
            DBManager.MainPassword = ConfigurationManager.AppSettings["MainPassword"];
        }

        private void RegisterRoutes()
        {
            // Edit the base address of Service1 by replacing the "Service1" string below
            RouteTable.Routes.Add(new ServiceRoute("SaniteriWebService", new WebServiceHostFactory(), typeof(SaniteriWebService)));
        }
    }
}
