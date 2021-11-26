using System;

namespace StreamControlLite.Settings.Decoders
{
    public class BaseAudioDecoder
    {
        public virtual String GetDecoderCommandLine()
        {
            throw new NotImplementedException();
        }
    }
}