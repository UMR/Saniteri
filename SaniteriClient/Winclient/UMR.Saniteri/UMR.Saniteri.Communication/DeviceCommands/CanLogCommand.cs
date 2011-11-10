using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UMR.Saniteri.Communication.DeviceCommands
{
    public class CanLogCommand : BasedNetworkConnection
    {
        public CanLogCommand(string ipAddress)
            : base(new InitializerNetworkTCP(ipAddress, string.Empty), "Log Command")
        {

        }

        protected override void createCommand()
        {
            this.acknowledgeWait = 1;
            this.responseWait = false;
            this.command = new byte[8];
            this.command[0] = 0xAA;
            this.command[1] = 0xBB;
            this.command[2] = 0x00;
            this.command[3] = 0x0B;
            this.command[4] = 0x00;
            this.command[5] = 0x00;
            this.command[6] = 0x00;
            this.makeCheckSum();
        }

        public override bool communicate(InitializerNetwork initializer)
        {
            if (base.communicate(initializer))
            {
                return true;
            }
            return false;
        }

        protected override bool processData(byte[] buffer)
        {
            return base.processData(buffer);
        }
    }
}
