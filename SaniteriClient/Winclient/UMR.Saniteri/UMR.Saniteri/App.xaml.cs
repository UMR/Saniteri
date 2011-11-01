using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using UMR.Saniteri.DataFactory;
using UMR.Saniteri.Data;
using UMR.Saniteri;
using UMR.Saniteri.Common;

namespace UMR.Saniteri
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SingleInstanceCreator.make("Saniteri", this);
            base.OnStartup(e);
            InitializeConnectionInfo();
            checkDatabase();
            InitializeDialogManager();
        }

        private void InitializeDialogManager()
        {
            // messagebox
           DialogManager.popup = (Action<string>)(msg => MessageBox.Show(msg));
            // confirm box
           DialogManager.confirm = (Func<string, string, bool>)((msg, capt) =>
                MessageBox.Show(msg, capt, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes);
        }

        private void InitializeConnectionInfo()
        {
            DBManager.MainServerName = UMR.Saniteri.Properties.Settings.Default.MainServerName;
            DBManager.MainDBName = UMR.Saniteri.Properties.Settings.Default.MainDBName;
            DBManager.MainIntegratedSecurity = UMR.Saniteri.Properties.Settings.Default.MainIntegratedSecurity;
            DBManager.MainUserName = UMR.Saniteri.Properties.Settings.Default.MainUserName;
            DBManager.MainPassword = UMR.Saniteri.Properties.Settings.Default.MainPassword;
        }

        private void checkDatabase()
        {
            try
            {
                DatabaseManager.server.createDataBase(DatabaseManager.dataFile, DatabaseManager.dataFile);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg = ex.InnerException.Message;
                DialogManager.popup(msg);
            }
        }

    }
}
