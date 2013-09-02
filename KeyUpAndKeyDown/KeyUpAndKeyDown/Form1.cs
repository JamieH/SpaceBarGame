using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyUpAndKeyDown
{

    public partial class Form1 : Form
    {
        public int count = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
        }

        void Form1_KeyUp(object sender, KeyEventArgs e)
        {



            switch (count) {
                case 1:
                    label1.Text = timespan.ToString();
                break;

                case 2:
                    timespan = then - now;
                    label2.Text = timespan.ToString();
                break;

                case 3:
                count = 0;
                    timespan = then - now;
                    label3.Text = timespan.ToString();
                break;
            }
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            count++;
            now = DateTime.Now;
        }
    }
}
