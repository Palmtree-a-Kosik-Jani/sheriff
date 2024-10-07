using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bead
{
    class Sheriff : VarosElem
    {
        public int hp = 100;
        public int dmg;
        public int gold = 0;
        public int x,y;
        public List<Tuple<int, int>> whiskeyLoc = new List<Tuple<int, int>>();
        public int[] townhallLoc = new int[2];
        public bool townHallFelfedezett = false;
        public new bool felfed = true;
        public bool mindentFelfedezett = false;
        public bool nyert = false;
        public string allapot = "";
        public Sheriff()
        {
            Random random = new Random();
            dmg = random.Next(20, 36);
        }
        public override void toString()
        {
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{'S',3} ");
            Console.ResetColor();
        }
        public void dontes(VarosElem[,] varos)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    try
                    {
                        if (varos[this.x + i, this.y + j] is Bandit bandita)
                        {
                            tamad(bandita);
                            allapot = "Harcol";
                            return;
                        }
                        if (varos[this.x + i, this.y + j] is Gold)
                        {
                            gold++;
                            lepes(varos, this.x + i, this.y + j);
                            allapot = "Aranyat talált";
                            return;
                        }
                        if (varos[this.x + i, this.y + j] is Whiskey whiskey && hp <= 60)
                        {
                            Tuple<int, int> wp = new Tuple<int, int>(this.x + i, this.y + j);
                            lepes(varos, this.x + i, this.y + j);
                            whiskeyLoc.Remove(wp);
                            whiskey.heal(this);
                            allapot = "Heal-elődik";
                            return;
                        }
                        if (varos[this.x + i, this.y + j] is Whiskey && hp > 60)
                        {
                            Tuple<int, int> wp = new Tuple<int, int>(this.x + i, this.y + j);
                            if (!whiskeyLoc.Contains(wp))
                            {
                                whiskeyLoc.Add(new Tuple<int, int>(this.x + i, this.y + j));
                            }
                            FelfedezoKorut(varos);
                            allapot = "Whiskey-t talált, felfedez tovább";
                            return;
                        }
                        if (varos[this.x + i, this.y + j] is Townhall && gold < 5)
                        {
                            townhallLoc[0] = this.x;
                            townhallLoc[1] = this.y;
                            townHallFelfedezett = true;
                            FelfedezoKorut(varos);
                            allapot = "Városházát talált, felfedez tovább";
                            return;
                        }
                        if (varos[this.x + i, this.y + j] is Townhall && gold == 5)
                        {
                            lepes(varos, this.x + i, this.y + j);
                            Veg();
                            return;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                    

                }
            }

            if (hp <= 60)
            {
                if (whiskeyLoc.Count > 0)
                {
                    WhiskeyKeres(varos);
                    allapot = "Talált már, Whiskey-t keres";
                    return;
                }
                else
                {
                    FelfedezoKorut(varos);
                    allapot = "Whiskey-t keres";
                    return;
                }
            }
            if (gold == 5 && townHallFelfedezett)
            {
                MoveBFS(varos, townhallLoc[0], townhallLoc[1]);
                allapot = "Kimenekít";
                return;
            }
            if (mindentFelfedezett && gold < 5)
            {
                Vadaszik(varos);
                allapot = "Banditára vadászik";
                return;
            }
            if (!mindentFelfedezett && gold < 5)
            {
                FelfedezoKorut(varos);
                allapot = "Felfedez";
                return;
            }
            if (gold == 5 && !townHallFelfedezett)
            {
                FelfedezoKorut(varos);
                allapot = "Városházát keres";
                return;
            }
            //if (!mindentFelfedezett)
            //{
            //    allapot = "Felfedez";
            //    FelfedezoKorut(varos); 
            //}
        }

        private void Vadaszik(VarosElem[,] varos)
        {
            int nearestX = -1, nearestY = -1;
            double nearestDistance = double.MaxValue;

            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    if (varos[i, j] is Bandit)
                    {
                        double distance = Math.Sqrt(Math.Pow(i - x, 2) + Math.Pow(j - y, 2));
                        if (distance < nearestDistance)
                        {
                            nearestX = i;
                            nearestY = j;
                            nearestDistance = distance;
                        }
                    }
                }
            }

            if (nearestX != -1 && nearestY != -1)
            {
                
                try
                {
                    MoveBFS(varos, nearestX, nearestY);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Hiba történt az útvonal keresése során: {e.Message}");
                }
            }

        }

        private void WhiskeyKeres(VarosElem[,] varos)
        {
            int nearestX = -1, nearestY = -1;
            double nearestDistance = double.MaxValue;

            for (int i = 0; i < whiskeyLoc.Count; i++)
            {
                double distance = Math.Sqrt(Math.Pow(whiskeyLoc[i].Item1 - x, 2) + Math.Pow(whiskeyLoc[i].Item2 - y, 2));
                if (distance < nearestDistance)
                {
                    nearestX = whiskeyLoc[i].Item1;
                    nearestY = whiskeyLoc[i].Item2;
                    nearestDistance = distance;
                }
            }   
            
            if (nearestX != -1 && nearestY != -1) MoveBFS(varos, nearestX, nearestY);
        }

        private void Veg()
        {
            nyert = true;
            Console.WriteLine("Sheriff nyert!");
            Console.ReadKey();
        }

        public void FelfedezoKorut(VarosElem[,] varos)
        {
            LegkozFelfedezetlen(varos);
            
        }
        private void lepes(VarosElem[,] varos, int ujX, int ujY)
        {
            Ground ground = new Ground();
            varos[this.x, this.y] = ground;
            varos[ujX, ujY] = this;
            this.x = ujX;
            this.y = ujY;
        }

        private void tamad(Bandit bandit)
        {
            bandit.hp -= this.dmg;
            if (bandit.hp <= 0)
            {
                gold += bandit.gold;
            }
        }
        private void LegkozFelfedezetlen(VarosElem[,] varos)
        {
            int nearestX = -1, nearestY = -1;
            double nearestDistance = double.MaxValue;

            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    if (!varos[i, j].felfed && varos[i, j] is Ground) 
                    {
                        double distance = Math.Sqrt(Math.Pow(i - x, 2) + Math.Pow(j - y, 2));
                        if (distance < nearestDistance)
                        {
                            nearestX = i;
                            nearestY = j;
                            nearestDistance = distance;
                        }
                    }
                }
            }
            if (nearestX != -1 && nearestY != -1) MoveBFS(varos, nearestX, nearestY);
            else if (nearestX == -1 && nearestY == -1) mindentFelfedezett = true;
        }
        public void MoveBFS(VarosElem[,] varos, int targetX, int targetY)
        {
            // A BFS-hez sor használata, ahol csomópontokat fogunk tárolni
            Queue<Node> queue = new Queue<Node>();

            // A már feldolgozott csomópontokat ebben tároljuk
            HashSet<Node> visited = new HashSet<Node>();

            // Kiindulási csomópont
            Node startNode = new Node(this.x, this.y);
            Node targetNode = new Node(targetX, targetY);

            // A kiindulási csomópontot hozzáadjuk a sorhoz
            queue.Enqueue(startNode);
            visited.Add(startNode);

            // Amíg van elem a sorban, folytatjuk a keresést
            while (queue.Count > 0)
            {
                // Kivesszük a sorból az aktuális csomópontot
                Node currentNode = queue.Dequeue();

                // Ha elértük a célt, visszafejtjük az útvonalat
                if (currentNode.X == targetNode.X && currentNode.Y == targetNode.Y)
                {
                    RetracePath(varos, startNode, currentNode);
                    return;
                }

                // Megvizsgáljuk a szomszédokat
                foreach (var neighbor in GetNeighbors(varos, currentNode))
                {
                    // Csak akkor adjuk hozzá, ha még nem jártunk ott
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        neighbor.Parent = currentNode;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            // Ha ide jutunk, nem találtunk érvényes útvonalat
            Console.WriteLine("Nincs érvényes útvonal!");
        }

        private void RetracePath(VarosElem[,] varos, Node startNode, Node endNode)
        {
            Node currentNode = endNode;

            List<Node> path = new List<Node>();

            // Visszafejtjük az útvonalat a célhoz
            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            // Megfordítjuk az útvonalat, hogy a kiindulási pontból a cél felé haladjunk
            path.Reverse();

            // Megtesszük az első lépést az útvonalon
            if (path.Count > 0)
            {
                Node nextStep = path[0];
                lepes(varos, nextStep.X, nextStep.Y);
            }
        }

        // Szomszédok lekérdezése (ugyanúgy működik, mint az A* algoritmusban)
        private List<Node> GetNeighbors(VarosElem[,] varos, Node node)
        {
            List<Node> neighbors = new List<Node>();

            int[] dx = { 0, 1, 0, -1, -1, 1, -1, 1 };
            int[] dy = { 1, 0, -1, 0, -1, -1, 1, 1 };

            for (int i = 0; i < 8; i++)
            {
                int newX = node.X + dx[i];
                int newY = node.Y + dy[i];

                if (newX >= 0 && newX < 25 && newY >= 0 && newY < 25 && varos[newX, newY] is Ground)
                {
                    neighbors.Add(new Node(newX, newY));
                }
            }

            return neighbors;
        }


    }

    public class Node
    {
        public int X, Y;
        public double G, H;  
        public Node Parent;

        public double F { get { return G + H; } }  

        public Node(int x, int y)
        {
            X = x;
            Y = y;
            G = 0;
            H = 0;
            Parent = null;
        }
    }
}
