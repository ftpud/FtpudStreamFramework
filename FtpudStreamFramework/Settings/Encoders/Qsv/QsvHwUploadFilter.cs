namespace FtpudStreamFramework.Settings.Filters
{
    public class QsvHwUploadFilter : VideoFilter
    {
        private bool deinterlace = false;

        public QsvHwUploadFilter(bool deinterlaceEnabled)
        {
            deinterlace = deinterlaceEnabled;
        }

        public override string GetFilterCommandLine()
        {
            string filterCmd = "format=nv12,hwupload=extra_hw_frames=64";
            if (deinterlace)
            {
                filterCmd += ",deinterlace_qsv";
            }

            return filterCmd;
        }
    }
}