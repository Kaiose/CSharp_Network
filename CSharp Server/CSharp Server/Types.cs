using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_Server
{
    public enum SessionType : ushort
    {
        Client = 100,
        Server = 101,
        NPC = 102,
    }

    public enum ServerType : ushort
    { 
        LoginServer = 1000,
        DBServer = 1001,
        ManagerServer = 1002,
        FieldServer = 1003,
    
    }




}
