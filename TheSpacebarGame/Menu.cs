using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace TheSpacebarGame
{
    public partial class Menu : Form
    {
        public static string PicPath; //Avatar Path
        public static IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 666); //Make new Server
        public static Game game = new Game(); //Make a new instance of the Gake

        public Menu()
        {
            InitializeComponent();
        }

        private void pickFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                PicPath = fileDialog.FileName;            }
        }

        private void joinButton_Click(object sender, EventArgs e)
        {
            Program.Ourname = textBox2.Text;
            Boolean output = Network.Start(ip, textBox2.Text, PicPath);

            if (output)
            {
                Game.load.Show();
            }
            else
            {
                MessageBox.Show("Server is not responding");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                 ip = new IPEndPoint(IPAddress.Parse(textBox1.Text), 666);
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Network.AutoDiscover(); //This is auto discovery. It discovers the server
            System.Threading.Thread.Sleep(2000); // Sleep for 2 seconds to allow for a response as Lidgren is Async
            if (ip != new IPEndPoint(IPAddress.Parse("127.0.0.1"), 666))
            {
                textBox1.Text = ip.Address.ToString(); // TextBox = ipAddresss
            }
        }
    }
}
