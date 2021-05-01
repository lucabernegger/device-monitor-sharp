using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

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

                var ipStats = networkInterface.GetIPStatistics();
                networkInfos.Add(new()
                {
                    BytesReceived = ipStats.BytesReceived,
                    BytesSent = ipStats.BytesSent,
                    IncomingErrors = ipStats.IncomingPacketsWithErrors,
                    OutgoingErrors = ipStats.OutgoingPacketsWithErrors,
                    OutgoingDiscarded = ipStats.OutgoingPacketsDiscarded,
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
