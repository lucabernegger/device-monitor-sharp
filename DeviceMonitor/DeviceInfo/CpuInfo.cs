using System;
using System.Runtime.InteropServices;

namespace DeviceMonitor.DeviceInfo
{
    public class CpuInfo
    {
        /// <summary>
        /// Current Cpu load in percent
        /// </summary>
        public double TotalPercentage { get; set; }
        public int TotalThreads { get; set; }

    }
}
