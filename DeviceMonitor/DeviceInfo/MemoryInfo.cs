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


        public static MemoryInfo Parse(string output,OSPlatform os)
        {
            if (os == OSPlatform.Linux)
            {
                var lines = output.Split(Environment.NewLine);
                var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var swap = lines[2].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                return new ()
                {
                    TotalMemory = Convert.ToDouble(memory[1]),
                    UsedMemory = Convert.ToDouble(memory[2]),
                    FreeMemory = Convert.ToDouble(memory[3]),
                    SharedMemory = Convert.ToDouble(memory[4]),
                    CacheMemory = Convert.ToDouble(memory[5]),
                    AvailableMemory = Convert.ToDouble(memory[6]),
                    TotalSwap = Convert.ToDouble(swap[1]),
                    UsedSwap = Convert.ToDouble(swap[2]),
                    FreeSwap = Convert.ToDouble(swap[3]),
                };
            }

            if (os == OSPlatform.Windows)
            {
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

            return null;
        }
    }
}
