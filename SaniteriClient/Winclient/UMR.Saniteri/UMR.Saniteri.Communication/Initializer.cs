using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace UMR.Saniteri.Communication
{
    interface IInitializer
    {
        CancellationTokenSource cancel { get; }
        bool success { get; set; }
        bool timeout { get; set; }
        string message { get; set; }
        string securityKey { get; set; }
        bool initialized { get; set; }
    }

    public abstract class InitializerNetwork : IInitializer, IDisposable
    {
        public const int REMOTE_PORT_TCP = 3030;
        public const int REMOTE_PORT_UDP = 0x77FE;

        public int remotePort { get; protected set; }
        public SocketType socketType { get; protected set; }
        public ProtocolType protocolType { get; protected set; }

        public string securityKey { get; set; }
        public bool initialized { get; set; }
        public bool security { get; set; }
        public bool busy { get; set; }
        public bool success { get; set; }
        public bool timeout { get; set; }
        public string message { get; set; }
        public Socket socket { get; set; }
        public string IP { get; protected set; }
        public CancellationTokenSource cancel { get; protected set; }
        public bool messageReported { get; set; }

        public InitializerNetwork(string IP, string securityKey, CancellationTokenSource cancel)
        {
            this.securityKey = securityKey;
            this.cancel = cancel;
            this.IP = IP;
        }

        public virtual void Dispose()
        {
            if (this.socket != null)
            {
                if (this.socket != null)
                {
                    if (this.socket.Connected && this.socket.ProtocolType == ProtocolType.Tcp) { try { this.socket.Disconnect(false); } catch { } }
                    this.socket.Close();
                }
                this.socket.Dispose();
                this.socket = null;
            }
        }
    }

    public class InitializerNetworkTCP : InitializerNetwork
    {
        public InitializerNetworkTCP(string IP, string securityKey, CancellationTokenSource cancel = null)
            : base(IP, securityKey, cancel)
        {
            this.remotePort = REMOTE_PORT_TCP;
            this.socketType = SocketType.Stream;
            this.protocolType = ProtocolType.Tcp;
        }
    }
}
