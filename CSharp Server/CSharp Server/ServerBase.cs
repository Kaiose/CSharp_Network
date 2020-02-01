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
        private IPEndPoint localEndPoint = null;
        private Socket listenSocket = null;
        private ConcurrentBag<SocketAsyncEventArgs> AcceptEventPool = new ConcurrentBag<SocketAsyncEventArgs>();
        private ConcurrentBag<SocketAsyncEventArgs> ReceiveEventPool = new ConcurrentBag<SocketAsyncEventArgs>();
        

        protected bool shutdown = false;
        public ServerBase(int socketPoolCount = 100, string IP = "127.0.0.1", Int32 port = 9000)
        {
            ipAddress = IPAddress.Parse(IP);
            localEndPoint = new IPEndPoint(ipAddress, port);
            listenSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            Init_Pool(socketPoolCount);
            Listen(socketPoolCount);

        }
        
        

        private void Init_Pool(int count = 100)
        {
            SocketAsyncEventArgs AcceptEvent, ReceiveEvent;
            for (int i = 0; i < count; i++)
            {
                
                AcceptEvent = new SocketAsyncEventArgs();
                AcceptEvent.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEvent_Completed);

                ReceiveEvent = new SocketAsyncEventArgs();
                ReceiveEvent.Completed += new EventHandler<SocketAsyncEventArgs>(ReceiveEvent_Completed);

                AcceptEventPool.Add(AcceptEvent);
                ReceiveEventPool.Add(ReceiveEvent);
            }
        }

        private void AcceptEvent_Completed(object sender, SocketAsyncEventArgs e)
        {
            Socket socket = e.AcceptSocket;

            //

            var session = SessionManager.instance.Alloc();
            
            session.socket = socket;

            Console.WriteLine("Accept Complete!!");
            
            SocketAsyncEventArgs receiveEvent;
            ReceiveEventPool.TryTake(out receiveEvent);

            receiveEvent.UserToken = session;            
            
            /*
             * getbuffer from buffer pool
             */ 

            receiveEvent.SetBuffer(session.buffer,session.totalOffset,session.buffer.Length - session.totalOffset);



            socket.ReceiveAsync(receiveEvent);
            
            
        }

        private void ReceiveEvent_Completed(object sender, SocketAsyncEventArgs e)
        {
            Session session = e.UserToken as Session;

            int currentOffset = 0;

            if (session.socket.Connected == true && e.BytesTransferred > 0)
            {


               //Seesion
                PacketType packetType = (PacketType)BitConverter.ToInt32(e.Buffer, sizeof(int));
                Packet packet = Packet.getPacket(packetType);
                currentOffset = packet.Decode(e.Buffer);

                ((Action<Session,Packet>)session.PacketFunc[(int)packetType])(session, packet);
            }

            session.totalOffset += (e.BytesTransferred - currentOffset);
            e.SetBuffer(session.buffer, session.totalOffset, session.buffer.Length - session.totalOffset);

            Console.WriteLine("total offset : {0}, Decoded Size : {1} ",session.totalOffset,currentOffset);

           
            session.socket.ReceiveAsync(e);

            /*
             * Remain Buffer pull 
             */
        }

        protected void Listen(int socketCount)
        {
            listenSocket.Bind(localEndPoint);
            listenSocket.Listen(socketCount);

            SocketAsyncEventArgs acceptEvent;
            while(AcceptEventPool.TryTake(out acceptEvent))
            {
                bool result = listenSocket.AcceptAsync(acceptEvent);
                if(result == false)
                {

                }
            }


        }
    }
}
