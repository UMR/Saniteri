using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using UMR.Saniteri.Communication;
using UMR.Saniteri.Data;

namespace UMR.Saniteri.CommunicationService
{
    public partial class CommunicationSevice : ServiceBase
    {
        private readonly long StatusCheckTimespan = TimeSpan.FromSeconds(5).Ticks;
        private Thread _configThread = null;
        private bool bRun = false;
        private DateTime _lastStatusCheckTimespan = DateTime.Now;
        public IDeviceConnection DeviceCommunicationService { get; set; }

        private void ProcessingLoop()
        {
            while (bRun)
            {
                if ((DateTime.Now - _lastStatusCheckTimespan).Ticks < StatusCheckTimespan)
                {
                    Thread.Sleep(500);
                }
                else
                {
                    try
                    {
                        if (DeviceCommunicationService == null)
                        {
                            this.DeviceCommunicationService = DeviceConnectionFactory.GetConnectionController();
                        }
                        DeviceCommunicationService.GetCanStatus();
                        _lastStatusCheckTimespan = DateTime.Now;
                    }
                    catch (Exception ex)
                    {
                        DeviceCommunicationService = null;
                    }
                }
            }
        }

        public CommunicationSevice()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            DBManager.MainServerName = Properties.Settings.Default.MainServerName;
            DBManager.MainDBName = Properties.Settings.Default.MainDBName;
            DBManager.MainIntegratedSecurity = Properties.Settings.Default.MainIntegratedSecurity;
            DBManager.MainUserName = Properties.Settings.Default.MainUserName;
            DBManager.MainPassword = Properties.Settings.Default.MainPassword;
            bRun = true;
            _configThread = new Thread(new ThreadStart(ProcessingLoop));
            _configThread.Start();
        }

        protected override void OnStop()
        {
            bRun = false;
            _configThread = null;
            DeviceCommunicationService = null;
        }
    }
}
