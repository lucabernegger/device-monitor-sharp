using System.Collections.Generic;

namespace Dashboard.DeviceInfo
{
    public class NetworkInfo
    {
        /// <summary>
        /// List of network adapters 
        /// </summary>
        public List<NetworkAdapter> NetworkAdapters { get; set; }

        /// <summary>
        /// Current TCP connection count
        /// </summary>
        public int TcpConnections { get; set; }

    }

    public class Ip
    {
        public string Address { get; set; }
    }

    public class NetworkAdapter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long BytesReceived { get; set; }
        public long BytesSent { get; set; }
        public long IncomingErrors { get; set; }
        public long OutgoingErrors { get; set; }
        public long OutgoingDiscarded { get; set; }
        public bool IsReceiveOnly { get; set; }
        public int TcpConnectionCount { get; set; }
        public List<Ip> Ips { get; set; }
    }
}
