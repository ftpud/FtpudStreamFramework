using System;
using System.Buffers.Binary;
using System.Linq;

namespace StreamControlLite.Core
{
    public class FlvUtils
    {
        public static int Convert3BytesToUInt24(byte[] b)
        {
            byte[] newBuf = new byte[4] { 0, 0, 0, 0 };
            Buffer.BlockCopy(b, 0, newBuf, 1, 3);
            return unchecked((int)BinaryPrimitives.ReadUInt32BigEndian(newBuf));
        }
        
        
        public static byte[] ConvertUint24To3Bytes(int num)
        {
            byte[] newBuf = new byte[4];
            byte[] buf3b = new byte[3];
            BinaryPrimitives.WriteInt32BigEndian(newBuf, num);
            Buffer.BlockCopy(newBuf, 1, buf3b, 0, 3);
            return buf3b;
        }


        public static byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }

            return rv;
        }
    }
}