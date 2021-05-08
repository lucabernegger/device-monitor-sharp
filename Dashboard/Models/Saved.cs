using System;

#nullable disable

namespace Dashboard.Models
{
    public partial class Saved
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Data { get; set; }
    }
}
