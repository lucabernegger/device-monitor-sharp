using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Timers;
using Dashboard.Models;
using Newtonsoft.Json;

namespace Dashboard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static Settings Settings;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            LoadSettings();
            services.AddRazorPages();
            services.AddControllers();
            services.AddResponseCompression();
            services.AddMemoryCache();

            services.AddDbContext<ApplicationDbContext>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
            var timer = new Timer(Settings.StoreDatabaseInterval);
            timer.Start();
            timer.Elapsed += async (_, _) =>
            {
                try
                {
                    using var client = new HttpClient();
                    string json = await client.GetStringAsync(Settings.Url);
                    var response = JsonConvert.DeserializeObject<WebResponse>(json);
                    if (response is not null)
                    {
                        await using var db = new ApplicationDbContext();
                        if (response.IsEncrypted)
                        {
                            response.Data = StringCipher.Decrypt((string)response.Data, Settings.EncryptionKey);
                        }

                        db.Saveds.Add(new()
                        {
                            Time = response.Time,
                            Data = JsonConvert.SerializeObject(response.Data)
                        });
                        await db.SaveChangesAsync();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }

            };
        }

        static void LoadSettings()
        {
            if (!File.Exists("settings.json"))
            {
                File.WriteAllText("settings.json", JsonConvert.SerializeObject(new Settings(), Formatting.Indented));
            }
            Settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("settings.json"));
            if (Settings != null && Settings.Url.Last() != '/')
            {
                Settings.Url += "/";
                File.WriteAllText("settings.json", JsonConvert.SerializeObject(Settings, Formatting.Indented));
            }

            if (Settings is not null && Settings.EncryptionKey == null && Settings.EncryptionEnabled)
            {
                using var rng = new RNGCryptoServiceProvider();
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);
                Settings.EncryptionKey = Convert.ToBase64String(tokenData);
                Console.WriteLine("[INFO] No encryptionkey was specified. A new key was generated: " + Settings.EncryptionKey);
                File.WriteAllText("settings.json", JsonConvert.SerializeObject(Settings, Formatting.Indented));

            }
            Debug.WriteLine(Settings.ConnectionString);
        }
    }
}
