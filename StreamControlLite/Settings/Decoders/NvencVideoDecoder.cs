namespace StreamControlLite.Settings.Decoders
{
    public class NvencVideoDecoder : BaseVideoDecoder
    {
        private int _fps = 30;
        private int _bitrate  = 8000;
        
        public override string GetDecoderCommandLine()
        {
            return $"-c:v h264_nvenc -preset llhq -profile:v high -rc ll_2pass_quality -r {_fps} -g {_fps*2} -b:v {_bitrate}K";
        }

        public NvencVideoDecoder(int bitrate, int fps)
        {
            _fps = fps;
            _bitrate = bitrate;
        }
    }
}