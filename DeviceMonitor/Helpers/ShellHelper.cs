using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitor.Helpers
{
    public static class ShellHelper
    {
        public static string Bash(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        public static string Cmd(this string cmd)
        {
            var output = "";

            var info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.Arguments = $"/C \"{cmd}\"";
            info.RedirectStandardOutput = true;

            using (var process = Process.Start(info))
            {
                return output = process.StandardOutput.ReadToEnd();
            }
            
        }
    }
}
