using System;
using Microsoft.VisualBasic;
using StreamControlLite.Core;
using StreamControlLite.Settings.Decoders;
using StreamControlLite.Util;

namespace StreamControlLite.Settings
{
    public static class StreamSettings
    {
        // User definable settings

        public static BaseVideoDecoder VideoDecoder { get; set; }
        public static BaseAudioDecoder AudioDecoder { get; set; }

        public static LogLevel LogLevel { get; set; } = LogLevel.Verbose;

        public static int InternalCommunicationPort { get; set; } = 5051;
    }
}