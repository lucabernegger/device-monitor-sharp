using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace DeviceMonitor.DeviceInfo
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

        public static NetworkInfo Parse(NetworkInterface[] networkInterfaces,int tcpConnections)
        {
            var networkAdapters = new List<NetworkAdapter>();
            
            foreach (var networkInterface in networkInterfaces)
            {
                var ips = new List<Ip>();
                foreach (var ip in networkInterface.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        
                        ips.Add(new()
                        {
                              Address = ip.Address.ToString(),
                        });
                    }
                }

                var ipStats = networkInterface.GetIPStatistics();
                networkAdapters.Add(new()
                {
                    BytesReceived = ipStats.BytesReceived,
                    BytesSent = ipStats.BytesSent,
                    IncomingErrors = ipStats.IncomingPacketsWithErrors,
                    OutgoingErrors = ipStats.OutgoingPacketsWithErrors,
                    OutgoingDiscarded = ipStats.OutgoingPacketsDiscarded,
                    Description = networkInterface.Description,
                    IsReceiveOnly = networkInterface.IsReceiveOnly,
                    Name = networkInterface.Name,
                    Ips = ips,
                    TcpConnectionCount = tcpConnections
                });
            }

            return new()
            {
                NetworkAdapters = networkAdapters,
                TcpConnections = tcpConnections
            };
        }
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
