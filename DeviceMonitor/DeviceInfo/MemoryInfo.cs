using System;
using System.Runtime.InteropServices;

namespace DeviceMonitor.DeviceInfo
{
    public class MemoryInfo
    {
        /// <summary>
        /// Total installed memory in MB
        /// </summary>
        public double TotalMemory { get; set; }
        /// <summary>
        /// Currently used memory in MB
        /// </summary>
        public double UsedMemory { get; set; }

        /// <summary>
        /// Currently free memory in MB
        /// </summary>
        public double FreeMemory { get; set; }

        /// <summary>
        /// Currently linux only
        /// </summary>
        public double SharedMemory { get; set; }

        /// <summary>
        /// Currently linux only
        /// </summary>
        public double CacheMemory { get; set; }

        /// <summary>
        /// Currently linux only
        /// </summary>
        public double AvailableMemory { get; set; }

        /// <summary>
        /// Currently linux only
        /// </summary>
        public double TotalSwap { get; set; }

        /// <summary>
        /// Currently linux only
        /// </summary>
        public double UsedSwap { get; set; }

        /// <summary>
        /// Currently linux only
        /// </summary>
        public double FreeSwap { get; set; }


    }
}
