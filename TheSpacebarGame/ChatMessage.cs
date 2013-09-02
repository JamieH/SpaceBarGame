using System.Drawing;
using System.IO;

namespace TheSpacebarGame
{
    public class ChatMessage // New Class for a Chat Message
    {
        public Player Sender;
        public string Message;


        public ChatMessage(Player sender, string message)
        {
            Sender = sender; //Name
            Message = message;
        }
    }
}