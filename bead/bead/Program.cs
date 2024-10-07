using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bead
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Varos varos = new Varos();
            varos.Gen();

            while (true) 
            {
                Console.Clear();
                if (Varos.sher.hp <=0)
                {
                    Console.WriteLine("Meghalt a Sheriff!");
                    Console.ReadKey();
                    break;
                }
                if (Varos.sher.nyert)
                {
                    break;
                }
                Varos.sher.dontes(varos.varos);
                varos.felfedez();
                varos.Jelenit();
                foreach(var band in Varos.banditak)
                {
                    band.move(varos.varos);
                }


                Thread.Sleep(100);
            }
        }

    }
}
