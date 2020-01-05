using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
namespace CSharp_Server
{

    public enum PacketType
    {
        NONE = 0,
        E_C_REQ_MESSAGE = 100,
        E_S_ANS_MESSAGE = 101,
    }

    

    abstract public class Packet     {
        PacketType packetType;
        public int offset;
        protected MemoryStream memoryStream;
        public Packet()
        {
            memoryStream = new MemoryStream(1024);
            packetType = PacketType.NONE;
            
        }

        public abstract void Encode();

        public abstract void Decode(byte[] recvBytes, int len);

        public void setPacketType(PacketType packetType)
        {
            this.packetType = packetType;
        }

        public PacketType getPacketType() { return packetType; }
        public MemoryStream getMemoryStream() { return memoryStream; }
    }
    // [size:int][type:int][stream]


    public class PK_C_REQ_MESSAGE : Packet
    {
        public string message;
        public PK_C_REQ_MESSAGE() 
        {
            setPacketType(PacketType.E_C_REQ_MESSAGE);
        }
        public override void Encode()
        {
            
            byte[] buff = Encoding.Unicode.GetBytes(message);
            int size = sizeof(int) + sizeof(PacketType) + buff.Length;
            memoryStream.Write(BitConverter.GetBytes(size), 0, sizeof(int));
            memoryStream.Write(BitConverter.GetBytes((int)getPacketType()), 0, sizeof(PacketType));
            memoryStream.Write(buff, 0, buff.Length);
            
            

        }

        public override void Decode(byte[] recvBytes,int len)
        {
            int offset = sizeof(int) + sizeof(PacketType);
            message = Encoding.Unicode.GetString(recvBytes,offset,len - offset);
            Console.WriteLine(message);
        }

    }


}
