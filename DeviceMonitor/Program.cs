using System;
using System.Linq;
using System.Net.NetworkInformation;
using Newtonsoft.Json;

namespace DeviceMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new SystemInfo();
            x.UpdateData();
            Console.WriteLine(x.GetAsJson());

        }
    }
}
