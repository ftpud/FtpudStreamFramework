namespace FtpudStreamFramewok.Settings.Filters
{
    public class ScaleFilter : VideoFilter
    {
        private int _width;
        private int _height;
        
        public ScaleFilter(int Width, int Height)
        {
            _width = Width;
            _height = Height;
        }

        public override string GetFilterCommandLine()
        {
            return $"scale=(iw*sar)*min({_width}/(iw*sar)\\,{_height}/ih):ih*min({_width}/(iw*sar)\\,{_height}/ih)";
        }
    }
}