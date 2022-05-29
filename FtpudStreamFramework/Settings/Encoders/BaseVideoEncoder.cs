using System;
using FtpudStreamFramework.Settings.Filters;

namespace FtpudStreamFramework.Settings.Encoders
{
    public class BaseVideoEncoder
    {
        public virtual String GetEncoderCommandLine()
        {
            throw new NotImplementedException();
        }

        public virtual String GetEncoderInitCommand()
        {
            return String.Empty;
        }

        public virtual VideoFilter GetPostFilter()
        {
            return null;
        }
    }
}