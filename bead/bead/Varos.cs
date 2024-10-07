using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bead
{
    class Varos : VarosElem
    {
        public VarosElem[,] varos = new VarosElem[25, 25];
        public static int whiskeyCount = 0;
        public static Bandit[] banditak = new Bandit[4];
        public static Sheriff sher;

        public void Gen()
        {
            do
            {
                VarosElem[,] varos = new VarosElem[25, 25];
                barrGen();
            }
            while (!Bejarhato());
            GroundGen();
            SheriffGen();
            BanditGen();
            GoldGen(5);
            WhiskeyGen(3);
            TownhallGen();
            felfedez();
            Jelenit();

        }
        public void WhiskeyEll()
        {
            
            if (whiskeyCount < 3) 
            {
                int kellWhiskey = 3 - whiskeyCount;
                WhiskeyGen(kellWhiskey);
            }
        }
        private void BanditGen()
        {
            Random random = new Random();
            int x, y;
            for (int i = 0; i < 4; i++)
            {
                do
                {
                    x = random.Next(0, 25); y = random.Next(0, 25);
                } while (!(varos[x, y] is Ground));
                Bandit bandit = new Bandit();
                bandit.x = x;
                bandit.y = y;
                varos[x, y] = bandit;
                varos[x, y].felfed = false;
                banditak[i] = bandit;
            }
            
        }

        private void TownhallGen()
        {
            Random random = new Random();
            int x;
            int y;
            do
            {
                x = random.Next(0, 25); y = random.Next(0, 25);
            } while (!(varos[x, y] is Ground));
            Townhall th = new Townhall();
            th.felfed = false;
            varos[x, y] = th;
        }

        private void WhiskeyGen(int v)
        {
            Random random = new Random();
            int x;
            int y;
            for (int i = 0; i < v; i++)
            {
                do
                {
                    x = random.Next(0, 25); y = random.Next(0, 25);
                } while (!(varos[x, y] is Ground));
                Whiskey whiskey = new Whiskey();
                whiskey.felfed = false;
                varos[x, y] = whiskey;
                whiskeyCount++;
            }
        }

        private void GoldGen(int v)
        {
            Random random = new Random();
            int x;
            int y;
            for (int i = 0; i < v; i++)
            {
                do
                {
                    x = random.Next(0, 25); y = random.Next(0, 25);
                } while (!(varos[x, y] is Ground));
                Gold gold = new Gold();
                gold.felfed = false;
                varos[x, y] = gold;
            }
        }

        private void SheriffGen()
        {
            Random random = new Random();
            int x, y;
            do
            {
                x = random.Next(0,25); y = random.Next(0,25);
            } while (varos[x, y] is Barricade);
            sher = new Sheriff();
            sher.x = x;
            sher.y = y;
            varos[x, y] = sher;
            varos[x, y].felfed = true;
        }

        public void GroundGen()
        {
            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    if (!(varos[i, j] is Barricade bar))
                    {
                        Ground gr = new Ground();
                        gr.felfed = false;
                        varos[i, j] = gr;
                    }
                }
            }
        }

        public void barrGen()
        {
            Random rnd = new Random();
            
            for (int i = 0; i < 100; i++)
            {
                int x;
                int y;
                do
                {
                    x = rnd.Next(0, 25);
                    y = rnd.Next(0, 25);
                }
                while (varos[x, y] is Barricade);
                Barricade bar = new Barricade();
                bar.felfed = false;
                varos[x, y] = bar;
            }
        }
        public bool Bejarhato()
        {
            bool[,] latogatott = new bool[25, 25];
            Queue<(int, int)> sor = new Queue<(int, int)>();

            if (varos[0, 0] is Barricade)
                return false;

            sor.Enqueue((0, 0));
            latogatott[0, 0] = true;

            int[] iranyX = { 0, 1, 0, -1, -1, 1, -1, 1 };
            int[] iranyY = { 1, 0, -1, 0, -1, -1, 1, 1 };

            while (sor.Count > 0)
            {
                var (x, y) = sor.Dequeue();

                for (int i = 0; i < 8; i++)
                {
                    int ujX = x + iranyX[i];
                    int ujY = y + iranyY[i];

                    if (ujX >= 0 && ujX < 25 && ujY >= 0 && ujY < 25 &&
                        !latogatott[ujX, ujY] && !(varos[ujX, ujY] is Barricade))
                    {
                        sor.Enqueue((ujX, ujY));
                        latogatott[ujX, ujY] = true;
                    }
                }
            }

            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    if (!(varos[i, j] is Barricade) && !latogatott[i, j])
                        return false;
                }
            }

            return true;
        }
        public void Jelenit()
        {

            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    if (varos[i, j].felfed == true)
                    {
                        varos[i, j].toString();
                    }
                    else if (varos[i, j].felfed == false)
                    {
                        //varos[i, j].toString();

                        Console.BackgroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($"{'?',3} ");
                        Console.ResetColor();
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("Sheriff Élet: " + sher.hp+" // Arany: "+sher.gold+"/5 // " + sher.allapot);
            if (whiskeyCount < 3)
            {
                int neededWhiskey = 3 - whiskeyCount;
                WhiskeyGen(neededWhiskey);
            }
            felfedez();
        }
        public void felfedez()
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int ujX = sher.x + i;
                    int ujY = sher.y + j;
                    try
                    {
                        varos[ujX, ujY].felfed = true;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }
        

    }
}
