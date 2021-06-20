using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Tradopoly_NS
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
        }

        //if signin button is pressed
        private void button1_Click(object sender, EventArgs e)
        {
            SignIn c = new SignIn();
            this.Hide();
            c.ShowDialog();
            this.Close();
        }
        
        //if signup button is pressed
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text == textBox3.Text)
                {
                    string msg = $"signup,{textBox4.Text},{textBox1.Text},{textBox3.Text}";
                    Send(msg);
                    string back = receive();
                    if (back == "success")
                    {
                        label7.Text = "SUCCESS";
                        label7.Location = new Point(338, 352);
                        label7.ForeColor = Color.Green;
                        label1.Visible = false;
                        label2.Visible = false;
                        label3.Visible = false;
                        label4.Visible = false;
                        textBox1.Visible = false;
                        textBox2.Visible = false;
                        textBox3.Visible = false;
                        textBox4.Visible = false;
                        SignIn c = new SignIn();
                        this.Hide();
                        c.ShowDialog();
                        this.Close();
                    }
                    else if (back == "fail-email")
                    {
                        label7.Text = "FAIL - The email is already in use";
                        label7.Location = new Point(237, 352);
                        label7.ForeColor = Color.Red;
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
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
            return received;
        }

    }
}

