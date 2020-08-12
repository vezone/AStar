using System;
using static System.Console;

namespace AStar.src
{
    public class Renderer
    {
        public Renderer()
        { 
            CursorVisible = false;
        }

        public void Refresh()
        {
            SetCursorPosition(0, 0);
        }

        public static void PrintColor(char toPrint, ConsoleColor color)
		{
            var prevColor = ForegroundColor;
            ForegroundColor = color;
            Write(toPrint);
            ForegroundColor = prevColor;
		}

        public void RenderGrid(Grid grid)
        {
            for (int y = 0; y < grid.RowLength; y++)
            {
                for (int x = 0; x < grid.ColumnLength; x++)
                {
                    if (grid[y][x] == '+')
                    {
                        PrintColor(grid[y][x], ConsoleColor.Green);
                    }
                    else if (grid[y][x] == '-')
                    {
                        PrintColor(grid[y][x], ConsoleColor.Red);
                    }
                    else if (grid[y][x] == 'P')
                    {
                        PrintColor(grid[y][x], ConsoleColor.Yellow);
                    }
                    else if (grid[y][x] == 'H' || grid[y][x] == 'T' || grid[y][x] == 'R')
                    {
                        PrintColor(grid[y][x], ConsoleColor.Magenta);
                    }
                    else
					{
                        PrintColor(grid[y][x], ConsoleColor.White);
                    }
                }
                WriteLine();
            }
        }

    }
}
