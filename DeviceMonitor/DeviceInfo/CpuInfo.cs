using System.Collections.Generic;

namespace DeviceMonitor.DeviceInfo
{
    public class CpuInfo
    {
        /// <summary>
        /// Current Cpu load in percent
        /// </summary>
        public double TotalPercentage { get; set; }
        public int TotalThreads { get; set; }
        public uint NumberOfCores { get; set; }
        public uint CurrentClockSpeed { get; set; }
        public List<CpuCoreInfo> CpuCores { get; set; }
    }
}
