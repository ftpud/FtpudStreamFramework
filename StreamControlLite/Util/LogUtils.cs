using System;
using StreamControlLite.Settings;

namespace StreamControlLite.Util
{
    public enum LogLevel
    {
        Silent = 0,
        Verbose = 4,
        Debug = 8,
        Trace = 12
    }

    public static class LogUtils
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