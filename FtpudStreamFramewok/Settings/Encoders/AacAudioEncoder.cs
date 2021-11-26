using System;

namespace FtpudStreamFramewok.Settings.Encoders
{
    public class AacAudioEncoder : BaseAudioEncoder
    {
        private int _bitrate;
        private int _rate;
        
        public AacAudioEncoder(int bitrate, int rate)
        {
            _bitrate = bitrate;
            _rate = rate;
        }

        public override String GetEncoderCommandLine()
        {
            return $" -b:a {_bitrate}k -c:a aac -ar {_rate} -ac 2 ";
        }
    }
}