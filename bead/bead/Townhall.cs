using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bead
{
    class Townhall : VarosElem
    {
        public override void toString()
        {
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($"{'T',3} ");
            Console.ResetColor();
        }
    }
}
