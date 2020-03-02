using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CSharp_Server
{
    sealed class Client : Session
    {

        

        public Client(ProtocolHandler protocolHandler, Action<object, SocketAsyncEventArgs> accept_Completed, Action<object, SocketAsyncEventArgs> receive_Completed) : base(protocolHandler, accept_Completed, receive_Completed)
        {
            Init();
        }

        public Client(ProtocolHandler protocolHandler) : base(protocolHandler)
        {
            Init();
        }

        public override void Init()
        {
            sessionType = SessionType.Client;

            base.Init();
        }

        public void Disconnect()
        {
            socket.Disconnect(true);
            socket.Close();  
            //socket.dispose() = Close 의 Timeout이 없는 것과 동일함 
        }

        

    }
}
