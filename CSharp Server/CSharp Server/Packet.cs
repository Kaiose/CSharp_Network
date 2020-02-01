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
        C_REQ_MESSAGE = 100,
        S_ANS_MESSAGE = 101,
    }

    

    public class Packet {

        protected PacketType packetType;
        protected byte[] buffer = new byte[1024];
        public int offset = sizeof(int) + sizeof(PacketType);


        public Packet()
        {
            
        }

        protected virtual void Serialize() { }
        protected virtual void Deserialize(byte[] recvBytes) { }
        public void Encode() // totalLen + typeHeader + data
        {
            offset = sizeof(int) + sizeof(PacketType);
            Serialize();
            Stream.finalSet(buffer, getPacketType(), offset);
        }

        public int Decode(byte[] recvBytes) { // only data
            Deserialize(recvBytes);
            return offset; 
        }
        //public abstract void Decode(byte[] recvBytes, int len);

        
        public static Packet getPacket(PacketType packetType)
        {
            Packet result = null;
            switch (packetType)
            {
                case PacketType.C_REQ_MESSAGE:
                    result = new PK_C_REQ_MESSAGE();
                    break;
                case PacketType.S_ANS_MESSAGE:
                    break;
            }
            return result;
        }

        public PacketType getPacketType() { return packetType; }

    }
    // [size:int][type:int][stream]


    public class PK_C_REQ_MESSAGE : Packet
    {
        
        public string message;
        
        public PK_C_REQ_MESSAGE() 
        {
            this.packetType = PacketType.C_REQ_MESSAGE;
        }
        
        protected override void Serialize()
        {
            Stream.write(buffer,message,ref this.offset);
        }


        protected override void Deserialize(byte[] recvBytes)
        {
            Stream.read(recvBytes, ref message, ref this.offset);
        }

    }


}
