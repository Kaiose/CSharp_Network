﻿using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CSharp_Server
{

    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();

            server.Init();

            server.Start_Accept();

            Console.Read();

        }

    }
}
