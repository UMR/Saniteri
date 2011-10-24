using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;
using System.ComponentModel;

namespace UMR.Saniteri.DataFactory
{
    public interface IServer
    {
        EntityConnection getConnection(string modelName, string dataFilePath);
        void createDataBase(string databaseName, string script);
        void deleteDataBase(string databaseName);
        bool databaseExists(string databaseName);
        bool saveSettings(object connectionSettings);
        void backupDatabase(string databaseName, string filePath, ProgressInformation progress);
        void restoreDatabase(string databaseName, string filePath, ProgressInformation progress);
        string backupLocation { get; }
    }

    public class ProgressInformation : INotifyPropertyChanged
    {
        Exception _error;
        public Exception error
        {
            get { return this._error; }
            set { this._error = value; this.onPropertyChanged("error"); }
        }

        int _percent;
        public int percent
        {
            get { return this._percent; }
            set { this._percent = value; this.onPropertyChanged("percent"); }
        }

        protected void onPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
