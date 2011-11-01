using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UMR.Saniteri.Communication
{
    public interface IDeviceConnection
    {
        void GetCanStatus();
        void GetCanLog();
        void UpdateCanFirmware();
    }
}
