using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UMR.Saniteri.Communication
{
    public class DeviceConnectionFactory
    {
        private static IDeviceConnection _connectionController = null;

        public static IDeviceConnection GetConnectionController()
        {
            if (_connectionController == null)
            {
                _connectionController = new DeviceConnectionController();
            }
            return _connectionController;
        }

        public static void ResetConnectionController()
        {
            if (_connectionController != null)
            {
                _connectionController = null;
            }
        }
    }
}
