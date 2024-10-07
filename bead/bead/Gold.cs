using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bead
{
    class Gold : VarosElem
    {
        public void SheriffPickup(Sheriff player)
        {
            player.gold++;
        }
        public void BanditPickup(Bandit bandit)
        {
            bandit.gold++;
        }
        public override void toString()
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($"{'G',3} ");
            Console.ResetColor();
        }
    }
}
