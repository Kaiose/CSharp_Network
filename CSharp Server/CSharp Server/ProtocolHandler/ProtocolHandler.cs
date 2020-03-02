using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_Server
{


    public class ProtocolHandler
    {

        protected Delegate[] PacketFunc = new Action<Session, Packet>[ushort.MaxValue];
        public static Packet[] PacketDic = new Packet[ushort.MaxValue];

        protected ProtocolHandler()
        {
            Init_ProtocolHandler();
        }

        public virtual void Init_ProtocolHandler()
        {
              
        }

        public void Call_PacketFunc(Session session, Packet rowPacket)
        {
            ((Action<Session,Packet>)PacketFunc[(int)rowPacket.getPacketType()])(session, rowPacket);
        }

        

    }
}
