using System.IO.Pipes;
using System.Threading;
using FtpudStreamFramewok.Util;

namespace FtpudStreamFramewok.Core
{
    public class Interconnection
    {
        public string InterPipeName { get; } = "testPipe1";

        public bool AwaitConnections { get; set; } = true;

        public NamedPipeServerStream NamedPipeServerStream => _namedPipeServerStream;
        private NamedPipeServerStream _namedPipeServerStream { get; set; }

        public void Init()
        {
            new Thread(new ThreadStart(() =>
            {
                LogUtils.Log(LogLevel.Verbose,"Awaiting client");
                using (_namedPipeServerStream = new NamedPipeServerStream(InterPipeName, PipeDirection.Out, 1))
                {
                    while (AwaitConnections)
                    {
                        _namedPipeServerStream.WaitForConnection();
                        LogUtils.Log(LogLevel.Verbose,"Named pipe client connected");
                        while (_namedPipeServerStream.IsConnected)
                        {
                            Thread.Sleep(1000);
                        }
                    }
                }
            })).Start();
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