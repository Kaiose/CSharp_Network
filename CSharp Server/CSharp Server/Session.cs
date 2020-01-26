using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;
namespace CSharp_Server
{
    class Session
    {

        public byte[] buffer = new byte[1024];
        public int totalOffset = 0;
        public Socket socket { set; get; }
        

        
        public Session()
        {
        }


        public void Send(Packet packet)
        {

        }

        public virtual void MacthAndRun(Packet packet)
        {
            
            switch (packet.getPacketType())
            {
                case PacketType.E_C_REQ_MESSAGE:
                    C_REQ_MESSAGE(packet);
                    break;
            }       
        }
        
        
        public static void C_REQ_MESSAGE(Packet rowPacket)
        {
            PK_C_REQ_MESSAGE packet = rowPacket as PK_C_REQ_MESSAGE;

            Console.WriteLine(packet.message);
        }


    }
}
