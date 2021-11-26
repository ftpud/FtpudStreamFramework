using System;

namespace StreamControlLite.Settings.Encoders
{
    public class BaseVideoEncoder
    {
        public virtual String GetEncoderCommandLine()
        {
            throw new NotImplementedException();
        }
    }
}