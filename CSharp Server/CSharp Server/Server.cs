using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Collections.Concurrent;
using System.Buffers;

namespace CSharp_Server
{
    class Server : ServerBase
    {
        
        public Server() : base()
        {
        }

        

        public override void Init()
        {
            
            SessionManager.instance.Regist_Create_Session_Func(SessionType.Client,
                () => { return new Client(new ClientProtocolHandler()); });

            SessionManager.instance.Regist_Create_Session_Func(SessionType.Server,
                () => { return new ServerSession(new ServerSessionProtocolHandler()); });


            SessionManager.instance.Make_Session(SessionType.Client, 100);
            SessionManager.instance.Make_Session(SessionType.Server, 10);


            base.Init();
        }



        public void Start_Accept()
        {
            foreach(var keyValue in SessionManager.instance.closedSessions)
            {
                Accept(keyValue.Value);
            }

        }



    }
}
