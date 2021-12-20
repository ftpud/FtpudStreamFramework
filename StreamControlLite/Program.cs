using System;
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

            StreamSettings.AudioEncoder = new AacAudioEncoder(320, 44100);
            StreamSettings.VideoEncoder = new MacM1HWVideoEncoder(8000, 30);
            
            Decorator.instance().AddGlobalFilter(new ScaleFilter(1920, 1080));
            Decorator.instance().AddGlobalFilter(new PadFilter(1920, 1080));
            
            Extensions.InfoBox.instance().Init();
            
            Interconnection.instance().Init();
            Transmitter.instance().Init();

            Publisher publisher = new Publisher();
            publisher.RunPublisher(new StreamRtmpTarget("rtmp://192.168.0.129/test"));

            
            MediaLibrary.instance().Init();
            MediaLibrary.instance().Load("/Users/ftpud/Downloads/");
            
            
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