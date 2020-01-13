using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_Server
{
    interface PacketInterface
    {

        public void Encode()
        {
        
        }


        public void temp()
        {

        }
        public void Decode(byte[] recvBytes, int len);
        


    }
}
