using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace LTG
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(KpiData.ConnectionString))
            {
                connection.Open();
                KpiData.EnsureLoginSessionTable(connection);
                KpiData.EnsureKpiRoleColumn(connection);
                KpiData.EnsureKpiTargetsTable(connection);
            }
        }
    }
}