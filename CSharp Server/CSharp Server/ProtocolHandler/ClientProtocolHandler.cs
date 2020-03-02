using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace CSharp_Server
{
    partial class ClientProtocolHandler : ProtocolHandler
    {
        public ClientProtocolHandler() : base()
        {
        }

        public override void Init_ProtocolHandler()
        {
            foreach (var protocol in Enum.GetValues(typeof(ClientProtocol)))
            {
                PacketFunc[(int)protocol] = Delegate.CreateDelegate(typeof(Action<Session, Packet>), this, $"Recv_{(ClientProtocol)protocol}");


                var creater = Assembly.GetExecutingAssembly();
                PacketDic[(int)protocol] = creater.CreateInstance($"CShar_Server.PK_{protocol}") as Packet; 
            }

        }

    }
}
