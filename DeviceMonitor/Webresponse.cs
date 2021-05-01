using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitor
{
    public class WebResponse
    {
        public DateTime Time { get; set; } = DateTime.Now;
        public object Data { get; set; }
        public bool IsEncrypted { get; set; }
    }
}
