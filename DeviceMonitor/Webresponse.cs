using System;

namespace DeviceMonitor
{
    public class WebResponse
    {
        public DateTime Time { get; set; } = DateTime.Now;
        public object Data { get; set; }
        public bool IsEncrypted { get; set; }
    }
}
