using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Lidgren.Network;
using Timer = System.Windows.Forms.Timer;

namespace TheSpacebarGame
{
    public partial class Game : Form
    {
        //ANTICHEAT

        public static LoadScreen load = new LoadScreen(); //Instance of LoadScreen
        public static Dictionary<long, Player> PlayerStore; //This is where we store the player
        public static Dictionary<long, ChatMessage> ChatMessages; //This is where we store the chat messages

        private static readonly Timer startTimer = new Timer(); //Countdown Timer
        public static Timer gameTimer = new Timer();
        private static int timerCount; //How many times has the timer ticked?
        public static int globalGameTime;
        private readonly Image picture = new Bitmap(900, 600); //The picture
        private int cheatammount = 0;

        private Timer debugTimer = new Timer(); //Timer used for Automating KeyDown for Debug
        private DateTime now;
        public int score = 0; //The score
        private DateTime then;
        private TimeSpan timespan;

        public Game()
        {
            InitializeComponent();
            Network.MessageReceived += MessageReceive; //Our Event Handler for Networking
            PlayerStore = new Dictionary<long, Player>(); //Stop PlayerStore being null
            ChatMessages = new Dictionary<long, ChatMessage>();
        }

        private void MessageReceive(NetIncomingMessage message) //Message Handler
        {
            byte packetHeader;
            try
            {
                packetHeader = message.ReadByte(); //Try read Header
            }
            catch
            {
                return;
            }

            switch ((Network.PacketTypes) packetHeader) //Switch PacketHeaderType
            {
                case Network.PacketTypes.RECEIVEPLAYER: //Receive a new player
                    string name = message.ReadString();
                    long uid = message.ReadInt64();

                    int bufferLength = message.ReadInt32();
                    byte[] buffer = message.ReadBytes(bufferLength);

                    PlayerStore.Add(uid, new Player(name, buffer, 0));

                    Console.WriteLine("Player {0} connected with ID: {1}", name, uid);
                    load.Invoke(new Action(() => load.RefreshDisplay()));
                    break;

                case Network.PacketTypes.GETPOS: //Receive someones score
                    long id = message.ReadInt64();
                    int score = message.ReadInt16();
                    Console.WriteLine(id);


                    PlayerStore[id].Score = score;
                    load.Invoke(new Action(() => RefreshDisplay()));
                    Console.WriteLine("Player: {0} Score: {1}", PlayerStore[id].Name, PlayerStore[id].Score);


                    if (PlayerStore[id].Score >= 100 & Program.SetWinner == false)
                    {
                        Program.Realwinner = PlayerStore[id].Name;
                        Program.SetWinner = true;
                        Program.LeaderBoardList.Add(String.Format("{0} : {1}", Program.Realwinner,
                        globalGameTime));
                        Invoke(new Action(() => LeaderBoard.DataSource = Program.LeaderBoardList));
                    }
                    break;

                case Network.PacketTypes.HOST: //If we receive this then we are the host
                    load.Invoke(new Action(() => load.buttonStart.Visible = true));
                    string fix = message.ReadString();
                    break;

                case Network.PacketTypes.STARTGAME: //Start the Game form.
                    load.Invoke(new Action(() => TheSpacebarGame.Menu.game.Show()));
                    string fix2 = message.ReadString();
                    break;

                case Network.PacketTypes.SCOREWIN: //Someone has Won
                    string person = message.ReadString();
                    Program.Place = message.ReadInt16();

                    if (Program.Place == 1 & person == Program.Ourname)
                    {
                        Program.LeaderBoardList.Add(String.Format("{0} : {1}", person,globalGameTime));
                        Invoke(new Action(() => LeaderBoard.DataSource = Program.LeaderBoardList));
                    }
                    else
                    {
                        Program.LeaderBoardList.Add(String.Format("{0} : {1}", person, globalGameTime));

                        Invoke(new Action(() => LeaderBoard.DataSource = Program.LeaderBoardList));
                    }
                    break;

                case Network.PacketTypes.DISCONNECT: //Someone has Disconnected
                    long thereID = message.ReadInt64();
                    if (PlayerStore.ContainsKey(thereID))
                    {
                        PlayerStore.Remove(thereID);
                    }
                    else
                    {
                        Console.WriteLine("Tried to remove non existant ID");
                    }

                    if (Program.Started)
                    {
                        load.Invoke(new Action(() => RefreshDisplay()));
                    }
                    break;
                case Network.PacketTypes.CHATREC: //Start the Game form.
                    long idrec = message.ReadInt64();
                    string recMessage = message.ReadString();
                    Console.WriteLine(idrec);
                    Console.WriteLine(recMessage);
                    Game.ChatMessages.Add(LoadScreen.RandomNumber(1, 99999999), new ChatMessage(PlayerStore[idrec], recMessage));
                    load.Invoke(new Action(() => load.RefreshChat()));

                    break;
            }
        }


