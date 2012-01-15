using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using System.IO;
using Microsoft.SqlServer.Management.Smo;
using System.Data.EntityClient;
using UMR.Saniteri.DataFactory.Properties;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using UMR.Saniteri.Data;
using UMR.Saniteri.Common;

namespace UMR.Saniteri.DataFactory
{
    public class SQLDatabase : IServer
    {
        private string settingsPath { get; set; }
        private SqlConnectionInfo connectionInfo { get; set; }
        public DatabaseSettings connectionSettings { get; set; }
        string modelName { get; set; }
        string dataFile { get; set; }

        public SQLDatabase()
        {
            this.connectionInfo = new SqlConnectionInfo();
            this.settingsPath = Path.Combine(CommonApplicationData.commonApplicationData.applicationFolderPath, "DBConfiguration.xml");
            if (!Directory.Exists(Path.GetDirectoryName(this.settingsPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(this.settingsPath));
            DatabaseSettings data = new ObjectReaderWriter().readObject<DatabaseSettings>(this.settingsPath);
            if (data == null)
            {
                var svrName = GetLocalDatabaseServer();
                data = new DatabaseSettings { databaseTechnology = "Microsoft SQL Server", serverName = svrName, integratedSecurity = true, databaseVersion = ApplicationData.applicationData.version };
                new ObjectReaderWriter().writeObject(data, this.settingsPath);
            }
            this.connectionSettings = data as DatabaseSettings;
            this.resetConnectionInfo();
        }

        public SQLDatabase(string _modelName, string dataFile)
            : this()
        {
            this.modelName = _modelName;
            this.dataFile = dataFile;
        }

        private string GetLocalDatabaseServer()
        {
            this.server = null;
            var svrName = string.Empty;
            this.servers = this.getSQLServers(true);
            this.server = this.servers.Where(e => e.serverName.Contains(Environment.MachineName + "\\SQLEXPRESS")).FirstOrDefault();
            if (this.server == null)
                this.server = this.servers.Where(e => e.serverName.Contains(Environment.MachineName)).FirstOrDefault();
            if (this.server != null)
                svrName = server.serverName;
            if (string.IsNullOrEmpty(svrName))
                svrName = ".\\SQLEXPRESS";
            return svrName;
        }

        private IEnumerable<ServerInstance> getSQLServers(bool shouldSortList)
        {
            var servers = new List<ServerInstance>();
            var sqlEnumerator = SqlDataSourceEnumerator.Instance;
            var serversView = sqlEnumerator.GetDataSources().DefaultView;
            serversView.Sort = "ServerName";
            foreach (DataRowView server in serversView)
            {
                var version = server["Version"].ToString();
                if (version.Length > 2 && Convert.ToDouble(version.Substring(0, 2)) < 10) continue;
                var serverName = server["ServerName"].ToString();
                var instanceName = server["InstanceName"].ToString();
                if (!string.IsNullOrEmpty(instanceName)) serverName += String.Format("\\{0}", instanceName);
                servers.Add(new ServerInstance(server["ServerName"].ToString(), serverName));
            }
            if (shouldSortList) servers = servers.OrderBy(e => e.serverName).ToList();
            return servers;
        }

        IEnumerable<ServerInstance> _servers;
        public IEnumerable<ServerInstance> servers
        {
            get { return this._servers; }
            set { this._servers = value;}
        }

        ServerInstance _server;
        public ServerInstance server
        {
            get { return this._server; }
            set { this._server = value; }
        }

        #region IDatabase Members

        public string backupLocation
        {
            get { return new Server(new ServerConnection(this.connectionInfo)).BackupDirectory; }
        }

        public EntityConnection getConnection(string modelName, string dataFilePath)
        {
            EntityConnectionStringBuilder ECSB = new EntityConnectionStringBuilder();
            ECSB.Provider = "System.Data.SqlClient";
            ECSB.Metadata = "res://*/modelName.csdl|res://*/modelName.ssdl|res://*/modelName.msl".Replace("modelName", modelName);
            string security = this.connectionInfo.UseIntegratedSecurity ? "Integrated Security=True;" : string.Format("User ID={0};Password={1};", this.connectionInfo.UserName, this.connectionInfo.Password);
            ECSB.ProviderConnectionString = string.Format(@"Data Source={0};Initial Catalog={1};{2}MultipleActiveResultSets=True;", this.connectionInfo.ServerName, dataFilePath, security);
            return new EntityConnection(ECSB.ToString());
        }

        public SaniteriModelEntities GetMainEntities()
        {
            return new SaniteriModelEntities(getConnection(modelName, dataFile));
        }

        public bool createDataBase(string DBVersion)
        {
            this.resetConnectionInfo();
            if (this.databaseExists(dataFile))
            {
                if (DBVersion == connectionSettings.databaseVersion)
                    return true;
                else
                    return createDataBase(dataFile, dataFile);
            }
            else
                return createDataBase(dataFile, dataFile);
        }

        public bool createDataBase(string databaseName, string scriptKey)
        {
            try
            {
                if (this.databaseExists(databaseName))
                    return true;
                var server = new Server(new ServerConnection(this.connectionInfo));
                var database = new Database(server, databaseName);
                var fileGroup = new FileGroup(database, "PRIMARY");
                var dateTime = DateTime.Now.ToString("ddMMyyHHmmss");
                var dataFile = new DataFile(fileGroup, databaseName + "_Data", Path.Combine(server.MasterDBLogPath, databaseName + dateTime + "_Data.mdf"));
                dataFile.GrowthType = FileGrowthType.Percent;
                dataFile.Growth = 10;
                fileGroup.Files.Add(dataFile);
                database.FileGroups.Add(fileGroup);
                var logFile = new LogFile(database, databaseName + "_Log", Path.Combine(server.MasterDBLogPath, databaseName + dateTime + "_Log.ldf"));
                logFile.GrowthType = FileGrowthType.Percent;
                logFile.Growth = 10;
                database.LogFiles.Add(logFile);
                database.Create();
                if (scriptKey != null)
                {
                    database.ExecuteNonQuery(Resources.ResourceManager.GetString(scriptKey + "_SQLScript"));
                    database.ExecuteNonQuery(Resources.ResourceManager.GetString(scriptKey + "_DefaultData"));
                }
                saveSettings(connectionSettings);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void deleteDataBase(string databaseName)
        {
            Server server = new Server(new ServerConnection(this.connectionInfo));
            server.KillDatabase(databaseName);
            SqlConnection.ClearAllPools();
        }

        private void resetConnectionInfo()
        {
            this.connectionInfo.ServerName = this.connectionSettings.serverName;
            if (this.connectionSettings.integratedSecurity)
                this.connectionInfo.UseIntegratedSecurity = true;
            else
            {
                this.connectionInfo.UserName = this.connectionSettings.userName;
                this.connectionInfo.Password = this.connectionSettings.password;
            }
        }

        public void tryToReachServer()
        {
            this.resetConnectionInfo();
            Server server = new Server(new ServerConnection(this.connectionInfo));
            var test = server.Status;
        }

        public bool databaseExists(string databaseName)
        {
            this.resetConnectionInfo();
            try
            {
                return new Server(new ServerConnection(this.connectionInfo)).Databases.Contains(databaseName);
            }
            catch
            {
                return false;
            }
        }

        public bool saveSettings(object connectionSettings)
        {
            this.connectionSettings = connectionSettings as DatabaseSettings;
            this.connectionSettings.databaseVersion = ApplicationData.applicationData.version;
            this.tryToReachServer();
            new ObjectReaderWriter().writeObject(this.connectionSettings, this.settingsPath);
            return true;
        }

        public void backupDatabase(string databaseName, string filePath, ProgressInformation progress)
        {
            var backup = new Backup();
            backup.Action = BackupActionType.Database;
            backup.Database = databaseName;
            backup.Devices.Clear();
            backup.Incremental = false;
            backup.Initialize = true;
            backup.LogTruncation = BackupTruncateLogType.Truncate;
            var backupItemDevice = new BackupDeviceItem(filePath, DeviceType.File);
            backup.Devices.Add(backupItemDevice);
            backup.PercentCompleteNotification = 10;
            backup.PercentComplete += (sender, e) => { progress.percent = e.Percent; };
            backup.Complete += (sender, e) => { if (e.Error != null) progress.error = new Exception(e.Error.Message); };
            backup.SqlBackup(new Server(new ServerConnection(this.connectionInfo)));
        }

        public void restoreDatabase(string databaseName, string filePath, ProgressInformation progress)
        {
            if (this.databaseExists(databaseName))
                throw new Exception(string.Format("Database \"{0}\" already exists.", databaseName));

            var server = new Server(new ServerConnection(this.connectionInfo));
            //
            var restore = new Restore();
            restore.Devices.Clear();
            restore.Devices.Add(new BackupDeviceItem(filePath, DeviceType.File));
            //
            var dateTime = DateTime.Now.ToString("ddMMyyHHmmss");
            var newLogicalDataFileName = databaseName + dateTime + "_Data";
            var newLogicalLogFileName = databaseName + dateTime + "_Log";
            var dt = restore.ReadFileList(server);
            var dataFile = new RelocateFile(dt.Rows[0]["LogicalName"].ToString(), Path.Combine(server.MasterDBLogPath, newLogicalDataFileName + ".mdf"));
            var logFile = new RelocateFile(dt.Rows[1]["LogicalName"].ToString(), Path.Combine(server.MasterDBLogPath, newLogicalLogFileName + ".ldf"));
            //
            restore.RelocateFiles.Add(dataFile);
            restore.RelocateFiles.Add(logFile);
            restore.Database = databaseName;
            restore.ReplaceDatabase = true;
            restore.PercentCompleteNotification = 10;
            restore.PercentComplete += (sender, e) => { progress.percent = e.Percent; };
            restore.Complete += (sender, e) => { if (e.Error != null) progress.error = new Exception(e.Error.Message); };
            restore.SqlRestore(server);
            var database = server.Databases[databaseName];
            if (newLogicalDataFileName != dataFile.LogicalFileName)
            {
                string modifyLogicalName = "alter database {0} modify file(name='{1}', newname='{2}')";
                database.ExecuteNonQuery(string.Format(modifyLogicalName, databaseName, dataFile.LogicalFileName, newLogicalDataFileName));
                database.ExecuteNonQuery(string.Format(modifyLogicalName, databaseName, logFile.LogicalFileName, newLogicalLogFileName));
            }
            database.SetOnline();
        }

        #endregion
    }

    public class ServerInstance
    {
        public string serverName { get; set; }
        public bool authenticationChangable { get; private set; }
        public ServerInstance(string serverName, string serverNameDisplay)
        {
            this.serverName = serverNameDisplay;
            this.authenticationChangable = Environment.MachineName == serverName;
        }
    }
}
