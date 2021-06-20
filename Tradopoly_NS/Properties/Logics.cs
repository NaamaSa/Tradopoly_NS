using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text;

namespace Tradopoly_NS
{
    public class Logics
    {
        public static int RollDice(PictureBox px)
        {
            int dice = 0;
            Random rnd = new Random();
            dice = rnd.Next(1, 7);

            px.Image = Image.FromFile(@"C:\Users\Park-Hamada Student\Desktop\Tradopoly_NS\Tradopoly_NS\Resources\Dice" + dice + ".PNG");
            return dice;
        }
    }
}
