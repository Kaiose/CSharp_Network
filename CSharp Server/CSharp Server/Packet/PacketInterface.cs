using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_Server
{

public class Packet
    {
        
        protected ushort packetType;
        protected byte[] buffer = new byte[1024];
        public int offset = sizeof(int) + sizeof(ushort);



        protected virtual void Serialize() { }
        protected virtual void Deserialize(byte[] recvBytes) { }
        public void Encode() // totalLen + typeHeader + data
        {
            offset = sizeof(int) + sizeof(ushort); // size + packetType
            Serialize();
            Stream.finalSet(buffer, getPacketType(), offset);
        }

        public int Decode(byte[] recvBytes)
        { // only data
            Deserialize(recvBytes);
            return offset;
        }

        public virtual ushort getPacketType() { return packetType; }
    }
}
