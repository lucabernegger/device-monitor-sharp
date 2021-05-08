using System.Collections.Generic;
using Dashboard.DeviceInfo;


namespace Dashboard.Platforms
{
    public class Platform
    {
       public CpuInfo Cpu { get; set; }
       public List<DriveInfo> Drives { get; set; }
       public MemoryInfo Memory { get; set; }
       public NetworkInfo Network { get; set; }
       public bool IsWindows { get; set; }

    }
}