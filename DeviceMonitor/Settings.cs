namespace DeviceMonitor
{
    public class Settings
    {
        public string Url { get; set; } = "http://localhost:8000/";
        public string EncryptionKey { get; set; }
        public bool EncryptionEnabled { get; set; } = false;
        public int StoreDatabaseInterval { get; set; } = 10000;
        public string DbHost { get; set; } = "localhost";
        public string DbUser { get; set; }
        public string DbPassword { get; set; }
        public string DbName { get; set; }
    }
}
