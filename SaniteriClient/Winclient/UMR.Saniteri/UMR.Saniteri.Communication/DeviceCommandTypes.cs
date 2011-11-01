using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UMR.Saniteri.Communication
{
    public enum DeviceCommandTypes : byte
    {
        deviceACK = 0x15,
        deviceNACK = 0x16,
        deviceSatus = 0x01,
        deviceLog = 0x02,
        firmwareUpload = 0x03
    }
}
