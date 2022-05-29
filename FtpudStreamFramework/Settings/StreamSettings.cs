using FtpudStreamFramework.Settings.Encoders;
using FtpudStreamFramework.Util;

namespace FtpudStreamFramework.Settings
{
    public static class StreamSettings
    {
        // User definable settings

        public static BaseVideoEncoder VideoEncoder { get; set; }
        public static BaseAudioEncoder AudioEncoder { get; set; }

        public static LogLevel LogLevel { get; set; } = LogLevel.Verbose;

        public static int InternalCommunicationPort { get; set; } = 5051;

        public static string FfmpegConverter = "ffmpeg";
        public static string FfmpegPublisher = "ffmpeg";
    }
}