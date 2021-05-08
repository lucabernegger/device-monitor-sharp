namespace DeviceMonitor
{
    public class Settings
    {
        public string Url { get; set; } = "http://localhost:8000/";
        public string EncryptionKey { get; set; }
        public bool EncryptionEnabled { get; set; } = false;
    }
}
