using System;
using StreamControlLite.Settings;

namespace StreamControlLite.Util
{
    public enum LogLevel
    {
        Silent = 0,
        Verbose = 1,
        Trace = 2
    }

    public class LogUtils
    {
        public static void Log(LogLevel level, String log)
        {
            if (StreamSettings.LogLevel >= level)
            {
                Console.WriteLine(log);
            }
        }
    }
}