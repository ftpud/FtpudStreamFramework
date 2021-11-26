using System;

namespace StreamControlLite.Settings.Decoders
{
    public class AacAudioDecoder : BaseAudioDecoder
    {
        private int _bitrate;
        private int _rate;
        
        public AacAudioDecoder(int bitrate, int rate)
        {
            _bitrate = bitrate;
            _rate = rate;
        }

        public override String GetDecoderCommandLine()
        {
            return $" -b:a {_bitrate}k -c:a aac -ar {_rate} -ac 2 ";
        }
    }
}