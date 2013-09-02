#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Lidgren.Network;

#endregion

namespace GameServer
{
    internal class Program
    {
        public static Dictionary<long, Client> UserToConnection; //User Storage
        public static long ID; //ID for Thread TODO: Passing Info through Threads
        public static int Counter = 0; //Counter for Places
        public static bool Started = false; //Has the game started?
        public static string Name;
        public static string ConsoleLine; //ConsoleInput

        //Server object
        private static NetServer server;
        //Configuration object
        private static NetPeerConfiguration config;

        private static void Main(string[] args)
        {
            //TODO: Chat
            //TODO: UPnP
            //TODO: Fix Simulated Conditions

            config = new NetPeerConfiguration("spacebargame") {Port = 666, EnableUPnP = true, MaximumConnections = 50};

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);


            server = new NetServer(config);
            server.Start();
            server.UPnP.ForwardPort(666, "Spacebar Game 2013");


            NetIncomingMessage inc; // Incoming Message

            //Write to con..
            var consoleInput = new Thread(ConsoleIn); //Console.ReadLine is blocked, Thread it
            consoleInput.Start(); //Start the Thread

            UserToConnection = new Dictionary<long, Client>(); //UserStore

            while (true) //While True
            {
                if ((inc = server.ReadMessage()) != null) //if message is not null
                {
                    //Console.WriteLine(inc.MessageType); //Print MessageType
                    switch (inc.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval: //If ConnectionApproval request

                            if (inc.ReadByte() == (byte) PacketTypes.LOGIN) //and LOGIN PacketHeader
                            {
                                Console.WriteLine("Incoming LOGIN"); //Incoming Login

                                Name = inc.ReadString(); //Name is String Sent by Client
                                Console.WriteLine(Name); //Print Name

                                if (server.ConnectionsCount == 0) //ConnectionCount = 0
                                {
                                    Started = false; //Reset
                                }

                                if (UserToConnection.Values.Any(c => c.Name == Name) || Started || Name.Length >= 12)
                                    //If Name exists, or Game has Started or the Length is more then 12
                                {
                                    inc.SenderConnection.Deny("Duplicate Name"); //Deny
                                }
                                else
                                {
                                    inc.SenderConnection.Approve(); //Else Allow
                                    UserToConnection.Add(inc.SenderConnection.RemoteUniqueIdentifier,
                                        new Client(Name, inc.SenderConnection, 0, new byte[00000000]));
                                    //Add them to the Dictionary for PlayerStore
                                }

                                Thread.Sleep(500); //Sleep for Half a Second

                                if (server.ConnectionsCount == 1) //If Server.ConnectionCount is 1 they are host
                                {
                                    NetOutgoingMessage msg = server.CreateMessage(); //Host Message
                                    msg.Write((byte) PacketTypes.HOST); //Write HOST PacketHeader
                                    msg.Write("Congratulations, You are the host");
                                    server.SendMessage(msg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                                }
                            }
                            break;

                        case NetIncomingMessageType.Data:

                            var packetheader = inc.ReadByte();

                            switch ((PacketTypes) packetheader)
                            {
                                case PacketTypes.AVATAR:
                                    var bufferLength = inc.ReadInt32();
                                    var buffer = inc.ReadBytes(bufferLength);
                                    Console.WriteLine("Buffer Length:" + bufferLength);

                                    UserToConnection[inc.SenderConnection.RemoteUniqueIdentifier].Buffer = buffer;
                                    foreach (KeyValuePair<long, Client> entry in UserToConnection)
                                    {
                                        if (entry.Key != inc.SenderConnection.RemoteUniqueIdentifier)
                                        {
                                            NetOutgoingMessage syncClient = server.CreateMessage();

                                            syncClient.Write((byte) PacketTypes.RECEIVEPLAYER);
                                            syncClient.Write(Name);
                                            syncClient.Write(inc.SenderConnection.RemoteUniqueIdentifier);

                                            var buffer1 =
                                                UserToConnection[inc.SenderConnection.RemoteUniqueIdentifier].Buffer;

                                            syncClient.Write(buffer1.Length);
                                            syncClient.Write(buffer1);

                                            Console.WriteLine("SENT Client Name: {0}| Client ID: {1}", Name,
                                                inc.SenderConnection.RemoteUniqueIdentifier);
                                            Console.WriteLine("TO: {0}, {1}", entry.Value.Name, entry.Value.Score);
                                            server.SendMessage(syncClient, entry.Value.Connection,
                                                NetDeliveryMethod.ReliableOrdered); //OTHERPEOPLE
                                        }
                                    }

                                    foreach (KeyValuePair<long, Client> entry in UserToConnection)
                                    {
                                        //if (entry.Key != inc.SenderConnection.RemoteUniqueIdentifier)
                                        {
                                            NetOutgoingMessage syncClient = server.CreateMessage();

                                            syncClient.Write((byte) PacketTypes.RECEIVEPLAYER);
                                            syncClient.Write(entry.Value.Name);
                                            syncClient.Write(entry.Key);

                                            var buffer1 = entry.Value.Buffer;


                                            syncClient.Write(buffer1.Length);
                                            syncClient.Write(buffer1);
                                            Console.WriteLine(buffer1.Length);

                                            Console.WriteLine("SENT Client Name: {0}| Client ID: {1}", entry.Value.Name,
                                                entry.Key);
                                            Console.WriteLine("TO: {0}", inc.SenderConnection);

                                            server.SendMessage(syncClient, inc.SenderConnection,
                                                NetDeliveryMethod.ReliableOrdered); //CLIENT
                                        }
                                    }


                                    break;

                                case PacketTypes.SENDPOS:
                                    UserToConnection[inc.SenderConnection.RemoteUniqueIdentifier].Score =
                                        Convert.ToInt16(inc.ReadString());
                                    Console.WriteLine("{0}'s score is now: {1}",
                                        UserToConnection[inc.SenderConnection.RemoteUniqueIdentifier].Name,
                                        UserToConnection[inc.SenderConnection.RemoteUniqueIdentifier].Score);
                                    ID = inc.SenderConnection.RemoteUniqueIdentifier;
                                    UpdateScore();


                                    break;

                                case PacketTypes.STARTGAME:
                                    //GOGOGOGO
                                    Console.WriteLine("START THE GAME");
                                    Started = true;

                                    foreach (KeyValuePair<long, Client> entry in UserToConnection)
                                    {
                                        {
                                            NetOutgoingMessage syncClient = server.CreateMessage();
                                            syncClient.Write((byte) PacketTypes.STARTGAME);
                                            syncClient.Write("Go");
                                            syncClient.Write(
                                                UserToConnection[inc.SenderConnection.RemoteUniqueIdentifier].Score);

                                            server.SendMessage(syncClient, entry.Value.Connection,
                                                NetDeliveryMethod.ReliableOrdered);
                                        }
                                    }

                                    break;

                                case PacketTypes.CHATSEND:
                                    string messager = inc.ReadString();

                                    Console.WriteLine("{0} : {1}",
                                        UserToConnection[inc.SenderConnection.RemoteUniqueIdentifier].Name,
                                        messager);

                                    RelayChat(inc.SenderConnection.RemoteUniqueIdentifier, messager);

                                    break;
                            }

                            break;

                        case NetIncomingMessageType.StatusChanged:

                            Console.WriteLine(inc.SenderConnection + " status changed. " + inc.SenderConnection.Status);

                            if (inc.SenderConnection.Status == NetConnectionStatus.Disconnected ||
                                inc.SenderConnection.Status == NetConnectionStatus.Disconnecting)
                            {
                                UserToConnection.Remove(inc.SenderConnection.RemoteUniqueIdentifier);
                                NetOutgoingMessage outmsg = server.CreateMessage();
                                outmsg.WriteVariableInt64(inc.SenderConnection.RemoteUniqueIdentifier);
                                server.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered);
                            }
                            break;


                        case NetIncomingMessageType.DiscoveryRequest:
                            NetOutgoingMessage discovermsg = server.CreateMessage();
                            discovermsg.Write("Hey I just met you, I'm a server, so address me maybe");
                            Console.WriteLine("WOW ~NETWORKING~");
                            server.SendDiscoveryResponse(discovermsg, inc.SenderEndPoint);
                            break;
                    }
                }
                Thread.Sleep(1);
            }
        }

