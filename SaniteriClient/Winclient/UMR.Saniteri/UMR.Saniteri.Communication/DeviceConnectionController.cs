using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UMR.Saniteri.Communication.DeviceCommands;

namespace UMR.Saniteri.Communication
{
    public class DeviceConnectionController : IDeviceConnection
    {
        public void GetCanStatus()
        {
            new CanStatusCommand("202.168.246.210").communicate();
        }

        public void GetCanLog()
        {
            //new CanLogCommand("202.168.246.210").communicate();
        }

        public void UpdateCanFirmware()
        {
            //new CanFirmwareCommand("202.168.246.210").communicate();
        }
    }
}
