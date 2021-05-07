using System;
using System.Collections.Generic;

#nullable disable

namespace DeviceMonitor.Models
{
    public partial class Saved
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Data { get; set; }
        public bool IsEncrypted { get; set; }
    }
}
