using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bead
{
    class Ground : VarosElem
    {
        public override void toString()
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{' ',1} ");
            Console.ResetColor();
        }
    }
}
