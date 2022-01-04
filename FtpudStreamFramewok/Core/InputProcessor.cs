using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using FtpudStreamFramewok.Source;

namespace FtpudStreamFramewok.Core
{
    public class InputProcessor
    {
        private Thread _inputConverterThread;
        
        public void Play(SourceEntity input)
        {
            // kill old process
            Stop();
            
            // create and start new
            
            _inputConverterThread = new Thread(new ParameterizedThreadStart(ParameterizedThreadStart));
            _inputConverterThread.Start(input);
        }
        
        public void Stop()
        {
            // kill old process
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
            startInfo.Arguments = sourceEntity.ProvideSourceCommandLine();
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            converterProcess.StartInfo = startInfo;
            converterProcess.Start();
            _converterStdIn = converterProcess.StandardInput;
            converterProcess.WaitForExit();
            if (OnFinished != null)
            {
                OnFinished.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> OnFinished; 

        private static InputProcessor _instance;
        public static InputProcessor Instance()
        {
            return _instance ??= new InputProcessor();
        }
    }
}