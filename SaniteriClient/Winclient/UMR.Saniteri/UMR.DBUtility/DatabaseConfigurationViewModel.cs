using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data.Sql;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Input;
using UMR.Saniteri.Command;
using UMR.Saniteri.DataFactory;
using System.IO;
using UMR.Saniteri.Common;

namespace UMR.DBUtility
{
    public class DatabaseConfigurationViewModel : INotifyPropertyChanged
    {
        public SQLDatabase database { get; private set; }
        private string DBName = "SaniteriMain";            

        public DatabaseConfigurationViewModel()
        {
            canCreate = false;
            ready = false;
            message = "Loading ... Please wait";
            this.database = new SQLDatabase("SaniteriDataModel", "SaniteriMain");
            Task task = Task.Factory.StartNew(() =>
            {
                this.servers = this.getSQLServers(true);
                this.server = this.servers.SingleOrDefault(e => e.serverName == this.database.connectionSettings.serverName.Replace(".\\", Environment.MachineName + "\\"));
                if (this.database.connectionSettings.integratedSecurity)
                {
                    this.authentication = this._authentications[1];
                    this.integratedSecurity = true;
                }
                else
                {
                    this.authentication = this._authentications[0];
                    this.userName = this.database.connectionSettings.userName;
                    this.password = this.database.connectionSettings.password;
                }
                message = "";
                checkDatabaseAsync();
            });
        }

        IEnumerable<ServerInstance> _servers;
        public IEnumerable<ServerInstance> servers
        {
            get { return this._servers; }
            set { this._servers = value; this.onPropertyChanged("servers"); }
        }

        ServerInstance _server;
        public ServerInstance server
        {
            get { return this._server; }
            set
            {
                this._server = value;
                if (value != null && !value.authenticationChangable) this.authentication = this.authentications[0];
                this.onPropertyChanged("server");
            }
        }

        string[] _authentications;
        public string[] authentications
        {
            get
            {
                if (this._authentications == null) this._authentications = new string[] { "SQL Server Authentication", "Windows Authentication" };
                return this._authentications;
            }
        }

        string _authentication;
        public string authentication
        {
            get { return this._authentication; }
            set
            {
                this._authentication = value;
                this.integratedSecurity = this._authentication == this._authentications[1];
                this.onPropertyChanged("authentication");
            }
        }

        string _userName;
        public string userName { get { return this._userName; } set { this._userName = value; this.onPropertyChanged("userName"); } }

        string _password;
        public string password { get { return this._password; } set { this._password = value; this.onPropertyChanged("password"); } }

        bool _integratedSecurity;
        public bool integratedSecurity
        {
            get { return this._integratedSecurity; }
            set { this._integratedSecurity = value; this.onPropertyChanged("integratedSecurity"); }
        }

        string _message;
        public string message
        {
            get { return this._message; }
            private set { this._message = value; this.onPropertyChanged("message"); }
        }

        bool _ready;
        public bool ready
        {
            get { return this._ready; }
            private set { this._ready = value; this.onPropertyChanged("ready"); this.onPropertyChanged("canFinish"); }
        }

        bool _connected;
        public bool connected
        {
            get { return this._connected; }
            private set { this._connected = value; this.onPropertyChanged("connected"); this.onPropertyChanged("canFinish"); }
        }

        public bool canFinish { get { return this.connected && !this.canCreate; } }

        bool _canCreate = false;
        public bool canCreate 
        {
            get { return this._canCreate; }
            set { this._canCreate = value; this.onPropertyChanged("canCreate"); } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void onPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null) this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Commands

        DelegateCommand _checkDatabaseCommand;
        public ICommand checkDatabaseCommand
        {
            get
            {
                if (_checkDatabaseCommand == null) _checkDatabaseCommand = new DelegateCommand(() => this.checkDatabaseAsync(), () => { return this.ready; });
                return _checkDatabaseCommand;
            }
        }

        DelegateCommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null) _saveCommand = new DelegateCommand(() => this.saveSettings(), () => { return true; });
                return _saveCommand;
            }
        }

        DelegateCommand _createCommand;
        public ICommand CreateCommand
        {
            get
            {
                if (_createCommand == null) _createCommand = new DelegateCommand(() => this.CreateDatabase(), () => { return true; });
                return _createCommand;
            }
        }

        DelegateCommand _closeCommand;
        public ICommand closeCommand
        {
            get
            {
                if (_closeCommand == null) _closeCommand = new DelegateCommand(() => System.Environment.Exit(0));
                return _closeCommand;
            }
        }

        #endregion

        private void CreateDatabase()
        {
            try
            {
                ready = false;
                message = "Creating Database...please wait";
                resetConnectionInfo();
                this.database.createDataBase(ApplicationData.applicationData.version);
                if (database.databaseExists(DBName))
                {
                    message = "Database has created successfully.";
                    canCreate = false;
                }
                connected = true;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg = ex.InnerException.Message;
                DialogManager.popup(msg);
            }
        }

        private void resetConnectionInfo()
        {
            database.connectionSettings.serverName = server.serverName;
            if (integratedSecurity) database.connectionSettings.integratedSecurity = integratedSecurity;
            else
            {
                database.connectionSettings.integratedSecurity = false;
                database.connectionSettings.userName = userName;
                database.connectionSettings.password = password;
            }
        }

        private void checkDatabaseAsync()
        {
            if (database == null) database = new SQLDatabase();
            Task.Factory.StartNew(() => 
            {
                canCreate = !database.databaseExists(DBName);
                if (!canCreate) this.message = "Database Up to Date";
                ready = canCreate;
            });
        }

        private bool saveSettings()
        {
            this.ready = false;
            this.connected = false;
            database.connectionSettings.serverName = server.serverName;
            if (integratedSecurity) database.connectionSettings.integratedSecurity = integratedSecurity;
            else
            {
                database.connectionSettings.integratedSecurity = false;
                database.connectionSettings.userName = userName;
                database.connectionSettings.password = password;
            }
            try
            {
                database.saveSettings(database.connectionSettings);
                checkDatabaseAsync();
                return true;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg = ex.InnerException.Message;
                DialogManager.popup(msg);
                return connected;
            }
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
}
