using System;
using System.Threading.Tasks;

namespace DeviceMonitor
{
    class Program
    {
        static async Task Main()
        {
            var x = new SystemInfo();
            x.UpdateData();
            var server = new Webserver("http://localhost:8010/", () =>
            {
                x.UpdateData();

                return x.GetAsJson();
            });
            await server.Start();


        }
    }
}
