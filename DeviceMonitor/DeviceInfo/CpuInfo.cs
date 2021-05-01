using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitor.DeviceInfo
{
    public class CpuInfo
    {
        /// <summary>
        /// Current Cpu load in percent
        /// </summary>
        public double TotalPercentage { get; set; }

        public static CpuInfo Parse(string output,OSPlatform os)
        {
            if (os == OSPlatform.Windows)
            {
                var lines = output.Split(Environment.NewLine);

                return new()
                {
                    TotalPercentage = Convert.ToDouble(lines[1])
                };
            }

            if (os == OSPlatform.Linux)
            {
                return new()
                {
                    TotalPercentage = Convert.ToDouble(output.Replace("%", string.Empty))
                };
            }

            return null;
        }
    }
}
