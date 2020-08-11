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

        public static bool ContainsPoint(this Dictionary<Point, Point> dict, Point point)
        {
            return dict.Keys.Any(o => o == point);
        }
    }

    class Program
    {
        private static Grid _grid;
        
        public delegate List<Point> GetNeighborsDelegate(Point point);

        public static void SearchInWidth(List<Point> visited, Queue<Point> toCheckNeighbors, GetNeighborsDelegate getNeighbors)
		{
            var current = toCheckNeighbors.Dequeue();
            var neighbors = getNeighbors(current);

            foreach (var item in neighbors)
            {
                if (!visited.ContainsPoint(item))
                {
                    toCheckNeighbors.Enqueue(item);
                    visited.Add(item);
                }
            }
        }

        public static void SearchInWidth2(List<Point> visited, Dictionary<Point, Point> prevPostDict, Queue<Point> toCheckNeighbors, GetNeighborsDelegate getNeighbors)
        {
            var current = toCheckNeighbors.Dequeue();
            var neighbors = getNeighbors(current);

            foreach (var item in neighbors)
            {
                if (!prevPostDict.ContainsPoint(item))
                {
                    toCheckNeighbors.Enqueue(item);
                    prevPostDict.Add(item, current);
                    visited.Add(item);
                }
            }
        }

        static void Main(string[] args)
        {
            Console.Clear();
            _grid = new Grid(25, 54);
            var renderer = new Renderer();
            _grid.FillGrid();
            _grid.FillVerticalBlock(2, 25, 12);
            _grid.FillHorizontalBlock(10, 10, 16);

            _grid.FillHorizontalBlock(10, 35, 16);

            var heroePos = new Point(3, 3);
            var targetPos = new Point(50, 20);
            _grid[heroePos] = 'H';
            _grid[targetPos] = 'T';

            var visited = new List<Point>();
            var toCheckNeighbors = new Queue<Point>();
            var prevPosDict = new Dictionary<Point, Point>();

            visited.Add(heroePos);
            toCheckNeighbors.Enqueue(heroePos);
            renderer.RenderGrid(_grid);
            bool run = true;
            while (run)
            {
                renderer.Refresh();

                if (toCheckNeighbors.Count <= 0)
                {
                    break;
                }

                //SearchInWidth(visited, toCheckNeighbors, _grid.GetNeighbors);
                SearchInWidth2(visited, prevPosDict, toCheckNeighbors, _grid.GetNeighbors);
                //SearchInWidth(visited, toCheckNeighbors, _grid.GetWideNeighbors);
                //SearchInWidth2(_grid, visited, toCheckNeighbors, targetPos);

                for (int i = 0; i < visited.Count; i++)
                {
                    var item = visited[i];
                    if (item == heroePos)
                    {
                        continue;
                    }
                    else if (item == targetPos)
                    {
                        _grid[item] = 'R';
                    }
                    else
                    {
                        _grid[item] = '+';
                    }
                }

                renderer.RenderGrid(_grid);

                //Thread.Sleep(300);
            }

            //renderer.RenderGrid(_grid);

            var current = targetPos;
            var path = new List<Point>() { current };
            while (current != heroePos)
			{
                current = prevPosDict[current];
                path.Add(current);
			}

            for (int i = 0; i < path.Count; i++)
            {
                var item = path[i];
                if (item == heroePos)
                {
                    continue;
                }
                else if (item == targetPos)
                {
                    _grid[item] = 'R';
                }
                else
                {
                    _grid[item] = 'P';
                }
            }
            renderer.RenderGrid(_grid);

            Console.ReadLine();

        }
    }
}
