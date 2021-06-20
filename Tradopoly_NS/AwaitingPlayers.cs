using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Tradopoly_NS
{
    public partial class AwaitingPlayers : Form
    {
        public int num;
        public bool close;

        public AwaitingPlayers(int num)
        {
            InitializeComponent();
            this.num = num;
            this.close = false;
            Thread backgroundThread = new Thread(new ThreadStart(While_Not_Full));
            backgroundThread.Start();
        }

        public void While_Not_Full()
        {
            bool flag = false;
            if (this.num == 2)
            {
                Send("join_2");
                while (!flag)
                {
                    string[] msg = receive().Split(',');
                    if(msg[0] == "Full")
                    {
                        _2Players p = new _2Players(int.Parse(msg[1]));
                        flag = true;
                        p.ShowDialog();
                        this.Close();
                    }
                }
            }
            else if (this.num == 3)
            {
                Send("join_3");
                while (!flag)
                {
                    string[] msg = receive().Split(',');
                    if (msg[0] == "Full")
                    {
                        _3Players p = new _3Players(int.Parse(msg[1]));
                        flag = true;
                        p.ShowDialog();
                        this.Close();
                    }
                }
            }
            else if (this.num == 4)
            {
                Send("join_4");
                while (!flag)
                {
                    string[] msg = receive().Split(',');
                    if (msg[0] == "Full")
                    {
                        _4Players p = new _4Players(int.Parse(msg[1]));
                        flag = true;
                        p.ShowDialog();
                        this.Close();
                    }
                }
            }
        }
        
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (this.close == true)
            {
                this.Close();
                timer1.Enabled = false;
            }
        }

        //sends message to server
        private void Send(string msg)
        {
            byte[] msgBuffer = Encoding.Default.GetBytes(msg);
            Program.sck.Send(msgBuffer, 0, msgBuffer.Length, 0);
        }

        //recieves message from server
        private string receive()
        {
            byte[] buffer = new byte[255];
            string received = Encoding.ASCII.GetString(buffer, 0, Program.sck.Receive(buffer));
            Console.WriteLine(received);
            return received;
        }

    }
}
