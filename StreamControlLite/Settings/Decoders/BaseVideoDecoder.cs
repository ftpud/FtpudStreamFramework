using System;

namespace StreamControlLite.Settings.Decoders
{
    public class BaseVideoDecoder
    {
        public virtual String GetDecoderCommandLine()
        {
            throw new NotImplementedException();
        }
    }
}