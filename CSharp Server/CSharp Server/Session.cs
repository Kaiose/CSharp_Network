using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;
namespace CSharp_Server
{
    public class Session
    {
        public int id;

        public byte[] buffer = new byte[1024];
        public int totalOffset = 0;
        public Socket socket { set; get; }

        public Delegate[] PacketFunc = new Action<Session,Packet>[ushort.MaxValue];
        
        public Session()
        {

            foreach(var protocol in Enum.GetValues(typeof(PacketType)))
            {
                PacketFunc[(int)protocol] = Delegate.CreateDelegate(typeof(Action<Session,Packet>), this, $"Recv_{(PacketType)protocol}");
            }

        }


        public void Send(Packet packet)
        {

        }
        /*
        public virtual void MacthAndRun(Packet packet)
        {
            
            switch (packet.getPacketType())
            {
                case PacketType.C_REQ_MESSAGE:
                    Recv_C_REQ_MESSAGE(packet);
                    break;
            }       
        }
        */
        
        public static void Recv_C_REQ_MESSAGE(Session session, Packet rowPacket)
        {
            PK_C_REQ_MESSAGE packet = rowPacket as PK_C_REQ_MESSAGE;

            Console.WriteLine(packet.message);
        }


    }
}
