using System;
using System.Buffers.Binary;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace StreamControlLite.Core
{

    public class FlvFrame
    {
        
        public byte[] previousFrameSize { get; set; }
        public byte[] type { get; set; }
        public byte[] payloadSize { get; set; }
        public byte[] timestamp { get; set; }
        public byte[] timestampExtended { get; set; }
        public byte[] streamId { get; set; }
        public byte[] data { get; set; }
        
        public byte[] CombineFrame => FlvUtils.Combine(
            previousFrameSize, 
            type, 
            payloadSize, 
            timestamp, 
            timestampExtended, 
            streamId, 
            data);
    }
    

    public class FlvProcessor
    {
        public static byte[] ReceiveHeader(Socket socket)
        {
            return ReceiveDataFrame(socket, 9);
        }
        
        public static bool CheckHeader(byte[] header)
        {
            string signature = Encoding.ASCII.GetString(header, 0, 3);
            return "FLV" == signature;
        }
        
        public static FlvFrame ReadFlvFrame(Socket socket)
        {
            byte[] sizeOfPrevPacket = ReceiveDataFrame(socket, 4);
            byte[] packetType = ReceiveDataFrame(socket, 1);
            byte[] payloadSize = ReceiveDataFrame(socket, 3);
            byte[] timestampLower = ReceiveDataFrame(socket, 3);
            byte[] timestampUpper = ReceiveDataFrame(socket, 1);
            byte[] streamId = ReceiveDataFrame(socket, 3);

            int payloadSizeIntBytes = FlvUtils.Convert3BytesToUInt24(payloadSize);
            byte[] payloadData = ReceiveDataFrame(socket, payloadSizeIntBytes);

            //byte[] headerData = FlvUtils.Combine(sizeOfPrevPacket, packetType, payloadSize, timestampLower, timestampUpper, streamId);
            
            
            //int curentTs = convertUint24(timestampLower);
            //byte[] newTs = convertUint24ToBytes(streamTs);
            
            /*if (packetType[0] == 18)
            {
                Console.Write($"AMF: {payloadData[0]}\t");
                if (startingTimestamp != 0)
                {
                    BinaryPrimitives.WriteUInt32BigEndian(sizeOfPrevPacket, 16);
                }
            }*/

            return new FlvFrame()
            {
                payloadSize = payloadSize,
                streamId = streamId,
                data = payloadData,
                previousFrameSize = sizeOfPrevPacket,
                timestamp = timestampLower,
                timestampExtended = timestampUpper,
                type = packetType
            };
        }
        

        private static int BufferSize = 1024;
        private static byte[] ReceiveDataFrame(Socket socket, int size)
        {
            byte[] messageBuffer = new byte[size];

            int bytesReceived = 0;
            int totalBytesReceived = 0;
            do
            {
                byte[] buffer = new byte[BufferSize];

                // Receive at most the requested number of bytes, or the amount the 
                // buffer can hold, whichever is smaller.
                int toReceive = Math.Min(size - totalBytesReceived, BufferSize);
                bytesReceived = socket.Receive(buffer, toReceive, SocketFlags.None);

                // Copy the receive buffer into the message buffer, appending after 
                // previously received data (totalBytesReceived).
                Buffer.BlockCopy(buffer, 0, messageBuffer, totalBytesReceived, bytesReceived);

                totalBytesReceived += bytesReceived;
            } while (bytesReceived > 0);

            if (totalBytesReceived < size)
            {
                throw new Exception("Server closed connection prematurely");
            }

            return messageBuffer;
        }
    }
}
