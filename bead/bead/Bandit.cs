using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bead
{
    class Bandit : VarosElem
    {
        public int hp = 100;
        public int dmg;
        public int gold = 0;
        public int x, y;

        public override void toString()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($"{'B',3} ");
            Console.ResetColor();
        }
        public void move(VarosElem[,] varos)
        {
            List<int[]> lehetsegesMove = new List<int[]>();
            if (this.hp<=0)
            {
                Ground ground = new Ground();
                ground.felfed = true;
                varos[this.x, this.y] = ground;
                return;
            }
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    try
                    {
                        int ujX = this.x + i;
                        int ujY = this.y + j;
                        int[] koord = { ujX, ujY };
                        if (varos[ujX, ujY] is Sheriff)
                        {
                            tamad(Varos.sher);
                            return;
                        }
                        if (varos[ujX,ujY] is Ground)
                        {
                            lehetsegesMove.Add(koord);
                        }
                        if (varos[ujX,ujY] is Gold)
                        {
                            lepes(varos, ujX, ujY);
                            gold++;
                            return;
                        }
                        
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            Random r = new Random();
            if (lehetsegesMove.Count() > 0)
            {
                int[] lep = lehetsegesMove[r.Next(0, lehetsegesMove.Count())];
                lepes(varos, lep[0], lep[1]);
            }
        }

        private void lepes(VarosElem[,] varos, int ujX, int ujY)
        {
            Ground ground = new Ground();
            
            if (varos[this.x, this.y].felfed == false)
            {
                ground.felfed = false;
                varos[this.x, this.y] = ground;
            }
            else
            {
                ground.felfed= true;
            }
            if(varos[ujX, ujY].felfed == false)
            {
                this.felfed = false;
            }
            else
            {
                this.felfed = true;
            }
            varos[this.x, this.y] = ground;
            varos[ujX, ujY] = this;
            this.x = ujX;
            this.y = ujY;
        }

        private void tamad(Sheriff sher)
        {
            Random random = new Random();
            this.dmg = random.Next(4,16);
            sher.hp -= this.dmg;
        }
    }
}
