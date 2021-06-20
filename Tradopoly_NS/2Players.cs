using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Timers;
namespace Tradopoly_NS
{
    public partial class _2Players : Form
    {
        private SqlConnection conn;
        private SqlCommand cmd;
        private SqlDataReader dataReader;
        private const int MAXSIDE = 533;
        private const int MINSIDE = 37;
        private const int CORNERADD = 15;
        private const int DIFF = 47;
        private int x = MAXSIDE;
        private int y = MAXSIDE;
        private int xPlace = 10;
        private int yPlace = 10;
        private int diceVal;
        private const int SIZE = 11;
        private string[,] BOARD;
        private static Player player;
        private int index;
        private int jail;


        public _2Players(int index)
        {
            InitializeComponent();
            this.BOARD = new string[SIZE, SIZE];
            OpenSqlConnection();
            this.index = index;
            Thread backgroundThread = new Thread(new ThreadStart(receive));
            backgroundThread.Start();
        }

        private void OpenSqlConnection()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Park-Hamada Student\Desktop\Tradopoly_NS\Tradopoly_NS\Database1.mdf;Integrated Security=True";
            this.conn = new SqlConnection(connectionString);
        }

        private void _2Players_Load(object sender, EventArgs e)
        {
            Console.WriteLine("enter");
            setBoard(this.BOARD);
            int[] arrMoney = new int[] { 2, 4, 1, 1, 2, 1, 5 };
            player = new Player(IndexColor(), this.index, arrMoney, new string[22], new string[4], new string[2], new string[10], 1500, 10, 10);
            Console.WriteLine(player.ToString());
        }

        //find corresponding color
        private string IndexColor()
        {
            if (this.index == 0)
                return "red";
            else if (this.index == 1)
                return "blue";
            else if (this.index == 2)
                return "yellow";
            else
                return "green";
        }

        //set board
        private void setBoard(string[,] BOARD)
        {
            BOARD[0, 0] = "f,d";
            BOARD[1, 0] = "red1,StreetCard,DeedCard,Cards";
            BOARD[2, 0] = "special1,SpecialCard,Cards";
            BOARD[3, 0] = "red2,StreetCard,DeedCard,Cards";
            BOARD[4, 0] = "red3,StreetCard,DeedCard,Cards";
            BOARD[5, 0] = "train3,TrainCard,DeedCard,Cards";
            BOARD[6, 0] = "yellow1,StreetCard,DeedCard,Cards";
            BOARD[7, 0] = "yellow2,StreetCard,DeedCard,Cards";
            BOARD[8, 0] = "water,DeedCard,Cards";
            BOARD[9, 0] = "yellow3,StreetCard,DeedCard,Card";
            BOARD[10, 0] = "f,d";
            BOARD[10, 1] = "green1,StreetCard,DeedCard,Card";
            BOARD[10, 2] = "green2,StreetCard,DeedCard,Card";
            BOARD[10, 3] = "special2,SpecialCard,Cards";
            BOARD[10, 4] = "green3,StreetCard,DeedCard,Card";
            BOARD[10, 5] = "train4,TrainCard";
            BOARD[10, 6] = "special3,SpecialCard,Cards";
            BOARD[10, 7] = "blue1,StreetCard,DeedCard,Card";
            BOARD[10, 8] = "pay,k";
            BOARD[10, 9] = "blue2,StreetCard,DeedCard,Card";
            BOARD[10, 10] = "f,k";
            BOARD[9, 10] = "brown1,StreetCard,DeedCard,Card";
            BOARD[8, 10] = "pay,d";
            BOARD[7, 10] = "brown2,StreetCard,DeedCard,Card";
            BOARD[6, 10] = "pay,Cards";
            BOARD[5, 10] = "train1,TrainCard,DeedCard,Card";
            BOARD[4, 10] = "lblue1,StreetCard,DeedCard,Card";
            BOARD[3, 10] = "special4,SpecialCard,Cards";
            BOARD[2, 10] = "lblue2,StreetCard,DeedCard,Card";
            BOARD[1, 10] = "lblue3,StreetCard,DeedCard,Card";
            BOARD[0, 10] = "f,t";
            BOARD[0, 9] = "pink1,StreetCard,DeedCard,Card";
            BOARD[0, 8] = "power,DeedCard,DeedCard,Card";
            BOARD[0, 7] = "pink2,StreetCard,DeedCard,Card";
            BOARD[0, 6] = "pink3,StreetCard,DeedCard,Card";
            BOARD[0, 5] = "train2,TrainCard,DeedCard,Card";
            BOARD[0, 4] = "orange1,StreetCard,DeedCard,Card";
            BOARD[0, 3] = "special5,SpecialCard,Cards";
            BOARD[0, 2] = "orange2,StreetCard,DeedCard,Card";
            BOARD[0, 1] = "orange3,StreetCard,DeedCard,Card";
        }

