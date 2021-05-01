using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitor
{
    public class Settings
    {
        public string Url { get; set; } = "http://localhost:8000";
        public string EncryptionKey { get; set; }
        public bool EncryptionEnabled { get; set; } = false;
    }
}
