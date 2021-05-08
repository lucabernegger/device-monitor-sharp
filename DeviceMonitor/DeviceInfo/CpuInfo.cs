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
        public int NumberOfCores { get; set; }
        public int CurrentClockSpeed { get; set; }
        public List<CpuCoreInfo> CpuCores { get; set; }
    }
}
