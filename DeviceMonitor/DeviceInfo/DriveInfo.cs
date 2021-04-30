using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMonitor.Helpers;
using Newtonsoft.Json.Linq;

namespace DeviceMonitor.DeviceInfo
{
    class DriveInfo
    {
        /// <summary>
        /// Drive Identifier (/dev/sda1/ || C:\
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Size in GB
        /// </summary>
        public double Size { get; set; }

        /// <summary>
        /// Available disk space in GB
        /// </summary>
        public double Available { get; set; }

        /// <summary>
        /// Used disk space in GB
        /// </summary>
        public double Used { get; set; }

        /// <summary>
        /// Used disk space in percent
        /// </summary>
        public double UsedPercentage { get; set; }

        public static List<DriveInfo> Parse(List<JObject> objList)
        {
            var driveInfos = new List<DriveInfo>();
            foreach (var driveJobject in objList)
            {
                var identString = driveJobject["Filesystem"]?.Value<string>();
                var availableString = driveJobject["Avail"]?.Value<string>();
                var sizeString = driveJobject["Size"]?.Value<string>();
                var usedSting = driveJobject["Used"]?.Value<string>();
                var usedPercentSting = driveJobject["Use%"]?.Value<string>();
                driveInfos.Add(new DriveInfo()
                {
                    Available = UnitConverterHelper.ConvertToUnit(availableString,Unit.Gigabyte),
                    Identifier = identString,
                    Size = UnitConverterHelper.ConvertToUnit(sizeString, Unit.Gigabyte),
                    UsedPercentage = Convert.ToDouble(usedPercentSting.Remove(usedPercentSting.Length-1,1)),
                    Used = UnitConverterHelper.ConvertToUnit(usedSting, Unit.Gigabyte),
                });

            }
            return driveInfos;
        }

        public DriveInfo Parse(JObject obj)
        {
            return new();
        }
    }
}
