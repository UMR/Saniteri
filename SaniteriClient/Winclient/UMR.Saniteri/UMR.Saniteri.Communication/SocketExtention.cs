using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace UMR.Saniteri.Communication
{
    public static class SocketExtention
    {
        /// <summary>
        /// Wait until data arrived
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="timeout">Timeout second</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public static bool readWait(this Socket socket, double timeout, CancellationTokenSource cancel, bool cancelAnytime = true)
        {
            var dateTime = DateTime.Now.AddSeconds(timeout);
            while (!socket.Poll(500, SelectMode.SelectRead))
            {
                Thread.Sleep(50);
                if (DateTime.Now.CompareTo(dateTime) > 0) return false;
                if (cancel != null && cancel.IsCancellationRequested && cancelAnytime) return false;
            }
            return socket.Available > 0;
        }
    }
}
