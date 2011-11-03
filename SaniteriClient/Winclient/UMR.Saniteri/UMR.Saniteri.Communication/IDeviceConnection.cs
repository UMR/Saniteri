using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UMR.Saniteri.Communication
{
    public interface IDeviceConnection
    {
        void GetCanStatus();
        void GetCanStatus(string ipAddress);
        void GetCanLog();
        void GetCanLog(string ipAddress);
        void UpdateCanFirmware(string ipAddress);
    }
}
