using System;

namespace FtpudStreamFramewok.Settings.Encoders
{
    public class BaseVideoEncoder
    {
        public virtual String GetEncoderCommandLine()
        {
            throw new NotImplementedException();
        }
    }
}