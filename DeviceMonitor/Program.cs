using System;

namespace DeviceMonitor
{
    class Program
    {
        static void Main()
        {
            var x = new SystemInfo();
            x.UpdateData();
            Console.WriteLine(x.GetAsJson());

        }
    }
}
