using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bead
{
    class Barricade : VarosElem
    {
        public override void toString()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write($"{' ',1} ");
            Console.ResetColor();
        }
    }
}
