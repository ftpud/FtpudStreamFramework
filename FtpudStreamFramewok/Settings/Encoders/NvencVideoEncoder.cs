namespace FtpudStreamFramewok.Settings.Encoders
{
    public class NvencVideoEncoder : BaseVideoEncoder
    {
        private int _fps = 30;
        private int _bitrate = 8000;

        public override string GetEncoderCommandLine()
        {
            //return $"-c:v h264_nvenc -preset llhq -profile:v high -rc ll_2pass_quality -zerolatency 1 -force_key_frames \"expr:gte(t,n_forced*2)\" -r {_fps} -g {_fps * 2} -b:v {_bitrate}K";
            
            return $"-c:v h264_nvenc " +
                   $" -preset p6 -tune hq -profile high -rc cbr -2pass true -multipass 1 -rc-lookahead 8 -spatial-aq true  -temporal-aq true -force_key_frames \"expr:gte(t,n_forced*2)\" " +
                   $" -r {_fps} -g {_fps * 2} -b:v {_bitrate}k -minrate:v {_bitrate}k -maxrate:v {_bitrate}k -bufsize:v {_bitrate}k";
        }

        public NvencVideoEncoder(int bitrate, int fps)
        {
            _fps = fps;
            _bitrate = bitrate;
        }
    }
}