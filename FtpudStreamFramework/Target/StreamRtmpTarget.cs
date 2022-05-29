using System;

namespace FtpudStreamFramework.Target
{
    public class StreamRtmpTarget : StreamTarget
    {
        private readonly String _rtmpUrl;

        public StreamRtmpTarget(String rtmpUrl)
        {
            _rtmpUrl = rtmpUrl;
        }

        public override string ResolveTarget()
        {
            return _rtmpUrl;
        }
    }
}