using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace UMR.Saniteri.Communication
{
    public class ProtocolHelper
    {
        private ProtocolHelper() { this.nextPort = PORT_START; }

        static ProtocolHelper _protocolHelper;
        public static ProtocolHelper protocolHelper
        {
            get
            {
                if (_protocolHelper == null)
                {
                    _protocolHelper = new ProtocolHelper();
                    _protocolHelper.checkNetworkInterfaces();
                }
                return _protocolHelper;
            }
            private set { _protocolHelper = value; }
        }

        private const int PORT_START = 0x7000;
        private const int PORT_END = 0x7700;
        public int nextPort { get; set; }

        public readonly IPAddress emptyIP = new IPAddress(new byte[] { 0x00, 0x00, 0x00, 0x00 });

        public readonly IPAddress blankIP = new IPAddress((long)0);

        public IList<NetworkInterface> networkInterfaces { get; private set; }

        NetworkInterface _networkInterface;
        public NetworkInterface networkInterface
        {
            get { return this._networkInterface; }
            set
            {
                this.interfaceIPAddress = this.isValidNetworkInterface(value);
                if (this.interfaceIPAddress != null)
                    this._networkInterface = value;
            }
        }

        public IPAddress interfaceIPAddress { get; set; }

        public void checkNetworkInterfaces()
        {
            this.networkInterfaces = new List<NetworkInterface>();
            NetworkInterface[] networkInterfacesAll = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface NI in networkInterfacesAll)
            {
                if (NI.GetIPProperties() == null
                    || NI.NetworkInterfaceType == NetworkInterfaceType.Loopback
                    || NI.NetworkInterfaceType == NetworkInterfaceType.Tunnel
                    || NI.GetIPProperties().UnicastAddresses.Count == 0
                    || NI.OperationalStatus != OperationalStatus.Up)
                    continue;
                this.networkInterfaces.Add(NI);
            }
            this.networkInterface = this.networkInterfaces.FirstOrDefault();
        }

        public IPAddress isValidNetworkInterface(NetworkInterface networkInterface)
        {
            if (networkInterface == null) return null;
            IPInterfaceProperties properties = networkInterface.GetIPProperties();
            if (properties != null && properties.UnicastAddresses.Count > 0)
            {
                foreach (UnicastIPAddressInformation UIP in properties.UnicastAddresses)
                {
                    if (UIP.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (!UIP.Address.Equals(ProtocolHelper.protocolHelper.emptyIP)) return UIP.Address;
                    }
                }
            }
            return null;
        }

        public int getNextPort()
        {
            var nextPort = this.nextPort++;
            if (nextPort > PORT_END) this.nextPort = PORT_START;
            return nextPort;
        }
    }
}
