using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace UMR.Saniteri.CommunicationService
{
    [RunInstaller(true)]
    public partial class CommunicationSeviceInstaller : System.Configuration.Install.Installer
    {
        public CommunicationSeviceInstaller()
        {
            InitializeComponent();
        }
    }
}
