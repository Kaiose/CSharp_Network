using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;


namespace CSharp_Server
{
    
    
    // [size:int][type:int][stream]

    public class PK_CS_MESSAGE_REQ : Packet
    {  
        public string message;
        
        public PK_CS_MESSAGE_REQ() 
        {
            this.packetType = (ushort)ClientProtocol.CS_MESSAGE_REQ;
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
