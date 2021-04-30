using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMonitor.Helpers;
using Newtonsoft.Json.Linq;

namespace DeviceMonitor.DeviceInfo
{
    public class MemoryInfo
    {
        public double TotalMemory { get; set; }
        public double UsedMemory { get; set; }
        public double FreeMemory { get; set; }
        public double SharedMemory { get; set; }
        public double CacheMemory { get; set; }
        public double AvailableMemory { get; set; }

        public double TotalSwap { get; set; }
        public double UsedSwap { get; set; }
        public double FreeSwap { get; set; }


        public static MemoryInfo Parse(string output)
        {
            var lines = output.Split("\n");
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
    }
}
