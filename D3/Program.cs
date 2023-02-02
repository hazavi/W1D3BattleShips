using System;
using System.Activities.Expressions;
using System.Media;
namespace W1D3BattleShip

{
    class Program
    {
        enum Map { Ocean = 9, Ship = 7, Hit = 4, Miss = 11 }
        static Random random = new Random();
        static SoundPlayer player = new SoundPlayer();
        static int size;
        static int[,,,] grid;
        static int cX = 0, cY = 0;

        static void Main(string[] args)
        {
            Setup();
            Game();
        }
        #region Setup
        static void Setup()
        {
            Console.WriteLine("\nBattleships!\n");

            do { Console.Write("Gridsize: "); }
            while (!int.TryParse(Console.ReadLine(), out size));
            grid = new int[2, 2, size, size];
            CreateGrid();

            int ships;
            do { Console.Write("# Ships: "); }
            while (!int.TryParse(Console.ReadLine(), out ships));
            CreateShips(ships);

            Console.CursorSize = 100;
            Console.SetWindowSize(size < 5 ? 5 : size, size * 2 + 3);
            Console.SetBufferSize(size < 5 ? 5 : size, size * 2 + 3);
        }

        static void CreateGrid()
        {
            for (int p = 0; p <= 1; p++)
                for (int t = 0; t <= 1; t++)
                    for (int x = 0; x < size; x++)
                        for (int y = 0; y < size; y++)
                            grid[p, t, x, y] = (int)Map.Ocean;
        }

        static void CreateShips(int ships)
        {
            for (int p = 0; p <= 1; p++)
                for (int s = 0; s < ships; s++)
                    while (true)
                    {
                int x = random.Next(size);
                int y = random.Next(size);
                if (grid[p, 0, x, y] == (int) Map.Ocean)
                {
                    grid[p, 0, x, y] = (int) Map.Ship;
                    break;
                }
            }
        }
        #endregion
        static void Game()  
        {
            while (true)
            {
                ShowGrids();
                HumanTurn();
                ShowGrids();
                AITurn();
            }
        }

        static void ShowGrids()
        {
            Console.SetCursorPosition(0, 0);
            for (int t = 0; t <= 1; t++)
            {
                Console.WriteLine(t == 0  ? "Our" : "Enemy");
                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        Console.ForegroundColor = (ConsoleColor)grid[0, t, x, y];
                        Console.Write('■');
                    }
                    Console.WriteLine();
                }
            }
        }

        static void HumanTurn()
        {
            bool fired = false;
            while (!fired)
            {
                Console.SetCursorPosition(cX, cY + size + 2);
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.NumPad4:
                        if (cX > 0) cX--;
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.NumPad8:
                        if (cY > 0) cY--;
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.NumPad6:
                        if (cX < size - 1) cX++;
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.NumPad2:
                        if (cY < size - 1) cY++;
                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter:
                        Fire(cX, cY, 1);
                        fired = true;
                        break;
                    default:
                        break;
                }
            }
        }

        static void AITurn()
        {
            while (true)
            {
                int x = random.Next(size);
                int y = random.Next(size);
                if (grid[1, 1, x, y] == (int)Map.Ocean)
                {
                    Fire(x, y, 0);
                    break;
                }
            }
        }

        static void Fire(int x, int y, int tp)
        {
            player.Stream = Properties.Resources.fire;
            player.PlaySync();

            Map result = grid[tp, 0, x, y] == (int)Map.Ship ? Map.Hit else Map.Miss;
            player.Stream = result == Map.Hit ? Properties.Resources.hit : Properties.Resources.miss;
            player.PlaySync();

            grid[tp, 0, x, y] = (int)result;
            grid[(tp + 1) % 2, 1, x, y] = (int)result;
        }
    }
}