        private static void ConsoleIn()
        {
            Console.WriteLine("Console Enabled");
            while (true)
            {
                ConsoleLine = Console.ReadLine();
                if (ConsoleLine != null && ConsoleLine.Contains("kick*"))
                {
                    string kickName = ConsoleLine.Split(' ')[1];
                    Console.WriteLine(kickName);
                    foreach (KeyValuePair<long, Client> entry in UserToConnection)
                    {
                        if (entry.Value.Name.Contains(kickName))
                        {
                            entry.Value.Connection.Disconnect("KICKED");
                        }
                    }
                }

                else if (ConsoleLine != null && ConsoleLine.Contains("kick"))
                {
                    string kickName = ConsoleLine.Split(' ')[1];
                    Console.WriteLine(kickName);
                    foreach (KeyValuePair<long, Client> entry in UserToConnection)
                    {
                        if (entry.Value.Name == kickName)
                        {
                            entry.Value.Connection.Disconnect("KICKED");
                        }
                    }
                }
            }
        }

        private static void UpdateScore() //Thread for updating Score
        {
            var uid = ID;
            var syncClient = server.CreateMessage(); //Create Message
            syncClient.Write((byte) PacketTypes.GETPOS); //Write GETPOS Packet Header
            syncClient.Write(uid); //Write ID
            syncClient.Write(UserToConnection[uid].Score); //Write Score

            server.SendToAll(syncClient, NetDeliveryMethod.ReliableOrdered); //Send

            if (Counter > server.ConnectionsCount) //If the Counter is Bigger then Current Connections reset it
            {
                Counter = 0;
            }

            AnnouceWinner(uid);
        }

