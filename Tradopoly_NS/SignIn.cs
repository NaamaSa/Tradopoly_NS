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
    public partial class SignIn : Form
    {
        public SignIn()
        {
            InitializeComponent();
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 8000);
            Program.sck = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Program.sck.Connect(remoteEP);
        }

        //sign in button
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = $"signin,{textBox1.Text},{textBox2.Text}";
                Send(msg);
                string back = receive();
                if (back == "success")
                {
                    label4.Text = "SUCCESS";
                    label4.Location = new Point(338, 256);
                    label4.ForeColor = Color.Green;

                    label1.Visible = false;
                    label2.Visible = false;
                    label3.Visible = false;
                    label4.Visible = false;
                    textBox1.Visible = false;
                    textBox2.Visible = false;

                    FirstPage c = new FirstPage();
                    this.Hide();
                    c.ShowDialog();
                    this.Close();
                }
                else if (back == "fail")
                {
                    label4.Text = "FAIL - email or password is inccorrect";
                    label4.Location = new Point(237, 256);
                    label4.ForeColor = Color.Red;
                }

            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }
        
        //sign up button
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            SignUp c = new SignUp();
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

