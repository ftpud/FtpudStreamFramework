using System;
using System.IO;
using System.Threading;
using FtpudStreamFramework.Core;
using FtpudStreamFramework.Settings;
using FtpudStreamFramework.Settings.Encoders;
using FtpudStreamFramework.Settings.Encoders.Qsv;
using FtpudStreamFramework.Settings.Filters;
using FtpudStreamFramework.Target;
using FtpudStreamFramework.Util;
using StreamControlLite.Extensions;
using StreamControlLite.Extensions.WebUi;


namespace StreamControlLite
{
    class Program
    {
        static void Main(string[] args)
        {
            LogUtils.Log(LogLevel.Debug,"Init");

            StreamSettings.FfmpegConverter = "ffmpeg_lite";
            //StreamSettings.FfmpegPublisher = "ffmpeg_pub";
            StreamSettings.LogLevel = LogLevel.Debug;
            
            StreamSettings.AudioEncoder = new AacAudioEncoder(320, 44100);
            
            //StreamSettings.VideoEncoder = new MacM1HWVideoEncoder(4000, 30); //mac
            //StreamSettings.VideoEncoder = new NvencVideoEncoder(4000, 30);
            StreamSettings.VideoEncoder = new QsvHwVideoEncoder(5000, 30, true);
            
            Decorator.instance().AddGlobalFilter(new ScaleFilter(1280, 720));
            Decorator.instance().AddGlobalFilter(new PadFilter(1280, 720));
            // Decorator.instance().AddGlobalFilter(new Deinterlace());

            Extensions.InfoBox.instance().Init();
            
            Interconnection.instance().Init();
            Transmitter.instance().Init();

            Publisher publisher = new Publisher();
            
            publisher.RunPublisher(new StreamRtmpTarget("rtmp://192.168.0.129/tg"));
            //publisher.RunPublisher(new StreamRtmpTarget("rtmp://live.restream.io/live/re_5072685_fd0af45d4a0f980bde37"));

           // publisher.RunPublisher(new StreamRtmpTarget(File.ReadAllText("stream_link.txt")));
            
            MediaLibrary.instance().Init();
            // MediaLibrary.instance().Load("/Users/ftpud/Downloads/"); // mac
            // MediaLibrary.instance().Load("Z:\\_idoling\\dl"); // win
            // MediaLibrary.instance().Load("\\\\192.168.0.129\\akb\\_1TB\\"); // network drive
            // X:\_500GB\wasutahq
            
            //MediaLibrary.instance().Load("\\\\192.168.0.129\\akb\\_500GB\\wasutahq\\");
            //MediaLibrary.instance().Load("/Volumes/homes/_500GB/wasutahq");
            MediaLibrary.instance().Load("/mnt/500gb_2/anime/RequestOnly/op/");
            //MediaLibrary.instance().Load("/mnt/500gb/wasutahq/");

            WebUi.instance().Init(8088);

            
            // Start
            MediaLibrary.instance().Start();


            while (true)
            {
                Thread.Sleep(1000);
            }
            
        }
    }
}