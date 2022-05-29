using System;

namespace FtpudStreamFramework.Settings.Filters
{
    public class AssSubtitlesFilter : VideoFilter
    {
        private string _filename;
        
        public AssSubtitlesFilter(string filename)
        {
            _filename = filename;
        }

        public override string GetFilterCommandLine()
        {
            return $"ass='{_filename}'";
        }
    }
}