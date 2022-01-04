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

                SetupBackupProcess(2);

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
                            lastTs = PublishFrame(frame, _lastFrameSent, preservedTs, streamNum);

                            _lastFrameSentDateTime = DateTime.Now;
                            _lastFrameSent = frame;
                            _lastFramePreservedTs = preservedTs;
                            _lastFrameStreamNum = streamNum;
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

        private DateTime _lastFrameSentDateTime = DateTime.Now;
        private FlvFrame _lastFrameSent = null;
        private int _lastFramePreservedTs;
        private int _lastFrameStreamNum;

        private void SetupBackupProcess(int timeout = 4)
        {
            new Thread(new ThreadStart(() =>
            {
                while (AwaitConnections)
                {
                    if (_lastFrameSent != null && DateTime.Now.Subtract(_lastFrameSentDateTime).TotalSeconds >= timeout)
                    {
                        PublishFrame(_lastFrameSent, _lastFrameSent, _lastFramePreservedTs, _lastFrameStreamNum);
                        _lastFrameSentDateTime = DateTime.Now;
                        LogUtils.Log(LogLevel.Verbose, "Backup frame sent");
                    }

                    Thread.Sleep(1000);
                }
            })).Start();
        }

        private static int PublishFrame(FlvFrame frame, FlvFrame previousFrame, int preservedTs, int streamNum)
        {
            int lastTs;
            int ts = FlvUtils.Convert3BytesToUInt24(frame.timestamp);

            if (preservedTs != 0 && frame.type[0] == 18)
            {
                BinaryPrimitives.WriteUInt32BigEndian(frame.previousFrameSize, 16);
            }
            else if (previousFrame != null)
            {
                BinaryPrimitives.WriteUInt32BigEndian(frame.previousFrameSize,
                    10 + (uint)previousFrame.payloadSizeIntBytes);
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
            return lastTs;
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