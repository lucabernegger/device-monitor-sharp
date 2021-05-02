using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Dashboard.DeviceInfo
{
    public class DriveInfo
    {
        /// <summary>
        /// Drives Identifier (/dev/sda1/ || C:\
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

    }
}
