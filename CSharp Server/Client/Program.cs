using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {

            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(iPAddress, 9000);

            Socket clientSocket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            clientSocket.BeginConnect(ipEndPoint, new AsyncCallback(ConnectCallback), clientSocket);

            Console.WriteLine("Hello World!");
            while (true)
            {

            }
        }


        public static void ConnectCallback(IAsyncResult ar)
        {

            Socket socket = ar.AsyncState as Socket;

            socket.EndConnect(ar);

            byte[] buffer = new byte[1024];

            while (true)
            {
                string content = Console.ReadLine();

                Encoding.ASCII.GetBytes(content).CopyTo(buffer, 0);
                socket.Send(buffer);
            }
        }
    }
}
