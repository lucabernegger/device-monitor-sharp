using System;
using System.Diagnostics;
using System.IO;

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

        public static string Cmd(string cmd)
        {
            var info = new ProcessStartInfo
            {
                FileName = "cmd.exe", Arguments = $"/C \"{cmd}\"", RedirectStandardOutput = true
            };

            using var process = Process.Start(info);
            return process?.StandardOutput.ReadToEnd();
        }

        public static string Powershell(string cmd)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = @"powershell.exe",
                Arguments = @cmd,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var process = new Process {StartInfo = startInfo};
            process.Start();
            return process.StandardOutput.ReadToEnd();
        }

        public static string[] TryReadFileLines(string path)
        {
            try
            {
                return File.ReadAllLines(path);
            }
            catch
            {
                return Array.Empty<string>();
            }
        }

    }
}
