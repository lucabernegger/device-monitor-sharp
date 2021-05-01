using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;

namespace DeviceMonitor
{
    class Program
    {
        public static Settings Settings;
        private static Timer _databaseTimer;
        static async Task Main()
        {
            await LoadSettings();
            var info = new SystemInfo();
            info.UpdateData();
            var server = new Webserver(Settings.Url, () =>
            {
                info.UpdateData();
                var data = new WebResponse()
                {
                    IsEncrypted = Settings.EncryptionEnabled,
                    Data = info.GetAsJson()
                };
                if (Settings.EncryptionEnabled && Settings.EncryptionKey.Length > 0)
                {
                    var encrypted = StringCipher.Encrypt(info.GetAsJson(), Settings.EncryptionKey);
                    return JsonConvert.SerializeObject(new WebResponse()
                    {
                        IsEncrypted = true,
                        Data = encrypted
                    });
                }
                return JsonConvert.SerializeObject(new WebResponse()
                {
                    IsEncrypted = false,
                    Data = info
                }); 
            });
            _databaseTimer = new Timer(Settings.StoreDatabaseInterval);
            _databaseTimer.Start();
            _databaseTimer.Elapsed += (sender, args) =>
            {
                if (Settings.EncryptionEnabled && Settings.EncryptionKey.Length > 0)
                {
                    var encrypted = StringCipher.Encrypt(info.GetAsJson(), Settings.EncryptionKey);
                    Database.Add(new()
                    {
                        IsEncrypted = true,
                        Data = encrypted,
                        Time = DateTime.Now
                    });
                }
                else
                {
                    Database.Add(new()
                    {
                        IsEncrypted = false,
                        Data = info,
                        Time = DateTime.Now
                    });
                }
                
            };
            await server.Start();


        }

        static async Task LoadSettings()
        {
            if (!File.Exists("settings.json"))
            {
                await File.WriteAllTextAsync("settings.json", JsonConvert.SerializeObject(new Settings(), Formatting.Indented));
            }
            Settings = JsonConvert.DeserializeObject<Settings>(await File.ReadAllTextAsync("settings.json"));
            if (Settings != null && Settings.Url.Last() != '/')
            {
                Settings.Url += "/";
                await File.WriteAllTextAsync("settings.json", JsonConvert.SerializeObject(Settings, Formatting.Indented));
            }

            if (Settings is not null && Settings.EncryptionKey == null && Settings.EncryptionEnabled)
            {
                using var rng = new RNGCryptoServiceProvider();
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);
                Settings.EncryptionKey = Convert.ToBase64String(tokenData);
                Console.WriteLine("[INFO] No encryptionkey was specified. A new key was generated: " + Settings.EncryptionKey);
                await File.WriteAllTextAsync("settings.json", JsonConvert.SerializeObject(Settings, Formatting.Indented));

            }
        }
    }
}
