using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.VisualBasic;
using StreamControlLite.Settings.Filters;
using StreamControlLite.Util;

namespace StreamControlLite.Extensions
{
    public class InfoMessage
    {
        public String Message { get; set; }
        public int Timeout { get; set; }
    }

    public class InfoBox
    {
        public void Init()
        {
            File.WriteAllText("live.txt", "test");
            Decorator.instance().AddGlobalFilter(new TextFilter(new[]
            {
                new TextFilterOption(TextFilterOption.OptionName.textfile, "live.txt"),
                new TextFilterOption(TextFilterOption.OptionName.fontcolor, "white@0.8"),
                new TextFilterOption(TextFilterOption.OptionName.fontsize, "35"),
                new TextFilterOption(TextFilterOption.OptionName.x, "2"),
                new TextFilterOption(TextFilterOption.OptionName.y, "H-th"),
                new TextFilterOption(TextFilterOption.OptionName.box, "1"),
                new TextFilterOption(TextFilterOption.OptionName.boxcolor, "black@0.4"),
                new TextFilterOption(TextFilterOption.OptionName.boxborderw, "4"),
                new TextFilterOption(TextFilterOption.OptionName.reload, "1"),
            }));
            
            SetupTimer();
        }
        

        private List<InfoMessage> messageList = new List<InfoMessage>();

        private void UpdateMessages()
        {
            atomicWrite("live.txt", String.Join("\n", messageList.Select(m => m.Message)));
        }


        public void Push(String message, int time = 10)
        {
            messageList.Add(new InfoMessage() { Message = message, Timeout = time });
            UpdateMessages();
        }

        private void atomicWrite(String filename, String data)
        {
            String tmpFile1 = filename + ".tmp";
            String tmpFile2 = filename + ".tmp2";
            
            try
            {
                File.WriteAllText(tmpFile1, data);
            }
            catch(Exception e)
            {
                LogUtils.Log(LogLevel.Debug, e.Message);
            }

            try
            {
                File.Copy(tmpFile1, tmpFile2, true);
                File.Move(tmpFile2, filename, true);
            }
            catch (Exception e)
            {
                LogUtils.Log(LogLevel.Debug, e.Message);
            }
        }

        private void SetupTimer()
        {
            new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    messageList.ForEach(msg => msg.Timeout--);
                    if (messageList.Any(msg => msg.Timeout < 0))
                    {
                        messageList.RemoveAll(msg => msg.Timeout < 0);
                        UpdateMessages();
                    }
                }
            })).Start();
        }


        private static InfoBox _instance;

        public static InfoBox instance()
        {
            if (_instance == null)
            {
                _instance = new InfoBox();
            }

            return _instance;
        }
    }
}