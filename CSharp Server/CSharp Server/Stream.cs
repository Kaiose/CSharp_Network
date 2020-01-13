using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Buffers;
namespace CSharp_Server
{
    public class Stream
    {

        public static void finalSet(byte[] buffer, PacketType packetType, int offset )
        {
            writeLength(buffer, offset);
            writeHeader(buffer, packetType);
        }
        public static void write(byte[] buffer, string data, ref int offset)
        {
            byte[] encodedBytes = Encoding.Unicode.GetBytes(data);
            write(buffer, encodedBytes.Length, ref offset);
            Copy(buffer, encodedBytes, ref offset);

        }

        public static void read(byte[] buffer, ref string value, ref int offset)
        {
            int length = 0;
            read(buffer, ref length, ref offset);

            Encoding.Unicode.GetString(buffer, offset, length);
            offset += length;
        }

        public static void write(byte[] buffer, int data, ref int offset)
        {
            byte[] encodedBytes = BitConverter.GetBytes(data);
            Copy(buffer, encodedBytes, ref offset);
        }

        public static void read(byte[] buffer, ref int value, ref int offset) { 
        
            value = BitConverter.ToInt32(buffer, offset);
            offset += sizeof(int);
        }
       

        public static void write(byte[] buffer, float data, ref int offset)
        {
            byte[] encodedBytes = BitConverter.GetBytes(data);
            Copy(buffer, encodedBytes, ref offset);
        }

        public static void read(byte[] buffer, ref float value, ref int offset)
        {
            value = BitConverter.ToSingle(buffer, offset);
            offset += sizeof(float);
        }

        public static void write(byte[] buffer, double data, ref int offset)
        {
            byte[] encodedBytes = BitConverter.GetBytes(data);
            Copy(buffer,encodedBytes, ref offset);
        }

        public static void read(byte[] buffer, ref double value, ref int offset)
        {
            value = BitConverter.ToDouble(buffer, offset);
            offset += sizeof(double);
        }

        public static void write(byte[] buffer, byte[] data, ref int offset)
        {
            Copy(buffer, data, ref offset);
        }

        
        public static void read(byte[] buffer, byte[] value, ref int offset)
        {
            int length = 0;
            read(buffer, ref length, ref offset);

            for (int pos = 0; pos < length; ++pos)
                value[pos] = buffer[offset + pos];

            offset += length;
        }
        

        
        /// <summary>
        /// //////////////////////////////////////////////////////
        /// </summary>
        private static void writeLength(byte[] buffer, int offset)
        {
            byte[] encodedBytes = BitConverter.GetBytes(offset);
            Copy(buffer, encodedBytes, 0);
        }

        private static void writeHeader(byte[] buffer, PacketType packetType)
        {
            byte[] encodedBytes = BitConverter.GetBytes((int)packetType);
            Copy(buffer, encodedBytes, 4);
        }



        private static void Copy(byte[] buffer, byte[] encodedBytes, ref int offset)
        {
            Buffer.BlockCopy(encodedBytes, 0, buffer, offset,encodedBytes.Length);
            offset += encodedBytes.Length;

        }

        private static void Copy(byte[] buffer, byte[] encodedBytes, int pos)
        {
            Buffer.BlockCopy(encodedBytes, 0, buffer, pos, encodedBytes.Length);
        }


    }
}
