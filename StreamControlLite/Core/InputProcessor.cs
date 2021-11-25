using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using StreamControlLite.Source;
using StreamControlLite.Util;

namespace StreamControlLite.Core
{
    public class InputProcessor
    {
        private Thread _inputConverterThread;
        
        public void Play(SourceEntity input)
        {
            // kill old thread
            Stop();
            
            // create and start new
            _inputConverterThread = new Thread(new ParameterizedThreadStart(ParameterizedThreadStart));
            _inputConverterThread.Start(input);
        }
        
        public void Stop()
        {
            // kill old thread
            if (_inputConverterProcess != null && !_inputConverterProcess.HasExited)
            {
                // kill
                _converterStdIn.WriteLine("q");
                _converterStdIn.Flush();
            }
        }

        private StreamWriter _converterStdIn;
        Process _inputConverterProcess;
        
        private void ParameterizedThreadStart(object source)
        {
            SourceEntity sourceEntity = (SourceEntity)source;
            Process converterProcess = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();

            _inputConverterProcess = converterProcess;
            
            converterProcess.StartInfo = startInfo;
            
            startInfo.FileName = "ffmpeg";
            startInfo.Arguments = sourceEntity.ProvideSource();
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            //startInfo.UseShellExecute = false;
            converterProcess.StartInfo = startInfo;
            converterProcess.Start();
            _converterStdIn = converterProcess.StandardInput;
            converterProcess.WaitForExit();
            //ConsoleUtil.ExecuteBackgroundProcess("ffmpeg", sourceEntity.ProvideSource());
            if (onFinished != null)
            {
                onFinished.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> onFinished; 

        private static InputProcessor _instance;
        public static InputProcessor Instance()
        {
            return _instance ??= new InputProcessor();
        }
    }
}