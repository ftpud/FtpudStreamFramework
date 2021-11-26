﻿using FtpudStreamFramewok.Settings.Encoders;
using FtpudStreamFramewok.Util;

namespace FtpudStreamFramewok.Settings
{
    public static class StreamSettings
    {
        // User definable settings

        public static BaseVideoEncoder VideoEncoder { get; set; }
        public static BaseAudioEncoder AudioEncoder { get; set; }

        public static LogLevel LogLevel { get; set; } = LogLevel.Verbose;

        public static int InternalCommunicationPort { get; set; } = 5051;
    }
}