using System.Collections.Generic;
using DeviceMonitor.DeviceInfo;

namespace DeviceMonitor.Platforms
{
    public interface IPlatform
    {
       public CpuInfo Cpu { get; set; }
       public List<DriveInfo> Drives { get; set; }
       public MemoryInfo Memory { get; set; }
       public NetworkInfo Network { get; set; }
       public bool IsWindows { get; set; }
       public void UpdateData();
    }
}