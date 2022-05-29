using FtpudStreamFramework.Settings.Filters;

namespace FtpudStreamFramework.Settings.Encoders.Qsv
{
    public class QsvHwVideoEncoder : BaseVideoEncoder
    {
        private int _fps = 30;
        private int _bitrate = 8000;
        private VideoFilter _postFilter;

        public override string GetEncoderCommandLine()
        {
            var encoderSettings = $" -c:v h264_qsv -bf 2 -profile:v high -g {_fps * 2} -r {_fps} " +
                                  $"-b:v {_bitrate}k -maxrate:v {_bitrate}k -bufsize:v {_bitrate}k -preset veryslow -level:v 4.2 -bsf:v h264_mp4toannexb -copytb 1 ";
            // format=nv12,hwupload=extra_hw_frames=64,deinterlace_qsv
            // level:v 4.2 -profile:v high -g 60 -strict -2 -movflags +faststart -bsf:v h264_mp4toannexb -copytb 1

            return $" {encoderSettings} ";
        }

        public override string GetEncoderInitCommand()
        {
            
            return $"-init_hw_device qsv=hw -filter_hw_device hw -hwaccel qsv -hwaccel_output_format qsv";
        }

        public override VideoFilter GetPostFilter()
        {
            return _postFilter;
        }

        public QsvHwVideoEncoder(int bitrate, int fps, bool deinterlaceEnabled)
        {
            _fps = fps;
            _bitrate = bitrate;
            _postFilter = new QsvHwUploadFilter(deinterlaceEnabled);
        }
    }
}