using System;
using System.Threading;
using FtpudStreamFramewok.Target;
using FtpudStreamFramewok.Util;

namespace FtpudStreamFramewok.Core
{
    public class Publisher
    {
        public void RunPublisher(StreamTarget target)
        {
            new Thread(new ThreadStart(() =>
            {
                String rtmpTarget = target.ResolveTarget();
                string app = "ffmpeg";
                string otherOptions = " -flags low_delay -movflags +faststart -bsf:v h264_mp4toannexb ";
                string command = $" -loglevel error -i \\\\.\\pipe\\{Interconnection.instance().InterPipeName} {otherOptions} -c copy -f flv {rtmpTarget}";
                ConsoleUtil.ExecuteBackgroundProcess(app, command);
            })).Start();
        }
    }
}