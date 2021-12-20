using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading;
using FtpudStreamFramewok.Util;

namespace FtpudStreamFramewok.Core
{
    public class Interconnection
    {
        public string InterPipeName { get; } = "fsw.pipe";

        public bool AwaitConnections { get; set; } = true;

        public Stream PipeStream => _pipeStream;
        private Stream _pipeStream { get; set; }

        public string GetPipeFullPath()
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "\\\\.\\pipe\\" + InterPipeName;
            }
            else
            {
                return "/tmp/" + InterPipeName;
            }
        }

        public void Init()
        {

            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                new Thread(new ThreadStart(() =>
                {
                    LogUtils.Log(LogLevel.Verbose, "Awaiting client");
                    using (_pipeStream = new NamedPipeServerStream(InterPipeName, PipeDirection.InOut, 1))
                    {
                        while (AwaitConnections)
                        {
                            ((NamedPipeServerStream)_pipeStream).WaitForConnection();
                            LogUtils.Log(LogLevel.Verbose, "Named pipe client connected");
                            while (((NamedPipeServerStream)_pipeStream).IsConnected)
                            {
                                Thread.Sleep(1000);
                            }
                        }
                    }
                })).Start();
            }
            else
            {
                File.Delete(GetPipeFullPath());
                ConsoleUtil.ExecuteBackgroundProcess("mkfifo", GetPipeFullPath());


                new Thread(new ThreadStart(() =>
                {
                    LogUtils.Log(LogLevel.Verbose, "Opening pipe");
                    using (_pipeStream = new FileStream(GetPipeFullPath(), FileMode.Append))
                    {
                        while (AwaitConnections)
                        {
                            LogUtils.Log(LogLevel.Verbose, "Named pipe opened");
                            while (((FileStream)_pipeStream).CanWrite)
                            {
                                Thread.Sleep(1000);
                            }
                        }
                    }
                })).Start();
            }
        }


        private static Interconnection _interconnection;

        public static Interconnection instance()
        {
            if (_interconnection == null)
            {
                _interconnection = new Interconnection();
            }

            return _interconnection;
        }
    }
}