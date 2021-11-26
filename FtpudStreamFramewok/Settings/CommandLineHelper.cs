using System;
using FtpudStreamFramewok.Settings.Filters;

namespace FtpudStreamFramewok.Settings
{
    public class CommandLineHelper
    {
        public static String WrapInput(String input)
        {
            String videoCodec = StreamSettings.VideoEncoder.GetEncoderCommandLine();
            String audioCodec = StreamSettings.AudioEncoder.GetEncoderCommandLine();
            String filters = $" -vf \"{Decorator.instance().GetCommandLine()}\" ";
            string otherOptions = " -flags low_delay -movflags +faststart -bsf:v h264_mp4toannexb ";
            return $"-re -loglevel error {input} {videoCodec} {audioCodec} {filters} {otherOptions} -f flv tcp://127.0.0.1:{StreamSettings.InternalCommunicationPort}";
        }
    }
}