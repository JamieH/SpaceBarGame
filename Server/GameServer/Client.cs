using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace GameServer
{

    class Client
    {
        public string Name;
        public NetConnection Connection;
        public int Score;
        public byte[] Buffer;

        public Client(string name, NetConnection connection, int score, byte[] buffer)
        {
            Name = name; //Name
            Connection = connection; //ServerConnection for that Client
            Score = score; //Score
            Buffer = buffer; //The avatar in a Byte Array
        }

    }
}
