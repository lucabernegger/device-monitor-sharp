using System;
using System.Runtime.InteropServices;
using DeviceMonitor.Platforms;
using Newtonsoft.Json;

namespace DeviceMonitor
{
    public class SystemInfo
    {
        /// <summary>
        /// Check if OS is windows
        /// </summary>
        public bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public DateTime LastDataUpdate = DateTime.Now;
        public IPlatform Platform { get; set; }

        public SystemInfo()
        {
            if (IsWindows)
            {
                Platform = new Windows();
            }
            else
            {
                Platform = new Linux();
            }

            Platform.UpdateData();
        }
        public void Update()
        {
            if (LastDataUpdate.AddMilliseconds(Program.Settings.StoreDatabaseInterval) < DateTime.Now)
            {
                Platform.UpdateData();
                LastDataUpdate = DateTime.Now;
            }
        }
        public string GetAsJson()
        {
            Update();
            return JsonConvert.SerializeObject(Platform);
        }
    }
}
