using System;
using System.Text;
using System.Threading;

namespace GameOfLife
{
    public class Cell
    {
        public bool Alive;
        public int Gen;

        public Cell(bool alive)
        {
            this.Alive = alive;
            if (alive)
            {
                Gen = 1;
            }
            else
            {
                Gen = 0;
            }
        }

        public void UpdateCell(bool alive)
        {
            if (!alive) this.Gen = 0;
            else this.Gen++;
            this.Alive = alive;
        }
    }
    class Program
    {
        static int height = 100;
        static int width = 25;
        static Cell[,] grid;

        public static void InitGrid(Random rng)
        {
            for (int row = 0; row < width; row++)
            {
                for (int col = 0; col < height; col++)
                {
                    if (rng.Next(5) == 0) grid[col, row] = new Cell(true);
                    else grid[col, row] = new Cell(false);
                }
            }
        }

        public static Cell[,] InitBuffer(Cell[,] buffer)
        {
            for (int row = 0; row < width; row++)
            {
                for (int col = 0; col < height; col++)
                {
                    buffer[col, row] = new Cell(false);
                }
            }
            return buffer;
        }

        static void Main(string[] args)
        {
            grid = new Cell[height, width];
            Random rng = new Random();
            InitGrid(rng);
            while (true)
            {
                Evolve();
                Draw();
            }
        }

        private static void Evolve()
        {
            Cell[,] buffer = new Cell[height, width];
            buffer = InitBuffer(buffer);
            for (int col = 0; col < height; col++)
            {
                for (int row = 0; row < width; row++)
                {
                    int numAlive = 0;
                    int colCheck = col;
                    int rowCheck = row;

                    if (col == 0) colCheck = height - 2;
                    if (col == height - 1) colCheck = 1;
                    if (row == 0) rowCheck = width - 2;
                    if (row == width - 1) rowCheck = 1;

                    if (grid[colCheck - 1, rowCheck - 1].Alive) numAlive++;
                    if (grid[colCheck + 1, rowCheck + 1].Alive) numAlive++;

                    if (grid[colCheck - 1, rowCheck + 1].Alive) numAlive++;
                    if (grid[colCheck + 1, rowCheck - 1].Alive) numAlive++;

                    if (grid[colCheck, rowCheck + 1].Alive) numAlive++;
                    if (grid[colCheck, rowCheck - 1].Alive) numAlive++;

                    if (grid[colCheck + 1, rowCheck].Alive) numAlive++;
                    if (grid[colCheck - 1, rowCheck].Alive) numAlive++;

                    if (grid[col, row].Alive && numAlive < 2)
                    {
                        buffer[col, row].Gen = grid[col, row].Gen;
                        buffer[col, row].UpdateCell(false);
                    }
                    if (grid[col, row].Alive && numAlive == 2 || numAlive == 3)
                    {
                        buffer[col, row].Gen = grid[col, row].Gen;
                        buffer[col, row].UpdateCell(true);
                    }
                    if (grid[col, row].Alive && numAlive > 3)
                    {
                        buffer[col, row].Gen = grid[col, row].Gen;
                        buffer[col, row].UpdateCell(false);
                    }
                    if (!grid[col, row].Alive && numAlive == 3)
                    {
                        buffer[col, row].Gen = grid[col, row].Gen;
                        buffer[col, row].UpdateCell(true);
                    }
                }
            }
            grid = buffer;
        }

        private static void Draw()
        {
            Console.CursorVisible = false;
            //var builder = new StringBuilder();
            for (int row = 0; row < width; row++)
            {
                for (int col = 0; col < height; col++)
                {
                    if (grid[col, row].Alive)
                    {
                        if (grid[col, row].Gen == 1) Console.ForegroundColor = ConsoleColor.White;
                        else if (grid[col, row].Gen == 2) Console.ForegroundColor = ConsoleColor.Red;
                        else if (grid[col, row].Gen >= 3) Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                    if (col == height - 1) Console.Write("\n");
                }
            }
            Console.SetCursorPosition(0, 0);
        }
    }
}
