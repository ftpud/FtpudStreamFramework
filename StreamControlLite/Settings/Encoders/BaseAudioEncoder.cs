using System;

namespace StreamControlLite.Settings.Decoders
{
    public class BaseAudioEncoder
    {
        public virtual String GetEncoderCommandLine()
        {
            throw new NotImplementedException();
        }
    }
}