        private void keyUp(object sender, KeyEventArgs e)
        {
            //then = DateTime.Now;
            //TimeSpan old = timespan;

            //timespan = then - now;

            //if (timespan.Ticks - old.Ticks >= -70)
            //{
            //cheatammount++;
            //Console.WriteLine(timespan.Ticks);
            //Console.WriteLine(old.Ticks);
            //Console.WriteLine("CHEAT: Consis: You cheated for the {0} time", cheatammount);

            ////}

            //if (timespan.Ticks < 2000) //Too fast for a Human
            //{
            //    cheatammount++;
            //    Console.WriteLine("CHEAT: Quick: You cheated for the {0} time", cheatammount);
            //}

            //if (cheatammount == 10) //If they've cheated 10 times, Spam MessageBoxes
            //{
            //    MessageBox.Show("Why you cheat?");
            //    this.Close();
            //}

            if (e.KeyCode == Keys.Space) //If the key is SpaceBar
            {

                if (score >= 100)
                {
                }
                else
                {
                    score++; //Increment
                    Console.WriteLine(score);

                    Network.SendText(Network.PacketTypes.SENDPOS, score.ToString()); //Send Score
                    scoreLabel.Text = score.ToString(); //Label = our Score
                }
            }
            //}
        }

        private void Game_Load(object sender, EventArgs e)
        {
            startTimer.Interval = 1000; //Game Form loaded
            startTimer.Tick += startTimer_Tick; //Event Handler for Tick
            startTimer.Start(); //Start the Timer
        }

        public void RefreshDisplay()
        {
            using (Graphics g = Graphics.FromImage(picture)) //g is picture
            {
                g.Clear(SystemColors.Control);

                int counter = 1; //Counter for Players
                foreach (var entry in PlayerStore) //Foreach Player
                {
                    g.DrawImage(entry.Value.Avatar, new Rectangle(entry.Value.Score*8, 10, 32, 32));
                    g.DrawString(entry.Value.Name, new Font(FontFamily.GenericSansSerif, 12), Brushes.Black,
                        new Point(entry.Value.Score*8, 50));
                    counter++; // Next Player, Increment.
                }
            }
            pictureBox1.Image = picture; //PictureBox = generated picture
        }

        private void startTimer_Tick(object sender, EventArgs e)
        {
            switch (timerCount)
            {
                case 0:
                    countdownLabel.Text = "3"; // 0 Ticks
                    break;

                case 1:
                    countdownLabel.Text = "2"; //1 Tick
                    break;
                case 2:
                    countdownLabel.Text = "1"; //2 Ticks
                    break;
                case 3:
                    countdownLabel.Text = "GO"; //3 TIcks
                    Thread.Sleep(500); //Give half a second to say Go
                    startTimer.Stop();
                    countdownLabel.Visible = false;
                    KeyUp += keyUp;
                    KeyDown += Game_KeyDown;
                    Console.WriteLine("Game should be initalised");
                    gameTimer.Tick += gameTimer_Tick;
                    gameTimer.Interval = 1000;
                    gameTimer.Start();
                //debugTimer.Interval = 50; //Debug code to automate keypress
                    //debugTimer.Tick += debugTimer_Tick;
                    //debugTimer.Start();

                    break;
            }
            timerCount++; //We have ticked
        }

        void gameTimer_Tick(object sender, EventArgs e)
        {
            globalGameTime = globalGameTime + 1;
        }

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            now = DateTime.Now;
        }

        private void YouWinLabel_Click(object sender, EventArgs e)
        {
        }
    }
}