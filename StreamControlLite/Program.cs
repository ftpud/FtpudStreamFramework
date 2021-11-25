using System;
using System.Collections.Generic;
using StreamControlLite.Core;
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
            
            Interconnection.instance().Init();
            Transmitter.instance().Init();

            Publisher publisher = new Publisher();
            publisher.RunPublisher(new StreamPreviewRtmpTarget());

            List<FileSourceEntity> playList = new List<FileSourceEntity>()
            {
                new FileSourceEntity("Z:\\_idoling\\dl\\Task have Fun - Kimi nanda kara (MV).mp4"),
                new FileSourceEntity("Z:\\_idoling\\dl\\Task have Fun - 星フルWISH (MV).mp4"),
                new FileSourceEntity("Z:\\_idoling\\dl\\Task have Fun - Kimi nanda kara (MV).mp4"),
                new FileSourceEntity("Z:\\_idoling\\dl\\Task have Fun - 星フルWISH (MV).mp4"),
                new FileSourceEntity("Z:\\_idoling\\dl\\Task have Fun - Kimi nanda kara (MV).mp4"),
            };

            int listNum = 0;
            InputProcessor.Instance().onFinished += (sender, eventArgs) =>
            {
                listNum++;
                if (listNum == playList.Count) listNum = 0;
                InputProcessor.Instance().Play(playList[listNum]);
            };
            InputProcessor.Instance().Play(playList[listNum]);
            

            while (true)
            {
                String input = Console.ReadLine();
                InputProcessor.Instance().Stop();
            }
            
        }
    }
}