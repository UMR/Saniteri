using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace UMR.Saniteri.Communication
{
    public abstract class BasedNetworkConnection
    {
        public string commandName { get; set; }
        public bool cancelAnytime { get; set; }
        protected InitializerNetwork initializer { get; set; }
        protected int acknowledgeWait { get; set; }
        protected bool responseWait { get; set; }
        protected bool newPacket { get; set; }
        /// <summary>
        /// Length calculated from response
        /// </summary>
        protected int packetLength { get; set; }
        public byte[] response { get; protected set; }
        public byte[] acknowledge { get; protected set; }
        /// <summary>
        /// Timeout second
        /// </summary>
        protected double timeout { get; set; }
        protected double sendWait { get; set; }
        //
        protected int? length { get; set; }
        protected bool checksum { get; set; }
        protected byte[] command { get; set; }

        protected BasedNetworkConnection(string commandName)
        {
            this.commandName = commandName;
            this.timeout = 10;
            this.sendWait = .1;
            this.cancelAnytime = true;
        }

        public BasedNetworkConnection(InitializerNetwork initializer, string commandName)
            : this(commandName)
        {
            this.initializer = initializer;
        }

        protected virtual void reset()
        {
            this.acknowledge = null;
            this.response = null;
            this.packetLength = 0;
            this.checksum = false;
            this.responseWait = true;
            this.acknowledgeWait = 0;
            this.newPacket = true;
            this.initializer.busy = false;
            this.initializer.success = true;
            this.initializer.timeout = true;
        }

        protected abstract void createCommand();

        #region Initialization

        public virtual InitializerNetwork initialize()
        {
            try
            {
                this.initializer.socket = new Socket(AddressFamily.InterNetwork, this.initializer.socketType, this.initializer.protocolType);
                this.initializer.socket.Bind(new IPEndPoint(ProtocolHelper.protocolHelper.interfaceIPAddress, ProtocolHelper.protocolHelper.getNextPort()));
                this.initializer.socket.Connect(new IPEndPoint(IPAddress.Parse(this.initializer.IP), this.initializer.remotePort));
                this.initializer.initialized = true;
            }
            catch (Exception ex)
            {
                this.initializer.message = ex.Message;
            }
            return this.initializer;
        }

        #endregion

        #region Communication

        /// <summary>
        /// Communicate with Can Device
        /// </summary>
        /// <param name="initializer"></param>
        /// <param name="retry">Number of times to retry</param>
        /// <param name="wait">Wait second for next try</param>
        /// <returns></returns>
        internal virtual bool communicate(InitializerNetwork initializer, int retry, double wait = 1)
        {
            while (true)
            {
                if (this.communicate(initializer) && !this.initializer.busy) return true;
                if (!this.checkNext(ref retry, wait)) return false;
            }
        }

        /// <summary>
        /// Communicate with gateway
        /// </summary>
        /// <param name="retry">Number of times to retry</param>
        /// <param name="wait">Wait second for next try</param>
        /// <returns></returns>
        public virtual bool communicate(int retry = 1, double wait = 1)
        {
            while (true)
            {
                if (this.communicate() && !this.initializer.busy) return true;
                if (!this.checkNext(ref retry, wait)) return false;
            }
        }

        private bool checkNext(ref int retry, double wait)
        {
            if (--retry <= 0 || (this.initializer.cancel != null && this.initializer.cancel.IsCancellationRequested) || !this.initializer.initialized || this.initializer.security)
            {
                return false;
            }
            Thread.Sleep(TimeSpan.FromSeconds(wait / 3));
            Thread.Sleep(TimeSpan.FromSeconds(wait / 3));
            Thread.Sleep(TimeSpan.FromSeconds(wait / 3));
            return true;
        }

        public virtual bool communicate(InitializerNetwork initializer)
        {
            this.initializer = initializer;
            if (this.initializer.cancel != null && this.initializer.cancel.IsCancellationRequested) return false;
            this.reset();
            this.createCommand();
            //                        
            if (!this.initializer.socket.Poll((int)this.timeout, SelectMode.SelectWrite)) return this.initializer.success = false;
            if (this.initializer.protocolType == ProtocolType.Tcp)
                this.initializer.socket.Send(this.command);
            else this.initializer.socket.Send(this.command);

            if (this.sendWait > 0) Thread.Sleep(TimeSpan.FromSeconds(this.sendWait));
            while (this.initializer.socket.readWait(this.timeout, this.initializer.cancel, this.cancelAnytime))
            {
                this.initializer.timeout = false;
                byte[] buffer = new byte[this.initializer.socket.Available];
                this.initializer.socket.Receive(buffer);
                if (!this.processData(buffer)) break;
                this.initializer.timeout = true;
            }
            return this.processResponse();
        }

        protected virtual bool communicate()
        {
            using (this.initializer = this.initialize())
            {
                if (this.initializer.initialized) return this.communicate(this.initializer);
            }
            return false;
        }

        #region Processing Data

        protected bool isAcknowledge(byte[] response)
        {
            return response != null && response.Length > 0 && response[0] == (byte)DeviceCommandTypes.deviceACK;
        }

        protected bool processData(byte[] buffer)
        {
            this.response = this.response == null ? buffer : response.Concat(buffer).ToArray();
            if (this.newPacket && this.initializer.protocolType == ProtocolType.Tcp)
            {
                if (!this.calculatePacketLenth()) return true;
            }
            if (this.response.Length >= this.packetLength || this.initializer.protocolType != ProtocolType.Tcp)
            {
                if (!this.checkResponse(this.response[0])) return false;
                if (this.isAcknowledge(this.response))
                {
                    this.newPacket = true;
                    this.acknowledge = this.response.Take(packetLength).ToArray();
                    if (!this.validateChecksum(this.acknowledge)) return false;
                    if (--this.acknowledgeWait > 0) return true;
                    if (this.responseWait)
                    {
                        buffer = this.response.Skip(packetLength).ToArray();
                        this.response = null;
                        if (buffer.Length > 0) this.processData(buffer);
                    }
                }
                else
                {
                    this.newPacket = true;
                    this.responseWait = false;
                    this.validate(this.response);
                }
            }
            else
            {
                this.newPacket = false;
                return true;
            }
            return this.responseWait;
        }

        #endregion // Processing Data

        protected virtual bool calculatePacketLenth()
        {
            if (this.response.Length < 3) return false;
            this.packetLength = Convert.ToInt32(this.response[2].ToString("X").PadLeft(2, '0') + this.response[3].ToString("X").PadLeft(2, '0'), 16);
            return true;
        }

        #endregion communication

        #region Checksum

        protected bool validateChecksum(byte[] response)
        {
            if (!this.checksum) return true;
            if (response == null || response.Count() == 0 || response.Length < this.packetLength) return this.initializer.success = false;
            var bitCount = this.packetLength - 1;
            int tempLength = 0;
            try
            {
                for (int i = 0; i <= bitCount; i++)
                {
                    tempLength += response[i];
                }
                var tempHex = tempLength.ToString("X").PadLeft(2, '0');
                tempHex = tempHex.Substring(tempHex.Length - 2);
                tempLength = Convert.ToInt32(tempHex, 16);
                long checkSum = response[bitCount + 1];
                this.initializer.success = checkSum == tempLength;
            }
            catch { this.initializer.success = false; }
            return this.initializer.success;
        }

        protected void makeCheckSum()
        {
            var temp = this.command.Length / 255 >= 1 ? (this.command.Length - 255).ToString("X") : (this.command.Length).ToString("X");
            temp = temp.PadLeft(2, '0');
            this.command[this.command.Length-1] = Convert.ToByte(temp.Substring(0, 2), 16);
            long checkSum = 0;
            foreach (byte b in command)
            {
                checkSum = (checkSum + b) & 0xFFFF;
            }
            this.checksum = true;
        }

        #endregion

        #region Member Functions

        protected virtual bool validate(byte[] response)
        {
            this.response = response;
            if (this.response == null || this.response.Length == 0)
            {
                this.initializer.success = false;
                return false;
            }
            this.initializer.success = ((this.length == null || this.response.Count() >= this.length));
            if (this.initializer.success) this.validateChecksum(this.response);
            return this.initializer.success;
        }
       
        protected virtual bool processResponse()
        {
            if (this.initializer.timeout) this.initializer.success = false;
            return this.initializer.success;
        }

        protected bool checkResponse(int data)
        {
            DeviceCommandTypes deviceCommand = (DeviceCommandTypes)data;
            if (deviceCommand == DeviceCommandTypes.deviceACK)
            {
                //
            }
            else if (deviceCommand == DeviceCommandTypes.deviceNACK)
            {
                this.initializer.success = false;
            }
            return this.initializer.success;
        }

        #endregion
    }
}
