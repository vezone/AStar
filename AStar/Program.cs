using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AStar.src;

namespace sandbox_project
{
    public static class ListHelper
    {
        public static bool ContainsPoint(this List<Point> list, Point point)
        {
            return list.Any(o => o.X == point.X && o.Y == point.Y);
        }
    }

    class Program
    {
        private static Grid grid;

        static void Main(string[] args)
        {
            Console.Clear();
            grid = new Grid(25, 54);
            var renderer = new Renderer();
            grid.FillGrid();
            grid.FillVerticalBlock(5, 25, 5);
            grid.FillHorizontalBlock(10, 10, 16);

            var heroePos = new Point(3, 3);
            var targetPos = new Point(50, 20);
            grid[heroePos] = 'H';
            grid[targetPos] = 'T';

            var visited = new List<Point>();
            var toCheckNeighbors = new Queue<Point>();

            visited.Add(heroePos);
            toCheckNeighbors.Enqueue(heroePos);

            bool run = true;
            while (run)
            {
                renderer.Refresh();

                if (toCheckNeighbors.Count <= 0)
                {
                    return;
                }

                var current = toCheckNeighbors.Dequeue();
                var neighbors = grid.GetNeighbors(current);

                foreach (var item in neighbors)
                {
                    if (!visited.ContainsPoint(item))
                    {
                        if (item.X < 0 || item.Y < 0 ||
                        item.X >= grid.RowLength || item.Y >= grid.ColumnLength)
                        {
                            continue;
                        }
                        toCheckNeighbors.Enqueue(item);
                        visited.Add(item);
                    }
                }

                foreach (var item in visited)
                {
                    if (item == heroePos)
                    {
                        continue;
                    }
                    grid[item] = '+';
                }
                renderer.RenderGrid(grid);

                //Thread.Sleep(300);
            }

        }
    }
}
