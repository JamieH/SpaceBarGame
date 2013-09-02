using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace TheSpacebarGame
{
    static class Program
    {
        public static int Place = 0;
        public static string Ourname; //Our name
        public static string Realwinner; //The first winner
        public static bool SetWinner;
        public static bool Started = false; //RefreshDisplay check

        public static Dictionary<long, Player> PlayerStore; //This is where we store the player
        public static Dictionary<long, ChatMessage> ChatMessages; //This is where we store the chat messages
        public static List<string> LeaderBoardList = new List<string>();

            /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
             Application.Run(new Menu());

        }

    }
}
