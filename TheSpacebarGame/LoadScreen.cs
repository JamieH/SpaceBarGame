using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TheSpacebarGame
{
    public partial class LoadScreen : Form
    {
        private Image lobbyPictureImage = new Bitmap(900, 600);
        private Image chatWindowImage = new Bitmap(900, 600);

        public LoadScreen()
        {
            InitializeComponent();
        }

        public void RefreshDisplay()
        {
          using (var g = Graphics.FromImage(lobbyPictureImage)) //g is an alias, picture is gened
            {
              g.Clear(SystemColors.Control); // clear picture

                int counter = 1; // counter for how many players todo with pos
                int y = 10; //y coord

                foreach (KeyValuePair<long, Player> entry in Game.PlayerStore)
                {
                    if (counter % 5 == 0) // if mutiple of 5 increment y and reset counter
                    {
                        counter = 1;
                        y += 120;
                    }

                    g.DrawImage(entry.Value.Avatar, new Rectangle(y, 36 * counter, 32, 32)); //draw avatar
                    g.DrawString(entry.Value.Name, new Font(FontFamily.GenericSansSerif, 12), Brushes.Black, new Point(40 + y, 36 * counter)); //draw name
                    counter++;
                }
            }
          LobbyPicture.Image = lobbyPictureImage; //PictureBox1 Image = Generated Picture
        }


        private void LoadScreen_Load(object sender, EventArgs e)
        {
            Program.Started = true; //Allow for Disconnect Refresh
            var rDisplayThread = new Thread(RefreshDisplay);
            rDisplayThread.Start();
            //Menu menu = new Menu();
            //menu.Invoke(new Action(() => menu.Hide()));
        }

        private void buttonStart_Click(object sender, EventArgs e) //Host Button
        {
            Network.SendText(Network.PacketTypes.STARTGAME, "WeAreStarting");
        }

        private void chatButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(chatWindowTB.Text))
            {
                Network.SendText(Network.PacketTypes.CHATSEND, chatWindowTB.Text);
                chatWindowTB.Text = "";
            }
        }

        public void RefreshChat() //TODO: FIX THIS ITS TERRIBLE
        {
            var refreshThread = new Thread(RefreshChatThread);
            refreshThread.Start();
        }

        private void RefreshChatThread()
        {
            using (var g = Graphics.FromImage(chatWindowImage)) //g is an alias, picture is gened
            {
                g.Clear(SystemColors.Control); // clear picture

                int counter = 1; // Space Counter

                foreach (KeyValuePair<long, ChatMessage> entry in Game.ChatMessages)
                {
                    if (counter % 7 != 0)
                    {
                        Image imageToLoad = entry.Value.Sender.Avatar;
                        var image = Network.resizeImage(imageToLoad, new Size(16, 16));

                        g.DrawImage(image, new Rectangle(10, 36 * counter, 16, 16));
                        g.DrawString(String.Format("{0} : {1}", entry.Value.Sender.Name, entry.Value.Message), new Font(FontFamily.GenericSansSerif, 12), Brushes.Black, new Point(32, 36 * counter)); //draw name
                        counter++;
                    }
                    else
                    {
                        Image imageToLoad = entry.Value.Sender.Avatar;
                        var image = Network.resizeImage(imageToLoad, new Size(16, 16));

                        g.Clear(SystemColors.Control); // clear picture
                        counter = 1;
                        g.DrawImage(image, new Rectangle(10, 36 * counter, 16, 16));
                        g.DrawString(String.Format("{0} : {1}", entry.Value.Sender.Name, entry.Value.Message), new Font(FontFamily.GenericSansSerif, 12), Brushes.Black, new Point(32, 36 * counter)); //draw name
                        counter++;
                    }

                }
            }
            chatWindow.Image = chatWindowImage; //PictureBox1 Image = Generated Picture
        }
        
        //Function to get random number
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static long RandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }

        private void chatWindowTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(chatWindowTB.Text))
                {
                    Network.SendText(Network.PacketTypes.CHATSEND, chatWindowTB.Text);
                    chatWindowTB.Text = "";
                }
            }
        }

        private void chatWindow_Click(object sender, EventArgs e)
        {

        }
    }
}
