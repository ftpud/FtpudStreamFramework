using System;
using System.IO;
using FtpudStreamFramewok.Core;
using FtpudStreamFramewok.Settings;
using FtpudStreamFramewok.Settings.Encoders;
using FtpudStreamFramewok.Settings.Filters;
using FtpudStreamFramewok.Target;
using FtpudStreamFramewok.Util;
using StreamControlLite.Extensions;
using StreamControlLite.Extensions.WebUi;


namespace StreamControlLite
{
    class Program
    {
        static void Main(string[] args)
        {
            LogUtils.Log(LogLevel.Debug,"Init");

            //StreamSettings.LogLevel = LogLevel.Trace;
            
            StreamSettings.AudioEncoder = new AacAudioEncoder(320, 44100);
            
            //StreamSettings.VideoEncoder = new MacM1HWVideoEncoder(8000, 30); //mac
            StreamSettings.VideoEncoder = new NvencVideoEncoder(4000, 30);
            
            Decorator.instance().AddGlobalFilter(new ScaleFilter(1280, 720));
            Decorator.instance().AddGlobalFilter(new PadFilter(1280, 720));
            Decorator.instance().AddGlobalFilter(new Deinterlace());
            
            Extensions.InfoBox.instance().Init();
            
            Interconnection.instance().Init();
            Transmitter.instance().Init();

            Publisher publisher = new Publisher();
            
            //publisher.RunPublisher(new StreamRtmpTarget("rtmp://192.168.0.129/test"));
            publisher.RunPublisher(new StreamRtmpTarget(File.ReadAllText("stream_link.txt")));
            
            MediaLibrary.instance().Init();
            // MediaLibrary.instance().Load("/Users/ftpud/Downloads/"); // mac
            // MediaLibrary.instance().Load("Z:\\_idoling\\dl"); // win
            // MediaLibrary.instance().Load("\\\\192.168.0.129\\akb\\_1TB\\"); // network drive
            // X:\_500GB\wasutahq
            
            MediaLibrary.instance().Load("\\\\192.168.0.129\\akb\\_500GB\\wasutahq\\");
            
            WebUi.instance().Init(8080);

            
            // Start
            MediaLibrary.instance().Start();

            

            while (true)
            {
                String input = Console.ReadLine();
                Extensions.InfoBox.instance().Push(input);
                if (input == "next")
                {
                    InputProcessor.Instance().Stop();
                }
            }
            
        }
    }
}