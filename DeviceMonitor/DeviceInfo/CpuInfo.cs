using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitor.DeviceInfo
{
    public class CpuInfo
    {
        /// <summary>
        /// Current Cpu load in percent
        /// </summary>
        public double LoadPercentage { get; set; }

        public static CpuInfo Parse(string output)
        {
            return new()
            {
                LoadPercentage = Convert.ToDouble(output.Replace("%", string.Empty))
            };
        }
    }
}