        private static void AnnouceWinner(long uid)
        {
            if (UserToConnection[uid].Score >= 100) //Is the score above or = to 100?
            {
                Counter++; //Somone has Won
                NetOutgoingMessage message = server.CreateMessage();
                message.Write((byte) PacketTypes.SCOREWIN);
                message.Write(UserToConnection[uid].Name);
                message.Write(Counter);

                server.SendToAll(message, NetDeliveryMethod.ReliableOrdered);
            }
        }

        private static void RelayChat(long uid, string message1)
        {
            NetOutgoingMessage message = server.CreateMessage();
            message.Write((byte) PacketTypes.CHATREC);
            message.Write(uid);
            message.Write(message1);

            server.SendToAll(message, NetDeliveryMethod.ReliableOrdered);
        }
    }


    internal enum PacketTypes
    {
        LOGIN, //LOGIN REQUEST
        SENDPLAYER, //RECEIVE PLAYER FROM CLIENT
        RECEIVEPLAYER, //SEND PLAYER
        SENDPOS, //RECEIVE SCORE
        GETPOS, //SEND SCORE
        AVATAR, //AVATAR RECEIVE
        HOST, //TELL THE CLIENT THEY ARE THE HOST
        STARTGAME, //MAKE PEOPLE START GAME FORM
        SCOREWIN, //TELL PEOPLE SOMEONE HAS WON
        DISCONNECT, //SOMEONE HAS DISCONNECTED
                    CHATREC, // WE HAVE A CHAT MESSAGE
            CHATSEND // WE ARE SENDING A CHAT MESSAGE
    }
}