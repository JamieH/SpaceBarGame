using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using Lidgren.Network;

namespace TheSpacebarGame
{
    internal static class Network
    {
        public delegate void MessageHandler(NetIncomingMessage message);

        public enum PacketTypes
        {
            LOGIN, //LOGIN
            SENDPLAYER, //SEND US
            RECEIVEPLAYER, //RECEIVE PLAYER FROM SERVER
            SENDPOS, //SEND SCORE
            GETPOS, //RECEIVE SCORE UPDATE
            AVATAR, //AVATAR SEND
            HOST, //WE ARE HOST IF WE GET THIS
            STARTGAME, //START THE NEW FORM AND TIMER
            SCOREWIN, //SCORE UPDATE FOR WINNER
            DISCONNECT, //SOMEONE HAS DISCONNECTED
            CHATREC, // WE HAVE A CHAT MESSAGE
            CHATSEND // WE ARE SENDING A CHAT MESSAGE
        }

        private static readonly Thread NetworkingThread = new Thread(NetworkReceive);
        private static NetClient Client; // The Network Client

        public static event MessageHandler MessageReceived;

        private static void NetworkReceive()
        {
            NetIncomingMessage inc;
            while (true)
            {
                // Server.ReadMessage() Returns new messages, that have not yet been read.
                // If "inc" is null -> ReadMessage returned null -> Its null, so dont do this :)
                while ((inc = Client.ReadMessage()) != null)
                {
                    Console.WriteLine(inc.MessageType);
                    switch (inc.MessageType)
                    {
                            // Data type is all messages manually sent from client
                        case NetIncomingMessageType.Data:
                            if (MessageReceived != null)
                            {
                                MessageReceived(inc);
                            }

                            break;

                        case NetIncomingMessageType.StatusChanged:
                            // NetConnectionStatus.Connecting;
                            // NetConnectionStatus.Disconnected;
                            // NetConnectionStatus.Disconnecting;
                            // NetConnectionStatus.None;

                            // NOTE: Disconnecting and Disconnected are not instant unless client is shutdown with disconnect() ect timeout.
                            Console.WriteLine(Client.ConnectionStatus);
                            Console.WriteLine(inc.ReadString());
                            break;

                        case NetIncomingMessageType.DiscoveryResponse:
                            Console.WriteLine(inc.ReadString());
                            Menu.ip = inc.SenderEndPoint;
                            break;

                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.ErrorMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.VerboseDebugMessage:
                            Console.WriteLine(inc.ReadString());
                            break;

                        default:
                            break;
                    }
                } // If New messages
                Thread.Sleep(1);
            }
        }

        public static Boolean Start(IPEndPoint ipport, String name, String path)
        {
            var config = new NetPeerConfiguration("spacebargame"); // New Lidgren config
            Client = new NetClient(config); // Give NetClient Client our NetPeerConfig config
            config.EnableUPnP = true;

            NetOutgoingMessage msg = Client.CreateMessage(); // msg is the outgoing message
            Client.Start(); // Boot the client

            msg.Write((byte) PacketTypes.LOGIN); //LOGIN Packet header
            msg.Write(name); // Write our Name

            Client.UPnP.ForwardPort(666, "Spacebar Game 2013");
            Client.Connect(ipport, msg); //Connect to the server with our Hail message

            if (!NetworkingThread.IsAlive)
            {
                NetworkingThread.Start(); // Start NetworkingThread for incoming messages.
            }

            Thread.Sleep(500); // Sleep 500
            while (Client.ConnectionStatus != NetConnectionStatus.Connected)
            {
                if (Client.ConnectionStatus == NetConnectionStatus.Disconnected)
                {
                    NetworkingThread.Abort();
                    return false;
                }
            }
            SendFile(path); //Fire SendFile
            return true;
        }

        public static void AutoDiscover()
        {
            var config = new NetPeerConfiguration("spacebargame"); // New Lidgren config
            Client = new NetClient(config); // Give NetClient Client our NetPeerConfig config

            NetOutgoingMessage msg = Client.CreateMessage(); // msg is the outgoing message
            Client.Start(); // Boot the client

            Thread.Sleep(500); // Sleep 500

            if (!NetworkingThread.IsAlive)
            {
                Thread NetworkingThread1 = new Thread(NetworkReceive);
                NetworkingThread1.Start(); // Start NetworkingThread for incoming messages.
            }

            Thread.Sleep(500); // Sleep 500
            Client.DiscoverLocalPeers(666);
        }

        public static void SendText(PacketTypes packet, string message)
        {
            NetOutgoingMessage msg = Client.CreateMessage();
            msg.Write((byte) packet);
            msg.Write(message);
            Client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);

            Console.WriteLine(Client.ReadMessage());
        }

        public static void SendFile(String path) // Send File
        {
            NetOutgoingMessage msg = Client.CreateMessage();



            byte[] buffer;

            if (File.Exists(path) || path != null)
            {
                Image imageToLoad = Image.FromFile(path);
                var image = resizeImage(imageToLoad, new Size(32, 32));
                buffer = ImageToByte(image); // Read File into Byte Array
            }
            else
            {
                string base64image =
                    @"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";
                buffer = Convert.FromBase64String(base64image);
            }

            msg.Write((byte) PacketTypes.AVATAR); //Avatar PacketHeader
            int bufferLength = buffer.Length; // Send Length
            Console.WriteLine("Buffer Length:" + bufferLength);
            msg.Write(bufferLength);
            msg.Write(buffer); //Send Byte Array

            Client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
                // Send message, also send as Reliable and Ordered
        }

        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}