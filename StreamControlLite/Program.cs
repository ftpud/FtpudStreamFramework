using System;
using System.Collections.Generic;
using StreamControlLite.Core;
using StreamControlLite.Settings;
using StreamControlLite.Settings.Encoders;
using StreamControlLite.Settings.Filters;
using StreamControlLite.Source;
using StreamControlLite.Target;
using StreamControlLite.Util;

namespace StreamControlLite
{
    class Program
    {
        static void Main(string[] args)
        {
            LogUtils.Log(LogLevel.Verbose,"Init");

            StreamSettings.AudioEncoder = new AacAudioEncoder(320, 44100);
            StreamSettings.VideoEncoder = new NvencVideoEncoder(8000, 30);
            
            Decorator.instance().AddGlobalFilter(new ScaleFilter(1920, 1080));
            Decorator.instance().AddGlobalFilter(new PadFilter(1920, 1080));
            
            Extensions.InfoBox.instance().Init();
            
            Interconnection.instance().Init();
            Transmitter.instance().Init();

            Publisher publisher = new Publisher();
            publisher.RunPublisher(new StreamRtmpTarget("rtmp://192.168.0.129/test"));

            List<FileSourceEntity> playList = new List<FileSourceEntity>()
            {
                new FileSourceEntity("Z:\\_idoling\\dl\\Task have Fun - 3WD (MV).mp4"),
                new FileSourceEntity("Y:\\wasutahq\\wasuta - Meranyaizar [HQ].ts"),
                new FileSourceEntity("Q:\\_dav\\RequestOnly\\Geneki Idol ga Erabu! Shimakura Rika  Ririka - Scream.mp4"),
                new FileSourceEntity("Z:\\_idoling\\dl\\Task have Fun - 星フルWISH (MV).mp4"),
                new FileSourceEntity("Z:\\_idoling\\dl\\Task have Fun - Kimi nanda kara (MV).mp4"),
            };

            int listNum = 0;
            InputProcessor.Instance().OnFinished += (sender, eventArgs) =>
            {
                listNum++;
                if (listNum == playList.Count) listNum = 0;
                InputProcessor.Instance().Play(playList[listNum]);
            };
            InputProcessor.Instance().Play(playList[listNum]);
            

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