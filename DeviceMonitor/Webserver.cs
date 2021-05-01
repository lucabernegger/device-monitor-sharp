using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitor
{
    public class Webserver
    {
        private static Func<string> _data;
        private static HttpListener _listener;


        public Webserver(string url,Func<string> data)
        {
            _data = data;
            _listener = new HttpListener();
            _listener.Prefixes.Add(url);
            
        }


        public async Task Start()
        {
            _listener.Start();
            while (true)
            {
                HttpListenerContext ctx = await _listener.GetContextAsync();
                HttpListenerResponse resp = ctx.Response;

                byte[] data = Encoding.UTF8.GetBytes(_data.Invoke());
                resp.ContentType = "text/json";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        }
    }
}
