using System;

namespace FtpudStreamFramewok.Settings.Encoders
{
    public class BaseAudioEncoder
    {
        public virtual String GetEncoderCommandLine()
        {
            throw new NotImplementedException();
        }
    }
}