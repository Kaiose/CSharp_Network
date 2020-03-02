using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CSharp_Server
{
    sealed class ServerSession : Session
    {
        public ServerSession(ProtocolHandler protocolHandler, Action<object, SocketAsyncEventArgs> accept_Completed, Action<object, SocketAsyncEventArgs> receive_Completed) : base(protocolHandler, accept_Completed, receive_Completed)
        {
            Init();
        }

        public ServerSession(ProtocolHandler protocolHandler) : base(protocolHandler)
        {
            Init();
        }

        public override void Init()
        {
            sessionType = SessionType.Server;
            base.Init();
        }
    }
}