        //roll picture is pressed
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //roll the dice
            int die1 = Logics.RollDice(pictureBox1);
            Thread.Sleep(50);
            int die2 = Logics.RollDice(pictureBox12);
            this.diceVal = die1 + die2;
            
            //move player
            if (this.x >= MINSIDE && this.x <= MAXSIDE && this.y == MAXSIDE) //buttom
            {
                int z = DIFF * this.diceVal; // how many pixels the player needs to move
                if (this.x - z > MINSIDE) //if the player destination is on the button
                {
                    this.x = this.x - z;
                    this.xPlace = this.xPlace - diceVal;
                    Console.WriteLine("2");
                }
                else if (this.x - z == MINSIDE - CORNERADD || this.x - z == MINSIDE + CORNERADD) //if the player destionation is on the bottom left corner
                {
                    this.x = 15;
                    this.y = 555;
                    this.xPlace = 0;
                    this.yPlace = 0;
                    Console.WriteLine("3");
                }
                else if (this.x - z < MINSIDE || this.x - z == MINSIDE + CORNERADD) //if the player's destination is on the left
                {
                    z = z - this.x;
                    this.y = MAXSIDE - z - 4 * CORNERADD;
                    this.x = MINSIDE;
                    this.yPlace = this.xPlace - diceVal + 10;
                    this.xPlace = 0;
                    Console.WriteLine("4");
                }
            }
            else if (this.y >= MINSIDE && this.y <= MAXSIDE && this.x == MINSIDE) //left
            {
                int z = DIFF * this.diceVal; //how many pixels the player needs to move
                if (this.y - z > MINSIDE) //if the player destination is on the left
                {
                    this.y = this.y - z;
                    this.yPlace = this.yPlace - diceVal;
                }
                else if (this.y - z == MINSIDE - CORNERADD || this.y - z == MINSIDE + CORNERADD) //if the player destionation is on the top left corner
                {
                    this.y = MINSIDE;
                    this.yPlace = 0;
                    this.xPlace = 0;
                }
                else if (this.y - z < MINSIDE) //if the player's destination is on the top
                {
                    z = z - this.y;
                    this.x = MINSIDE + z + 4 * CORNERADD;
                    this.y = MINSIDE;
                    this.xPlace = diceVal - this.yPlace;
                    this.yPlace = 0;
                }
            }
            else if (this.x >= MINSIDE && this.x <= MAXSIDE && this.y == MINSIDE) //top
            {
                int z = DIFF * this.diceVal; //how many pixels the player needs to move
                if (this.x + z < MAXSIDE) //if the player destination is on the top
                {
                    this.x = this.x + z;
                    this.xPlace = this.xPlace + diceVal;
                }
                else if (this.x + z == MAXSIDE + CORNERADD || this.x + z == MAXSIDE - CORNERADD) //if the player destionation is on the top right corner
                {
                    this.x = MAXSIDE;
                    this.xPlace = 10;
                    this.yPlace = 0;
                }
                else if (this.x + z > MAXSIDE) //if the player's destination is on the right
                {
                    z = z - (MAXSIDE - this.x);
                    this.y = MINSIDE + z + 2 * CORNERADD;
                    this.x = MAXSIDE;
                    this.yPlace = diceVal + this.xPlace - 10;
                    this.xPlace = 10;
                }
            }
            else if (this.y >= MINSIDE && this.y <= MAXSIDE && this.x == MAXSIDE) //right
            {
                int z = DIFF * this.diceVal; //how many pixels the player needs to move
                if (this.y + z < MAXSIDE) //if the player destination is on the right
                {
                    this.y = this.y + z;
                    this.yPlace = this.yPlace + diceVal;
                }
                else if (this.y + z == MAXSIDE + CORNERADD || this.y + z == MAXSIDE - CORNERADD) //if the player destionation is on the bottom right corner
                {
                    this.y = MAXSIDE;
                    this.yPlace = 10;
                    this.xPlace = 10;
                    player.SetAddTotalMoney(400);
                    label8.Text = player.GetTotalMoney().ToString();
                }
                else if (this.y + z > MAXSIDE) //if the player's destination is on the buttom
                {
                    z = z - (MAXSIDE - this.y);
                    this.x = MAXSIDE - z - 2 * CORNERADD;
                    this.y = MAXSIDE;
                    this.xPlace = 20 - this.yPlace - diceVal;
                    this.yPlace = 10;
                    player.SetAddTotalMoney(200);
                    label8.Text = player.GetTotalMoney().ToString();
                }
            }
            
