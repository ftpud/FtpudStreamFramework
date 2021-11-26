using System;

namespace StreamControlLite.Settings.Decoders
{
    public class BaseVideoEncoder
    {
        public virtual String GetEncoderCommandLine()
        {
            throw new NotImplementedException();
        }
    }
}