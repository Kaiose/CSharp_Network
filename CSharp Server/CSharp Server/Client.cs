using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CSharp_Server
{
    sealed class Client : IDisposable
    {
        private bool alreadyDispose = false;
        private Socket socket;

        
        public void Dispose()
        {
            if (alreadyDispose)
                return;

            Disconnect();
            //socket.Dispose(); <-
            alreadyDispose = true;

            GC.SuppressFinalize(this);

            
        }

        public void Disconnect()
        {
            socket.Disconnect(true);
            socket.Close();  
            //socket.dispose() = Close 의 Timeout이 없는 것과 동일함 
        }
        
        
        

    }
}
