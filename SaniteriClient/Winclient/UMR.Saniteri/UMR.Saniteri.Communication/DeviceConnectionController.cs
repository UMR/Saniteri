using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UMR.Saniteri.Communication.DeviceCommands;
using System.Net;
using System.Net.NetworkInformation;
using UMR.Saniteri.DataFactory;

namespace UMR.Saniteri.Communication
{
    public class DeviceConnectionController : IDeviceConnection
    {
        public void GetCanStatus()
        {
            using (var ctx = DatabaseManager.server.GetMainEntities())
            {
                var canList = ctx.can_inventory.Where(dr => dr.ip_address != null);
                foreach (var can in canList)
                    if (!string.IsNullOrEmpty(can.ip_address))
                        GetCanStatus(can.ip_address);
            }
        }

        public void GetCanStatus(string ipAdrress)
        {
            var ping = new Ping();
            if (ping.Send(ipAdrress, 1000).Status == IPStatus.Success)
                new CanStatusCommand(ipAdrress).communicate();
        }

        public void GetCanLog()
        {
            using (var ctx = DatabaseManager.server.GetMainEntities())
            {
                var canList = ctx.can_inventory.Where(dr => dr.ip_address != null);
                foreach (var can in canList)
                    if (!string.IsNullOrEmpty(can.ip_address))
                        GetCanStatus(can.ip_address);
            }
        }

        public void GetCanLog(string ipAdrress)
        {
            var ping = new Ping();
            if (ping.Send(ipAdrress, 1000).Status == IPStatus.Success)
                new CanLogCommand(ipAdrress).communicate();
        }

        public void UpdateCanFirmware(string ipAdrress)
        {
            var ping = new Ping();
            if (ping.Send(ipAdrress, 1000).Status == IPStatus.Success)
                new CanFirmwareCommand(ipAdrress).communicate();
        }
    }
}