            player.SetX(this.xPlace);
            player.SetY(this.yPlace);

            //check if place on board is buyable
            this.conn.Open();
            string[] c = this.BOARD[this.xPlace, this.yPlace].Split(',');

            if (c[0].Contains("train"))
            {
                string sql = $"SELECT owned FROM [Table] WHERE name = train1, name = train2, name = train3, name = train4, ";
                this.cmd = new SqlCommand(sql, conn);
                this.dataReader = cmd.ExecuteReader();
                string output = "";
                while (this.dataReader.Read())
                {
                    output = this.dataReader.GetValue(0) + "," + this.dataReader.GetValue(1);
                }
                if (output[int.Parse(c[0][c[0].Length - 1].ToString()) - 1] != player.GetNum() && output[int.Parse(c[0][c[0].Length - 1].ToString()) - 1] != 0)
                {
                    int count = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        if (output[i] == output[int.Parse(c[0][c[0].Length - 1].ToString()) - 1])
                            count++;
                    }

                }
            }
            else if (c[0] != "f" && c[0] != "pay" && c[0] != "get" && !c[0].Contains("special"))
            {
                string sql = $"SELECT owned, price FROM [Table] WHERE name = '{c[0]}'";
                this.cmd = new SqlCommand(sql, conn);
                this.dataReader = cmd.ExecuteReader();
                if (this.dataReader.Read())
                {
                    int owned = int.Parse(this.dataReader.GetValue(0).ToString());
                    string price = this.dataReader.GetValue(1).ToString();
                    Console.WriteLine($"this.dataReader: {owned}, {price}");
                    if (owned == 0)
                        label2.Text = price.ToString();
                    else if(owned != player.GetNum())
                    {
                        sql = $"SELECT rent FROM [Table] WHERE name = '{c[0]}'";
                        this.cmd = new SqlCommand(sql, conn);
                        this.dataReader = cmd.ExecuteReader();
                        string rent = "";
                        while (this.dataReader.Read())
                        {
                            rent = this.dataReader.GetValue(0).ToString();
                        }

                        player.SetSubTotalMoney(int.Parse(rent));
                        label8.Text = player.GetTotalMoney().ToString();
                        string m = $"money,{player.GetNum()},{owned},{rent}";
                        Console.WriteLine($"total: {player.GetTotalMoney()}");
                        //decreaseMoney(int.Parse(rent));
                        send(m);
                    }
                    this.dataReader.Close();
                    Console.WriteLine("9");
                }
            }
            
            //sends message to other players about movement
            string msg = $"movement,{player.GetNum()},{player.GetX().ToString()},{player.GetY().ToString()},2";
            this.conn.Close();
            send(msg);
        }

        //pay picture is pressed
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine($"board: {this.BOARD[this.xPlace, this.yPlace]}");
                string[] c = this.BOARD[this.xPlace, this.yPlace].Split(',');
                
                if (c[0] != "f" && c[0] != "pay" && c[0] != "get" && !c[0].Contains("special"))
                {
                    this.conn.Open();
                    string sql = $"SELECT owned, price FROM [Table] WHERE name = '{c[0]}'";
                    this.cmd = new SqlCommand(sql, conn);
                    this.dataReader = cmd.ExecuteReader();
                    string output = "";
                    while (this.dataReader.Read())
                    {
                        output = this.dataReader.GetValue(0) + "," + this.dataReader.GetValue(1);
                    }
                    string[] arr = output.Split(',');
                    int owned = int.Parse(arr[0]);
                    int price = int.Parse(arr[1]);
                    if ((c[1] == "DeedCard" || c[1] == "StreetCard" || c[1] == "TrainCard") && owned == 0)
                    {
                        if (player.GetTotalMoney() - price <= 0)
                        {
                            pictureBox1.Visible = false;
                            pictureBox2.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label16.Visible = true;
                            label16.Text = $"{player.GetColor()} lost!";
                            send($"lose,{player.GetNum()}");
                        }
                        else
                        {
                            sql = $"UPDATE Table SET owned={player.GetNum()} WHERE email = '{c[0]}'";
                            this.cmd = new SqlCommand(sql, conn);
                            if (c[1] == "StreetCard")
                                player.SetAddStreetList(this.BOARD[this.xPlace, this.yPlace]);
                            else if (c[1] == "TrainCard")
                            {
                                player.SetAddTrainList(this.BOARD[this.xPlace, this.yPlace]);
                                string[] info = this.BOARD[this.xPlace, this.yPlace].Split(',');
                                sql = $"UPDATE Table SET owned=player.GetNum(), info[0]=player.GetNum() WHERE email = '{c[0]}'";
                            }
                            else
                                player.SetAddCompanyList(this.BOARD[this.xPlace, this.yPlace]);
                            Console.WriteLine($"price: {price}");
                            player.SetSubTotalMoney(price);
                            Console.WriteLine($"total: {player.GetTotalMoney()}");
                            decreaseMoney(price);
                            label8.Text = player.GetTotalMoney().ToString();
                            label5.Text = c[0];
                            label7.Text = player.GetColor();
                            string str = $"property,{player.GetNum()},{c[0]},sold";
                            send(str);
                        }
                    }
                    else if (owned != 0 && owned!= player.GetNum()) // if owned by another player
                        Console.WriteLine($"{c[0]} is unavailable");
                    this.conn.Close();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }

        // decrease crash
        private void decreaseMoney(int total)
        {
            while (total > 0)
            {
                if (total % 500 != total && total % 500 <= int.Parse(label15.Text))
                {
                    label15.Text = (int.Parse(label15.Text) -  500).ToString();
                    total -= 500;
                }
                else if (total % 100 != total && total % 100 <= int.Parse(label14.Text))
                {
                    label14.Text = (int.Parse(label14.Text) - 1).ToString();
                    total -= 100;
                }
                else if (total % 50 != total && total % 50 <= int.Parse(label13.Text))
                {
                    label13.Text = (int.Parse(label13.Text) - 1).ToString();
                    total -= 50;
                }
                else if (total % 20 != total && total % 20 <= int.Parse(label12.Text))
                {
                    label12.Text = (int.Parse(label12.Text) - 1).ToString();
                    total -= 20;
                }
                else if (total % 10 != total && total % 10 <= int.Parse(label11.Text))
                {
                    label11.Text = (int.Parse(label11.Text) - 1).ToString();
                    total -= 10;
                }
                else if (total % 5 != total && total % 5 <= int.Parse(label10.Text))
                {
                    label10.Text = (int.Parse(label10.Text) - 1).ToString();
                    total -= 5;
                }
                else if (total % 1 != total && total % 1 <= int.Parse(label9.Text))
                {
                    label9.Text = (int.Parse(label9.Text) - 1).ToString();
                    total -= 1 * (total % 1);
                }
            }
        }
        
        //increase cash
        public void increaseMoney(int total)
        {
            while (total > 0)
            {
                if (total % 500 != total)
                {
                    label15.Text = (int.Parse(label15.Text) - total % 500).ToString();
                    total += 500 * (total % 500);
                }
                else if (total % 100 != total)
                {
                    label14.Text = (int.Parse(label14.Text) - total % 100).ToString();
                    total += 100 * (total % 100);
                }
                else if (total % 50 != total)
                {
                    label13.Text = (int.Parse(label13.Text) - total % 50).ToString();
                    total += 50 * (total % 50);
                }
                else if (total % 20 != total)
                {
                    label12.Text = (int.Parse(label12.Text) - total % 20).ToString();
                    total += 20 * (total % 20);
                }
                else if (total % 10 != total)
                {
                    label11.Text = (int.Parse(label11.Text) - total % 10).ToString();
                    total += 10 * (total % 10);
                }
                else if (total % 5 != total)
                {
                    label10.Text = (int.Parse(label10.Text) - total % 5).ToString();
                    total += 5 * (total % 5);
                }
                else if (total % 1 != total)
                {
                    label9.Text = (int.Parse(label9.Text) - total % 1).ToString();
                    total += 1 * (total % 1);
                }
            }
        }

        //a player moves
        private void moved(int numPlayer, int x, int y)
        {
            int xAxis, yAxis;
            if (x == 0) xAxis = MINSIDE;
            else if (x == 10) xAxis = MAXSIDE;
            else
            {
                xAxis = MINSIDE + CORNERADD + x * DIFF;
            }
            if (y == 0) yAxis = MINSIDE;
            else if (y == 10) yAxis = MAXSIDE;
            else
            {
                yAxis = MINSIDE + CORNERADD + y * DIFF;
            }
            if (numPlayer == 0)
                pictureBox3.Location = new Point(xAxis, yAxis);
            else if (numPlayer == 1)
                pictureBox11.Location = new Point(xAxis, yAxis);
        }

        //a player does something to a property
        private void property(int numPlayer, string playerColor, string propertyName, string activity)
        {
            this.conn.Open();
            if (activity == "sold")
            {
                string sql = $"UPDATE Table SET owned=numPlayer WHERE name= = '{propertyName}'";
                this.cmd = new SqlCommand(sql, conn);
                label5.Text = propertyName;
                label7.Text = playerColor;
            }
        }

        //a player pays money to another player
        private void money(int playerNum1, int playerNum2, int amount)
        {
            player.SetAddTotalMoney(amount);
            increaseMoney(amount);
        }

        //player send message to server
        private void send(string msg)
        {
            Console.WriteLine(msg);
            byte[] msgBuffer = Encoding.Default.GetBytes(msg);
            Program.sck.Send(msgBuffer, 0, msgBuffer.Length, 0);
        }

        //player wins the game
        public void win()
        {
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label16.Text = "YOU WON!";
            label16.Visible = true;
        }

        //player recieves message from server
        private void receive()
        {
            while (true)
            {
                byte[] buffer = new byte[255];
                string received = Encoding.ASCII.GetString(buffer, 0, Program.sck.Receive(buffer));
                string[] msg = received.Split(',');
                Console.WriteLine(received);
                if (msg[0] == "movement")
                {
                    moved(int.Parse(msg[1]), int.Parse(msg[2]), int.Parse(msg[3]));
                }
                else if (msg[0] == "property")
                    property(int.Parse(msg[1]), msg[2], msg[3], msg[4]);
                else if (msg[0] == "money" && int.Parse(msg[2]) == player.GetNum())
                    money(int.Parse(msg[1]), int.Parse(msg[2]), int.Parse(msg[3]));
                else if (msg[0] == "win")
                    win();
            }
        }

    }
}
