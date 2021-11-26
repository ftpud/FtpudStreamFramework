using System;

namespace StreamControlLite.Settings.Encoders
{
    public class BaseAudioEncoder
    {
        public virtual String GetEncoderCommandLine()
        {
            throw new NotImplementedException();
        }
    }
}