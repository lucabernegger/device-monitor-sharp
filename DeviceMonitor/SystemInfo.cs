using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DeviceMonitor.DeviceInfo;
using DeviceMonitor.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Threading;
using DriveInfo = DeviceMonitor.DeviceInfo.DriveInfo;

namespace DeviceMonitor
{
    class SystemInfo
    {
        /// <summary>
        /// Check if OS is windows
        /// </summary>
        public bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        /// <summary>
        /// Drive Info of System (Free,Used,...)
        /// </summary>
        public List<DriveInfo> Drives { get; set; }

        /// <summary>
        /// Memory & Swap Info of System
        /// </summary>
        public MemoryInfo Ram { get; set; }

        /// <summary>
        /// Cpu Info 
        /// </summary>
        public CpuInfo Cpu { get; set; }

        public List<NetworkInfo> Network { get; set; }

        public void UpdateData()
        {
            UpdateDriveData();
            UpdateRamData();
            UpdateCpuData();
            UpdateNetworkData();
        }

        private void UpdateDriveData()
        {
            if (IsWindows)
            {
                Drives = DriveInfo.Parse(System.IO.DriveInfo.GetDrives());
            }
            else
            {
                ShellHelper.Bash("df -h -x tmpfs | sed 's/  */,/g' > tempDrivesFile.csv");
                var jsonData = CsvHelper.ConvertCsvToJson(File.ReadAllLines("tempDrivesFile.csv"));
                var jsonList = JsonConvert.DeserializeObject<List<JObject>>(jsonData);
                Drives = DriveInfo.Parse(jsonList);
            }
            
        }
        private void UpdateRamData()
        {
            if (IsWindows)
            {
                var output = ShellHelper.Cmd("wmic OS get FreePhysicalMemory,TotalVisibleMemorySize /Value");
                Ram = MemoryInfo.Parse(output,OSPlatform.Windows);
            }
            else
            {
               var output = ShellHelper.Bash("free -m");
               Ram = MemoryInfo.Parse(output,OSPlatform.Linux); 
            }
            
        }
        private void UpdateCpuData()
        {
            if (IsWindows)
            {
                var output = ShellHelper.Cmd("wmic cpu get loadpercentage");
                Cpu = CpuInfo.Parse(output,OSPlatform.Windows);
            }
            else
            {
                var output = ShellHelper.Bash("top -bn1 | grep load | awk '{printf \"%.2f%%\\t\\t\\n\", $(NF-2)}'");
                Cpu = CpuInfo.Parse(output,OSPlatform.Linux);
            }
            
        }

        private void UpdateNetworkData()
        {
            Network = NetworkInfo.Parse(NetworkInterface.GetAllNetworkInterfaces());
        }
        public string GetAsJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
