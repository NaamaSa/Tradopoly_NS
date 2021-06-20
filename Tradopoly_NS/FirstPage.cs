using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Tradopoly_NS
{
    public partial class FirstPage : Form
    {
        public FirstPage()
        {
            InitializeComponent();
        }

        //2 players button is pressed
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            AwaitingPlayers c = new AwaitingPlayers(2);
            c.ShowDialog();
            this.Close();
        }

        //3 players button is pressed
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            AwaitingPlayers c = new AwaitingPlayers(3);
            c.ShowDialog();
            this.Close();
        }

        //4 players button is pressed
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            AwaitingPlayers c = new AwaitingPlayers(4);
            c.ShowDialog();
            this.Close();
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
            return received;
        }
    }
}

