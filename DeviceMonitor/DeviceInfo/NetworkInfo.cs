using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace DeviceMonitor.DeviceInfo
{
    public class NetworkInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long BytesReceived { get; set; }
        public long BytesSent { get; set; }
        public long IncomingErrors { get; set; }
        public long OutgoingErrors { get; set; }
        public long OutgoingDiscarded { get; set; }
        public bool IsReceiveOnly { get; set; }
        public List<Ip> Ips { get; set; }

        public static List<NetworkInfo> Parse(NetworkInterface[] networkInterfaces)
        {
            var networkInfos = new List<NetworkInfo>();

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
                networkInfos.Add(new()
                {
                    BytesReceived = networkInterface.GetIPStatistics().BytesReceived,
                    BytesSent = networkInterface.GetIPStatistics().BytesSent,
                    IncomingErrors = networkInterface.GetIPStatistics().IncomingPacketsWithErrors,
                    OutgoingErrors = networkInterface.GetIPStatistics().OutgoingPacketsWithErrors,
                    OutgoingDiscarded = networkInterface.GetIPStatistics().OutgoingPacketsDiscarded,
                    Description = networkInterface.Description,
                    IsReceiveOnly = networkInterface.IsReceiveOnly,
                    Name = networkInterface.Name,
                    Ips = ips
                });
            }

            return networkInfos;
        }
    }

    public class Ip
    {
        public string Address { get; set; }
    }
}
