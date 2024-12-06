using System;
using System.Runtime.InteropServices;

namespace _2048
{
    internal class Program
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        const int MF_BYCOMMAND = 0x00000000;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;
        const int SC_SIZE = 0xF000;

        static void Main(string[] args)
        {
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            Console.BufferHeight = height;
            string title = "2048 게임!";
            string text = "1.시작         2.종료(Esc)";
            Console.CursorVisible = false;
            while (true)
            {
                Console.SetCursorPosition((width - title.Length) / 2, (height / 2) - 2);
                Console.Write(title);
                Console.SetCursorPosition((width - text.Length) / 2, (height / 2) + 1);
                Console.Write(text);
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        PlayGame();
                        break;
                    case ConsoleKey.Escape:
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        return;
                }
            }
        }

        static void PlayGame()
        {
            int count = 0;
            int[,] array = new int[4, 4];
            bool win = false;
            for (int i = 0; i < 2; i++)
            {
                MakeRandomNumber(array);
            }
            Print(array, count);
            do
            {
                while (CanMove(Console.ReadKey(true).Key, array, ref win) == false)
                {
                }
                MakeRandomNumber(array);
                Print(array, ++count);
            } while (win == false && CanMakeNumber(array) == true);
            string text = "　게임 오버! ";
            if (win == true)
            {
                text = "　축하합니다. 2048을 달성하셨습니다! ";
            }
            Console.ResetColor();
            Console.SetCursorPosition(((Console.WindowWidth - text.Length) / 2) - 1, Console.WindowHeight / 2);
            Console.Write(text);
            Console.ReadKey(true);
            Console.Clear();
        }

        static int[] GetRandomOrder(int length)
        {
            int[] array = new int[length];
            for(int i = 0; i < length; i++)
            {
                array[i] = i;
            }
            Random random = new Random();
            for(int i = 0; i < length; i++)
            {
                int index = random.Next(0, length);
                int temp = array[i];
                array[i] = array[index];
                array[index] = temp;
            }
            return array;
        }

        static void MakeRandomNumber(int[,] array)
        {
            int height = array.GetLength(0);
            int width = array.GetLength(1);
            int[] x = GetRandomOrder(height);
            int[] y = GetRandomOrder(width);
            for (int i = 0; i < height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    if (array[x[i], y[j]] == 0)
                    {
                        array[x[i], y[j]] = 2;
                        return;
                    }
                }
            }
        }

        static bool CanMakeNumber(int[,] array)
        {
            int height = array.GetLength(0);
            int width = array.GetLength(1);
            for (int i = 0; i < height - 1; i++)
            {
                for (int j = 0; j < width - 1; j++)
                {
                    if (array[i, j] == 0 || array[i, j + 1] == 0 || array[i + 1, j] == 0 || array[i + 1, j + 1] == 0)
                    {
                        return true;
                    }
                    else if (array[i, j] == array[i, j + 1] || array[i, j] == array[i + 1, j] || array[i + 1, j + 1] == array[i + 1, j] || array[i + 1, j + 1] == array[i, j + 1])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static int MAX_SPACE_SIZE = 7;

        static void Print(int value, int x, int y)
        {
            switch (value)
            {
                case 0:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case 4:
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case 8:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case 16:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 32:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case 64:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case 128:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case 256:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case 512:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case 1024:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
            }
            int width = Console.WindowWidth / 2;
            int half = MAX_SPACE_SIZE / 2;
            for (int i = 0; i < MAX_SPACE_SIZE; i++)
            {
                for (int j = 0; j < MAX_SPACE_SIZE; j++)
                {
                    Console.SetCursorPosition(width + ((y - half + 1) * (MAX_SPACE_SIZE * 2)) + (j * 2), (x * MAX_SPACE_SIZE) + i);
                    if (i == 0 || j == 0 || i == MAX_SPACE_SIZE - 1 || j == MAX_SPACE_SIZE - 1)
                    {
                        Console.Write("■");
                    }
                    else
                    {
                        Console.Write("　");
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.Black;
            int length = value.ToString().Length;
            Console.SetCursorPosition(width + ((y - half + 1) * (MAX_SPACE_SIZE * 2)) + (half * 2) - (length / 2), (x * MAX_SPACE_SIZE) + half);
            if (length % 2 == 1)
            {
                Console.Write("\b　" + value.ToString());
            }
            else
            {
                Console.Write("\b　" + value.ToString());
            }
        }

        static void Print(int[,] array, int count)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.White;
            int column = array.GetLength(0);
            int line = array.GetLength(1);
            for (int i = 0; i < column; i++)
            {
                for (int j = 0; j < line; j++)
                {
                    Print(array[i, j], i, j);
                }
            }
            Console.ResetColor();
            string text = "이동 횟수:" + count;
            Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2 - 1, Console.WindowHeight - 2);
            Console.Write(text);
        }
        
        static bool CanMoveUp(int[,] array, ref bool win)
        {
            int height = array.GetLength(0);
            int width = array.GetLength(1);
            bool move = false;
            bool[,] weld = new bool[height - 1, width];
            for (int i = 1; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (array[i, j] > 0)
                    {
                        for (int k = i; k > 0; k--)
                        {
                            if (array[k - 1, j] == array[k, j] && weld[k - 1, j] == false)
                            {
                                array[k - 1, j] += array[k, j];
                                if (array[k - 1, j] >= 2048)
                                {
                                    win = true;
                                }
                                array[k, j] = 0;
                                move = true;
                                weld[k - 1, j] = true;
                                break;
                            }
                            else if (array[k - 1, j] == 0)
                            {
                                array[k - 1, j] = array[k, j];
                                array[k, j] = 0;
                                move = true;
                            }
                        }
                    }
                }
            }
            return move;
        }

        static bool CanMoveDown(int[,] array, ref bool win)
        {
            int height = array.GetLength(0);
            int width = array.GetLength(1);
            bool move = false;
            bool[,] weld = new bool[height - 1, width];
            for (int i = height - 2; i >= 0 ; i--)
            {
                for (int j = 0; j < width; j++)
                {
                    if (array[i, j] > 0)
                    {
                        for (int k = i; k < height - 1; k++)
                        {
                            if (array[k + 1, j] == array[k, j] && weld[k, j] == false)
                            {
                                array[k + 1, j] += array[k, j];
                                if (array[k + 1, j] >= 2048)
                                {
                                    win = true;
                                }
                                array[k, j] = 0;
                                move = true;
                                weld[k, j] = true;
                                break;
                            }
                            else if (array[k + 1, j] == 0)
                            {
                                array[k + 1, j] = array[k, j];
                                array[k, j] = 0;
                                move = true;
                            }
                        }
                    }
                }
            }
            return move;
        }

        static bool CanMoveLeft(int[,] array, ref bool win)
        {
            int height = array.GetLength(0);
            int width = array.GetLength(1);
            bool move = false;
            bool[,] weld = new bool[height, width - 1];
            for (int i = 0; i < height; i++)
            {
                for (int j = 1; j < width; j++)
                {
                    if (array[i, j] > 0)
                    {
                        for (int k = j; k > 0; k--)
                        {
                            if (array[i, k - 1] == array[i, k] && weld[i, k - 1] == false)
                            {
                                array[i, k - 1] += array[i, k];
                                if (array[i, k - 1] >= 2048)
                                {
                                    win = true;
                                }
                                array[i, k] = 0;
                                move = true;
                                weld[i, k - 1] = true;
                                break;
                            }
                            else if (array[i, k - 1] == 0)
                            {
                                array[i, k - 1] = array[i, k];
                                array[i, k] = 0;
                                move = true;
                            }
                        }
                    }
                }
            }
            return move;
        }

        static bool CanMoveRight(int[,] array, ref bool win)
        {
            int height = array.GetLength(0);
            int width = array.GetLength(1);
            bool move = false;
            bool[,] weld = new bool[height, width - 1];
            for (int i = 0; i < height; i++)
            {
                for (int j = width - 2; j >= 0; j--)
                {
                    if (array[i, j] > 0)
                    {
                        for (int k = j; k < width - 1; k++)
                        {
                            if (array[i, k + 1] == array[i, k] && weld[i, k] == false)
                            {
                                array[i, k + 1] += array[i, k];
                                if (array[i, k + 1] >= 2048)
                                {
                                    win = true;
                                }
                                array[i, k] = 0;
                                move = true;
                                weld[i, k] = true;
                                break;
                            }
                            else if (array[i, k + 1] == 0)
                            {
                                array[i, k + 1] = array[i, k];
                                array[i, k] = 0;
                                move = true;
                            }
                        }
                    }
                }
            }
            return move;
        }

        static bool CanMove(ConsoleKey key, int[,] array, ref bool win)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    return CanMoveUp(array, ref win);
                case ConsoleKey.DownArrow:
                    return CanMoveDown(array, ref win);
                case ConsoleKey.LeftArrow:
                    return CanMoveLeft(array, ref win);
                case ConsoleKey.RightArrow:
                    return CanMoveRight(array, ref win);
            }
            return false;
        }
    }
}