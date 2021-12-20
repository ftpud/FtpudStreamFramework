using System;
using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using FtpudStreamFramewok.Settings;
using FtpudStreamFramewok.Util;

namespace FtpudStreamFramewok.Core
{
    public class Transmitter
    {
        public bool AwaitConnections { get; set; } = true;

        public void Init()
        {
            new Thread(new ThreadStart(() =>
            {
                LogUtils.Log(LogLevel.Verbose, "Transmitter initialized");
                bool skipHeader = false;
                int preservedTs = 0;

                TcpListener listener = new TcpListener(IPAddress.Any, StreamSettings.InternalCommunicationPort);
                listener.Start();
                int streamNum = 0;

                while (AwaitConnections)
                {
                    bool interrupt = false;
                    int lastTs = 0;
                    TcpClient tcpClient = listener.AcceptTcpClient();
                    try
                    {
                        byte[] header = FlvProcessor.ReceiveHeader(tcpClient.GetStream().Socket);
                        if (!FlvProcessor.CheckHeader(header))
                        {
                            LogUtils.Log(LogLevel.Verbose, "Not flv!");
                            interrupt = true;
                        }

                        if (!skipHeader)
                        {
                            Interconnection.instance().PipeStream.Write(header);
                            skipHeader = true;
                        }

                        while (!interrupt)
                        {
                            FlvFrame frame = FlvProcessor.ReadFlvFrame(tcpClient.GetStream().Socket);

                            int ts = FlvUtils.Convert3BytesToUInt24(frame.timestamp);

                            if (preservedTs != 0 && frame.type[0] == 18)
                            {
                                BinaryPrimitives.WriteUInt32BigEndian(frame.previousFrameSize, 16);
                            }

                            lastTs = preservedTs + ts;
                            frame.timestamp = FlvUtils.ConvertUint24To3Bytes(lastTs);
                            frame.streamId = FlvUtils.ConvertUint24To3Bytes(streamNum);


                            Interconnection.instance().PipeStream.Write(frame.CombineFrame);


                            String log = "";
                            log += ($"{frame.type[0]} ");
                            log += ("DTS:" + FlvUtils.Convert3BytesToUInt24(frame.timestamp) + " ");
                            log += ("PPS:" + BinaryPrimitives.ReadUInt32BigEndian(frame.previousFrameSize) + " ");
                            log += ("CPS:" + FlvUtils.Convert3BytesToUInt24(frame.payloadSize) + " ");
                            LogUtils.Log(LogLevel.Trace, log);


                        }
                    }
                    catch (Exception e)
                    {
                        LogUtils.Log(LogLevel.Debug, "Broken pipe");
                    }
                    preservedTs = lastTs;
                    streamNum++;
                }
            })).Start();
        }


        private static Transmitter _instance;
        public static Transmitter instance()
        {
            if (_instance == null)
            {
                _instance = new Transmitter();
            }

            return _instance;
        }
    }
}