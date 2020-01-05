using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace CSharp_Server
{
    public class NetworkResource
    {
        public Socket socket = null;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public static readonly int ConnectionSize = 100;

        public NetworkResource(Socket socket) { this.socket = socket; }
    }

    public class Listener
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public static bool ShutDown = false;
        public void StartListening(string IP = "127.0.0.1", Int32 port = 9000)
        {

            IPAddress ipAddress = IPAddress.Parse(IP);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            Socket listenSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listenSocket.Bind(localEndPoint);
                listenSocket.Listen(NetworkResource.ConnectionSize);
                while (!ShutDown)
                {
                    allDone.Reset();

                    Console.WriteLine("Waitting for connections...");

                    listenSocket.BeginAccept(new AsyncCallback(AcceptCallback), listenSocket);

                    allDone.WaitOne();
                }

            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void AcceptCallback(IAsyncResult socketObject)
        {
            allDone.Set();

            Socket listenSocket = socketObject.AsyncState as Socket;
            Socket socket = listenSocket.EndAccept(socketObject);

            NetworkResource nr = new NetworkResource(socket);
            socket.BeginReceive(nr.buffer, 0, NetworkResource.BufferSize, 0, new AsyncCallback(ReadCallback), nr);
        }

        public static void ReadCallback(IAsyncResult networkObject)
        {
            NetworkResource nr = networkObject.AsyncState as NetworkResource;
            Socket socket = nr.socket;
            int recvBytes = socket.EndReceive(networkObject);

            if(recvBytes > 0)
            {

                byte[] buffer = new byte[1024];
                nr.buffer.CopyTo(buffer, 0);

                string content = Encoding.ASCII.GetString(buffer,0,recvBytes);

                Console.WriteLine(content);

            }

            socket.BeginReceive(nr.buffer, 0, NetworkResource.BufferSize, 0, new AsyncCallback(ReadCallback), nr);

        }

        public static void SendCallback(IAsyncResult networkOjbect)
        {

        }
    }

    public class Network
    {

        Listener listener = new Listener();

        public void Start()
        {
            listener.StartListening();
        }

    }
}
