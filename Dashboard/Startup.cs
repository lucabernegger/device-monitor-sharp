using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using Dashboard.Platforms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dashboard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static string EncryptionKey = "q2xfZnMwEAx1s0RjaGy6jUv3+176YGiaY1pqnAieSp0=";
        public IConfiguration Configuration { get; }
        public static List<WebResponse> Data = new();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddControllers();
            services.AddResponseCompression();
            services.AddMemoryCache();
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
            var timer = new Timer(5000);
            timer.Start();
            timer.Elapsed += async (sender, args) =>
            {
                try
                {
                    using var client = new HttpClient();
                    string json = await client.GetStringAsync($"http://localhost:8000");
                    var response = JsonConvert.DeserializeObject<WebResponse>(json);
                    if (response is not null)
                    {
                        if (response.IsEncrypted)
                        {
                            response.Data = JsonConvert.DeserializeObject<Platform>(StringCipher.Decrypt((string)response.Data, Startup.EncryptionKey));
                        }
                        else
                        {
                           
                            response.Data = JsonConvert.DeserializeObject<Platform>(response.Data.ToString() ?? string.Empty);
          
                        }
                        Data.Add(response);

                    }
                }
                catch (Exception e)
                {
                    
                     Debug.WriteLine(e.Message);
                }
                
            };
        }
    }
}
