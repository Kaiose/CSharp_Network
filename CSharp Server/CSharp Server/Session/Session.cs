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
        public SessionType sessionType {
            get;
            protected set;
        }

        public int id;

        public byte[] buffer = new byte[1024];
        public int totalOffset = 0;
        public Socket socket { set; get; }


        private ProtocolHandler protocolHandler = null;


        public SocketAsyncEventArgs acceptEvent = new SocketAsyncEventArgs();
        private SocketAsyncEventArgs receiveEvent = new SocketAsyncEventArgs();

        private Action<object, SocketAsyncEventArgs> accept_Completed = null;
        private Action<object, SocketAsyncEventArgs> receive_Completed = null;


        public Session(ProtocolHandler protocolHandler,
            Action<object, SocketAsyncEventArgs> accept_Completed, Action<object, SocketAsyncEventArgs> receive_Completed)
        {

            this.protocolHandler = protocolHandler;
            this.accept_Completed = accept_Completed;
            this.receive_Completed = receive_Completed;


        }


        public Session(ProtocolHandler protocolHandler)
        {

            this.protocolHandler = protocolHandler;
            this.accept_Completed = AcceptEvent_Completed;
            this.receive_Completed = ReceiveEvent_Completed;
        }

        public virtual void Init()
        {
            acceptEvent.Completed += new EventHandler<SocketAsyncEventArgs>(accept_Completed);
            receiveEvent.Completed += new EventHandler<SocketAsyncEventArgs>(receive_Completed);
        }


        public virtual void Process_Packet(Session session, Packet rowPacket)
        {
            protocolHandler.Call_PacketFunc(session, rowPacket);
        }


        private void AcceptEvent_Completed(object sender, SocketAsyncEventArgs e)
        {

            SessionManager.instance.Pop_ClosedSessions(this);
            SessionManager.instance.Push_OpenSessions(this);

            this.socket = e.AcceptSocket;

            Console.WriteLine("Accept Complete!!");

            /*
             * getbuffer from buffer pool
             */

            receiveEvent.SetBuffer(buffer, totalOffset, buffer.Length - totalOffset);

            socket.ReceiveAsync(receiveEvent);
        }

        private void ReceiveEvent_Completed(object sender, SocketAsyncEventArgs e)
        {
            
            int currentOffset = 0;

            if (socket.Connected == true && e.BytesTransferred > 0)
            {
                //Seesion
                ushort packetType = BitConverter.ToUInt16(e.Buffer, sizeof(int));
                Packet packet = ProtocolHandler.PacketDic[packetType];
                currentOffset = packet.Decode(e.Buffer);
                Process_Packet(this, packet);
            }

            totalOffset += (e.BytesTransferred - currentOffset);
            e.SetBuffer(buffer, totalOffset, buffer.Length - totalOffset);

            Console.WriteLine("total offset : {0}, Decoded Size : {1} ", totalOffset, currentOffset);


            socket.ReceiveAsync(e);

            /*
             * Remain Buffer pull 
             */
        }



        public void Send(Packet packet)
        {

        }

    }
}
