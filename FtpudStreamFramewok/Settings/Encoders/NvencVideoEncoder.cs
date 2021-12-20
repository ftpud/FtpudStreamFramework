namespace FtpudStreamFramewok.Settings.Encoders
{
    public class MacM1HWVideoEncoder : BaseVideoEncoder
    {
        private int _fps = 30;
        private int _bitrate  = 8000;
        
        public override string GetEncoderCommandLine()
        {
            return $"-c:v h264_videotoolbox -r {_fps} -g {_fps*2} -b:v {_bitrate}K";
        }

        public MacM1HWVideoEncoder(int bitrate, int fps)
        {
            _fps = fps;
            _bitrate = bitrate;
        }
    }
}