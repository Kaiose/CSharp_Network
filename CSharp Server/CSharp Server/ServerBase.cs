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
    class ServerBase
    {

        private IPAddress ipAddress = null;
        private IPEndPoint localEndPoint_for_clients = null;
        private IPEndPoint localEndPoint_for_servers = null;
        private Socket listenSocket_for_clients = null;
        private Socket listenSocket_for_servers = null;
        private Int32 port_for_clients = 0;
        private Int32 port_for_servers = 0;
        protected ConcurrentBag<SocketAsyncEventArgs> AcceptEventPool = new ConcurrentBag<SocketAsyncEventArgs>();
        protected ConcurrentBag<SocketAsyncEventArgs> ReceiveEventPool = new ConcurrentBag<SocketAsyncEventArgs>();
        
        readonly private ushort clients_backlog = 1000;
        readonly private ushort servers_backlog = 10;

        protected bool shutdown = false;
        public ServerBase(string IP = "127.0.0.1", Int32 port_clients = 9000, Int32 port_servers = 9100)
        {
            ipAddress = IPAddress.Parse(IP);
            localEndPoint_for_clients = new IPEndPoint(ipAddress, port_clients);
            localEndPoint_for_servers = new IPEndPoint(ipAddress, port_servers);

            listenSocket_for_clients = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenSocket_for_servers = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }


        public virtual void Init()
        {
            Bind();
            Listen();
        }

        private void Bind()
        {
            try
            {
                listenSocket_for_clients.Bind(localEndPoint_for_clients);
                listenSocket_for_servers.Bind(localEndPoint_for_servers);
            }
            catch(SocketException e)
            {
                Console.WriteLine($"Bind Error! error code : {e.SocketErrorCode} Message : {e.Message}");
            }
        }

        private void Listen()
        {
            try
            {
                listenSocket_for_clients.Listen(clients_backlog);
                listenSocket_for_servers.Listen(servers_backlog);
            }
            catch (SocketException e)
            {
                Console.WriteLine($"Listen Error! error code : {e.SocketErrorCode} Message : {e.Message}");
            }
        }


        // Type 종류가 명확하다. 팩토리제작 안함. Type 증가시 팩토리제작하겠음
        protected void Accept(Session session)
        { 
            if(session.sessionType == SessionType.Client)
            {
                if (false == listenSocket_for_clients.AcceptAsync(session.acceptEvent))
                {
                    Console.WriteLine("Clients Accept Error!");
                }

            }
            else if(session.sessionType == SessionType.Server)
            {
                if( false == listenSocket_for_servers.AcceptAsync(session.acceptEvent))
                {
                    Console.WriteLine("Servers Accept Error!");
                }
            }
            
        }






        


    }
}
