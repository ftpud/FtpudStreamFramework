using System;
using StreamControlLite.Settings.Filters;

namespace StreamControlLite.Settings
{
    public class CommandLineHelper
    {
        public static String WrapInput(String input)
        {
            String videoCodec = StreamSettings.VideoDecoder.GetDecoderCommandLine();
            String audioCodec = StreamSettings.AudioDecoder.GetDecoderCommandLine();
            String filters = $" -vf \"{Decorator.instance().GetCommandLine()}\" ";
            
            return $"-re -loglevel error {input} {videoCodec} {audioCodec} {filters} -f flv tcp://127.0.0.1:{StreamSettings.InternalCommunicationPort}";
        }
    }
}