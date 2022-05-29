using System;
using System.Threading;
using FtpudStreamFramework.Settings;
using FtpudStreamFramework.Target;
using FtpudStreamFramework.Util;

namespace FtpudStreamFramework.Core
{
    public class Publisher
    {
        public void RunPublisher(StreamTarget target)
        {
            new Thread(new ThreadStart(() =>
            {
                String rtmpTarget = target.ResolveTarget();
                string app = StreamSettings.FfmpegPublisher;
                string otherOptions = " -flags low_delay -movflags +faststart -bsf:v h264_mp4toannexb ";
                string command = $" -loglevel error -i {Interconnection.instance().GetPipeFullPath()} {otherOptions} -c copy -f flv {rtmpTarget}";
                ConsoleUtil.ExecuteBackgroundProcess(app, command);
            })).Start();
        }
    }
}