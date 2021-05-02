using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DeviceMonitor.DeviceInfo;
using DeviceMonitor.Helpers;

namespace DeviceMonitor.Platforms
{
    public class Windows : IPlatform
    {
        public CpuInfo Cpu { get; set; }
        public List<DriveInfo> Drives { get; set; }
        public MemoryInfo Memory { get; set; }
        public NetworkInfo Network { get; set; }
        public bool IsWindows { get; set; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public void UpdateData()
        {
            Cpu = GetCpu();
            Drives = GetDrives().ToList();
            Memory = GetMemory();
            Network = GetNetwork();
        }
        
        private NetworkInfo GetNetwork()
        {
            var tcpConnections = Convert.ToInt32(ShellHelper.Cmd("netstat -nao | find /i \"*\" /c"));

            var networkAdapters = new List<NetworkAdapter>();
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
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

        private MemoryInfo GetMemory()
        {
            var output = ShellHelper.Cmd("wmic OS get FreePhysicalMemory,TotalVisibleMemorySize /Value");
            var lines = output.Trim().Split(Environment.NewLine);
            var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
            var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);

            var total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
            var free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);
            var used = total - free;
            return new()
            {
                TotalMemory = total,
                UsedMemory = used,
                FreeMemory = free,
            };
        }

        private IEnumerable<DriveInfo> GetDrives()
        {
            foreach (var drive in System.IO.DriveInfo.GetDrives())
            {
                yield return new()
                {
                    Identifier = drive.VolumeLabel,
                    Available = Math.Round((double)drive.AvailableFreeSpace / 1000000000),
                    Size = Math.Round((double)drive.TotalSize / 1000000000),
                    Used = drive.TotalSize - drive.AvailableFreeSpace,
                    UsedPercentage = Math.Round(((double)(drive.TotalSize - drive.AvailableFreeSpace) / drive.TotalSize) / 1048576D)
                };
            }
        }

        private CpuInfo GetCpu()
        {
            var output = ShellHelper.Cmd("wmic cpu get loadpercentage");
            var threadCount = Convert.ToInt32(ShellHelper.Powershell("(Get-Process|Select-Object -ExpandProperty Threads).Count").Trim());
            var lines = output.Split(Environment.NewLine);
            using ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            var col = mos.Get();
            var mo = col.OfType<ManagementObject>().FirstOrDefault();
            return new()
            {
                TotalPercentage = Convert.ToDouble(lines[1]),
                TotalThreads = threadCount,
                NumberOfCores = GetPropertyValue<uint>(mo["NumberOfCores"]),
                CurrentClockSpeed = GetPropertyValue<uint>(mo["CurrentClockSpeed"]),
                CpuCores = GetCpuCores().ToList()
            };
        }
        private IEnumerable<CpuCoreInfo> GetCpuCores()
        {
            using var mos = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name != '_Total'");
            

            foreach (var mo in mos.Get())
            {
               
                yield return  new()
                {
                    Name = GetPropertyString(mo["Name"]),
                    PercentProcessorTime = GetPropertyValue<ulong>(mo["PercentProcessorTime"])
                };
            }

        }
        private string GetPropertyString(object obj)
        {
            return (obj is string str) ? str : string.Empty;
        }
        private T GetPropertyValue<T>(object obj) where T : struct
        {
            return (obj == null) ? default(T) : (T)obj;
        }
    }
}
