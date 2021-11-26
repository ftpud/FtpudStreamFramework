namespace StreamControlLite.Settings.Filters
{
    public class PadFilter : VideoFilter
    {
        private int _width;
        private int _height;
        
        public PadFilter(int Width, int Height)
        {
            _width = Width;
            _height = Height;
        }

        public override string GetFilterCommandLine()
        {
            return $"pad={_width}:{_height}:({_width}-iw*min({_width}/iw\\,{_height}/ih))/2:({_height}-ih*min({_width}/iw\\,{_height}/ih))/2";
        }
    }
}