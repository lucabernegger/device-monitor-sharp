﻿namespace Dashboard
{
    public class Settings
    {
        public string Url { get; set; } = "http://localhost:8000/";
        public string EncryptionKey { get; set; }
        public bool EncryptionEnabled { get; set; } = false;
        public int StoreDatabaseInterval { get; set; } = 10000;
        public string ConnectionString { get; set; }
    }
}
