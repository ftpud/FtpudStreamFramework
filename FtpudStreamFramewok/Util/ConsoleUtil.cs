using System;
using System.Diagnostics;

namespace FtpudStreamFramewok.Util
{
    public class ConsoleUtil
    {
        public static void ExecuteBackgroundProcess(string application, String args)
        {
            LogUtils.Log(LogLevel.Trace, $"Starting process: {application}");
            LogUtils.Log(LogLevel.Trace, $"Args: {args}");
            
            var process = new Process();
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = application,
                Arguments = args
            };

            ExecuteBackgroundProcess(process, processStartInfo);
        }
        
        public static void ExecuteBackgroundProcess(Process process, ProcessStartInfo startInfo)
        {
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }
    }
}