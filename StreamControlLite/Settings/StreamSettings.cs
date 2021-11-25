using System;
using Microsoft.VisualBasic;
using StreamControlLite.Core;
using StreamControlLite.Util;

namespace StreamControlLite.Settings
{
    public static class StreamSettings
    {
        // TODO: refactor

        public static LogLevel LogLevel { get; set; } = LogLevel.Verbose;

        public static String WrapInput(String input) =>
            $"{PreInputSettings} {input} {PostInputSettings} tcp://127.0.0.1:5051";

        private static int STREAM_WIDTH = 1280;
        private static int STREAM_HEIGHT = 720;

        public static String PreInputSettings = "-re -loglevel error ";

        public static String PostInputSettings =
            "-c:v h264_nvenc -preset llhq -profile:v high -rc ll_2pass_quality -r 30 -g 60 -b:v 8M" +
            /* SW
             $" -c:v h264" +
            $" -bf 2" +
            $" -b:v 4300k" +
            $" -maxrate:v 4300k" +
            $" -bufsize:v 4300k" +
            $" -r 30" +
            $" -b:a 320k" +
            $" -c:a aac" +
            $" -ar 44100" +
            $" -ac 2" +
            $" -g 60 " +
            $" -movflags +faststart" +
            $" -bsf:v h264_mp4toannexb" +
            $" -level:v 4.2 " +
            $" -profile:v high " +
            $" -g 60 " +
            $" -strict " +
            $" -2 " +*/
            $" -b:a 320k" +
            $" -c:a aac" +
            $" -ar 44100" +
            $" -ac 2" +
            $" -vf \"" +
            $"scale=(iw*sar)*min({STREAM_WIDTH}/(iw*sar)\\,{STREAM_HEIGHT}/ih):ih*min({STREAM_WIDTH}/(iw*sar)\\,{STREAM_HEIGHT}/ih), " +
            $"pad={STREAM_WIDTH}:{STREAM_HEIGHT}:({STREAM_WIDTH}-iw*min({STREAM_WIDTH}/iw\\,{STREAM_HEIGHT}/ih))/2:({STREAM_HEIGHT}-ih*min({STREAM_WIDTH}/iw\\,{STREAM_HEIGHT}/ih))/2" +
            $"\" " +
            $" -f flv";
    }
}