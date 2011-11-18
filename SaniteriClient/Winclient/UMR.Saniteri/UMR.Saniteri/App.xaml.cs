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
using Microsoft.Win32;

namespace UMR.Saniteri
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            InitializeDialogManager();
            SingleInstanceCreator.make("Saniteri", this);
            base.OnStartup(e);
            InitializeConnectionInfo();
            checkDatabase();
        }

        private void InitializeDialogManager()
        {
            DialogManager.popup = (Action<string>)(msg => MessageBox.Show(msg));
            DialogManager.confirm = (Func<string, string, bool>)((msg, capt) =>
                 MessageBox.Show(msg, capt, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes);
            DialogManager.openFile = (Func<string, string>)((filter) =>
                {
                    if (string.IsNullOrEmpty(filter)) filter = "Firmware Files (*.bin)|*.bin";
                    var fileName = string.Empty;
                    var dialog = new OpenFileDialog();
                    dialog.CheckPathExists = true;
                    dialog.Multiselect = false;
                    dialog.Filter = filter;
                    var result = dialog.ShowDialog();
                    if (result.HasValue && result.Value)
                        fileName = dialog.FileNames.FirstOrDefault();
                    return fileName;
                });
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
