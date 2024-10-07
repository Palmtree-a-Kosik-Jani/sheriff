using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bead
{
    class Whiskey : VarosElem
    {

        public override void toString()
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{'W',3} ");
            Console.ResetColor();
        }
        public void heal(Sheriff sheriff)
        {
            if (sheriff.hp <= 50)
            {
                sheriff.hp += 50;
            }
            else
            {
                sheriff.hp = 100;
            }
            
        }
    }
}
