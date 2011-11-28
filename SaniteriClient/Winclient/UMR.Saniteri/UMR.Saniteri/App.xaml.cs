using System;
using System.Windows;
using UMR.Saniteri.DataFactory;
using UMR.Saniteri.Data;
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
            checkDatabase();
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
