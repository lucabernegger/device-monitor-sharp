using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DeviceMonitor.DeviceInfo;
using DeviceMonitor.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DriveInfo = DeviceMonitor.DeviceInfo.DriveInfo;

namespace DeviceMonitor.Platforms
{
    public class Linux : IPlatform
    {
        public CpuInfo Cpu { get; set; }
        public List<DriveInfo> Drives { get; set; }
        public MemoryInfo Memory { get; set; }
        public NetworkInfo Network { get; set; }
        public bool IsWindows { get; set; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public void UpdateData()
        {
            Memory = GetMemory();
            Cpu = GetCpu();
            Drives = GetDrive().ToList();
            Network = GetNetwork();
        }

        private IEnumerable<DriveInfo> GetDrive()
        {
            ShellHelper.Bash("df -h -x tmpfs | sed 's/  */,/g' > tempDrivesFile.csv");
            var jsonData = CsvHelper.ConvertCsvToJson(File.ReadAllLines("tempDrivesFile.csv"));
            var jsonList = JsonConvert.DeserializeObject<List<JObject>>(jsonData);
            var driveInfo = new DriveInfo();
            foreach (var driveJobject in jsonList)
            {
                var identString = driveJobject["Filesystem"]?.Value<string>();
                var availableString = driveJobject["Avail"]?.Value<string>();
                var sizeString = driveJobject["Size"]?.Value<string>();
                var usedSting = driveJobject["Used"]?.Value<string>();
                var usedPercentSting = driveJobject["Use%"]?.Value<string>();
                yield return new()
                {
                    Available = UnitConverterHelper.ConvertToUnit(availableString, Unit.Gigabyte),
                    Identifier = identString,
                    Size = UnitConverterHelper.ConvertToUnit(sizeString, Unit.Gigabyte),
                    UsedPercentage = Convert.ToDouble(usedPercentSting?.Remove(usedPercentSting.Length - 1, 1)),
                    Used = UnitConverterHelper.ConvertToUnit(usedSting, Unit.Gigabyte),
                };

            }
        }

        private MemoryInfo GetMemory()
        {
            string[] output = ShellHelper.TryReadFileLines("/proc/meminfo");

            return new()
            {
                AvailableMemory = GetMegabytesFromLine(output, "MemFree:"),
                TotalMemory = GetMegabytesFromLine(output, "MemTotal:"),
                FreeMemory = GetMegabytesFromLine(output, "MemFree:"),
                TotalSwap = GetMegabytesFromLine(output, "SwapTotal:"),
                FreeSwap = GetMegabytesFromLine(output, "SwapFree:")
            };
        }

        private CpuInfo GetCpu()
        {
            var output = ShellHelper.Bash("top -bn1 | grep load | awk '{printf \"%.2f%%\\t\\t\\n\", $(NF-2)}'");
            var lines = ShellHelper.TryReadFileLines("/proc/cpuinfo");
            var coreCountRegex = new Regex(@"^cpu cores\s+:\s+(.+)");

            var cpuCoresString = (lines.FirstOrDefault(o => coreCountRegex.Match(o).Success) ?? string.Empty);
            return new()
            {
                TotalPercentage = Convert.ToDouble(output.Replace("%", string.Empty)),
                NumberOfCores = Convert.ToUInt32(coreCountRegex.Match(cpuCoresString).Groups[1].Value)
            };
        }

        private NetworkInfo GetNetwork()
        {
            var tcpConnections = Convert.ToInt32(ShellHelper.Bash("netstat -ant | grep ESTABLISHED | wc -l"));
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
        private double GetMegabytesFromLine(string[] meminfo, string token)
        {

            string? memLine = meminfo.FirstOrDefault(line => line.StartsWith(token) && line.EndsWith("kB"));

            if (memLine != null)
            {
                string mem = memLine.Replace(token, string.Empty).Replace("kB", "K").Trim();
                return UnitConverterHelper.ConvertToUnit(mem, Unit.Megabyte);
            }

            return 0;
        }
    }
}
