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

        public static List<Point> GetPathInWidth(Point targetPos, Point heroePos, Dictionary<Point, Point> prevPosDict)
        {
            var current = targetPos;
            var path = new List<Point>() { current };
            while (current != heroePos)
            {
                current = prevPosDict[current];
                path.Add(current);
            }
            return path;
        }

        public static void SearchInWidthMain(Grid grid, Renderer renderer)
        {
            Console.Clear();
            var heroePos = new Point(3, 3);
            var targetPos = new Point(50, 20);
            var visited = new List<Point>();
            var toCheckNeighbors = new Queue<Point>();
            var prevPosDict = new Dictionary<Point, Point>();

            visited.Add(heroePos);
            toCheckNeighbors.Enqueue(heroePos);

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
                SetGrid(visited, heroePos, targetPos);

                renderer.RenderGrid(_grid);
            }

            var path = GetPathInWidth(targetPos, heroePos, prevPosDict);
            SetGrid(path, heroePos, targetPos);
            renderer.RenderGrid(_grid);
        }

        public static void DijkstraSearch(List<Point> visited, 
            Dictionary<Point, (Point, int)> prevPostDict, 
            Dictionary<Point, int> pointCost, 
            Queue<(Point, int)> toCheckNeighbors, 
            GetNeighborsDelegate getNeighbors)
        {
            var current = toCheckNeighbors.Dequeue();
            var neighbors = getNeighbors(current.Item1);

            foreach (var item in neighbors)
            {
                var cost = pointCost.ContainsKey(item) ? pointCost[item] : 0;
                var newPointCost = cost + _grid.GetCostForPoint(item);
                if (!pointCost.ContainsKey(item) || newPointCost < pointCost[item])
                {
                    toCheckNeighbors.Enqueue((item, newPointCost));
                    prevPostDict.Add(item, (current.Item1, newPointCost));
                    visited.Add(item);
                }
            }

            toCheckNeighbors.OrderBy(o => o.Item2);
        }

        public static void DijkstraMain(Grid grid, Renderer renderer)
        {
            Console.Clear();

            var heroePos = new Point(3, 3);
            var targetPos = new Point(50, 20);

            SearchInWidthMain(_grid, renderer);

            var visited = new List<Point>();
            var prevPosDict = new Dictionary<Point, Point>();

            var prevPosDictWithCost = new Dictionary<Point, (Point, int)>();
            var toCheckNeighborsWithCost = new Queue<(Point, int)>();
            var costForPoint = new Dictionary<Point, int>();

            /*
            Нужно запоминать всех Neighbors, но для 
            начала разбирать только те, что с наивисшим
            приоритетом
            */

            visited.Add(heroePos);
            toCheckNeighborsWithCost.Enqueue((heroePos, 0));

            costForPoint[heroePos] = 0;

            bool run = true;
            while (run)
            {
                renderer.Refresh();

                if (toCheckNeighborsWithCost.Count <= 0)
                {
                    break;
                }

                DijkstraSearch(visited, 
                    prevPosDictWithCost, 
                    costForPoint,
                    toCheckNeighborsWithCost, 
                    _grid.GetNeighbors);
                SetGrid(visited, heroePos, targetPos);

                renderer.RenderGrid(_grid);
            }

            //var path = GetPathInWidth(targetPos, heroePos, prevPosDict);
            //SetGrid(path, heroePos, targetPos);
            renderer.RenderGrid(_grid);

            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            Console.Clear();
            var renderer = new Renderer();
            var heroePos = new Point(3, 3);
            var targetPos = new Point(50, 20);

            _grid = new Grid(25, 54);
            _grid.FillGrid();
            _grid.FillVerticalBlock(2, 25, 12);
            _grid.FillHorizontalBlock(10, 10, 16);
            _grid.FillHorizontalBlock(10, 35, 16);
            _grid[heroePos] = 'H';
            _grid[targetPos] = 'T';

            //SearchInWidthMain(_grid, renderer);
            DijkstraMain(_grid, renderer);

            Console.ReadLine();
        }

        public static void SetGrid(List<Point> list, Point heroePos, Point targetPos)
        {
            for(int i = 0; i < list.Count; i++)
            {
                var item = list[i];
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
        }
    }
}
