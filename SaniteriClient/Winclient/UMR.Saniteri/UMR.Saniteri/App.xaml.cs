using System;
using System.Windows;
using UMR.Saniteri.DataFactory;
using UMR.Saniteri.Data;
using UMR.Saniteri.Common;
using System.Diagnostics;
using System.IO;

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
            checkDatabase();
        }

        private void checkDatabase()
        {
            try
            {
                if (!DatabaseManager.server.createDataBase(ApplicationData.applicationData.version))
                {
                    Process.Start(Path.Combine(ApplicationData.applicationData.executingLocation, "UMR.DBUtility.exe"));
                    System.Environment.Exit(0);
                }
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
