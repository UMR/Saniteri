using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;
using System.Data.SqlClient;

namespace UMR.Saniteri.Data
{
    public static class DBManager
    {
        private static string GetEFConnectionString(string modelName, string connectionStr)
        {
            EntityConnectionStringBuilder ecsb = new EntityConnectionStringBuilder();
            ecsb.Provider = "System.Data.SqlClient";
            ecsb.ProviderConnectionString = connectionStr;
            ecsb.Metadata = @"res://*/" + modelName + @".csdl|" +
                            @"res://*/" + modelName + @".ssdl|" +
                            @"res://*/" + modelName + @".msl";
            return ecsb.ToString();
        }

        private static string GetMainEFConnection(string connectionStr) { return GetEFConnectionString("SaniteriDataModel", connectionStr); }
        private static string GetProviderConnectionString(string server, string dbName, bool bIntegratedSecurity, string userName, string password)
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = server;
            csb.InitialCatalog = dbName;
            csb.IntegratedSecurity = bIntegratedSecurity;
            if (!bIntegratedSecurity)
            {
                csb.UserID = userName;
                csb.Password = password;
            }
            csb.MultipleActiveResultSets = true;
            return csb.ToString();
            //return string.Format("server={0};database={1};Integrated Security=SSPI;MultipleActiveResultSets=True", server, dbName);
        }

        public static string MainServerName { get; set; }
        public static string MainDBName { get; set; }
        public static bool MainIntegratedSecurity { get; set; }
        public static string MainUserName { get; set; }
        public static string MainPassword { get; set; }

        public static string GetCurrentMainDBConnectionStr()
        {
            return GetMainEFConnection(GetProviderConnectionString(MainServerName, MainDBName, MainIntegratedSecurity, MainUserName, MainPassword));
        }

        public static SaniteriModelEntities GetMainEntities()
        {
            MainServerName = Properties.Settings.Default.MainServerName;
            MainDBName = Properties.Settings.Default.MainDBName;
            MainIntegratedSecurity = Properties.Settings.Default.MainIntegratedSecurity;
            MainUserName = Properties.Settings.Default.MainUserName;
            MainPassword = Properties.Settings.Default.MainPassword;

            EntityConnection ec = new EntityConnection(GetCurrentMainDBConnectionStr());
            return new SaniteriModelEntities(ec);
        }
    }
}
