using System;
using System.Collections.Generic;
using System.Linq;
using AStar.src;
using AStar.src.AStar;

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
        private static Grid grid;
        private static Point heroePos = new Point(3, 3);
        private static Point targetPos = new Point(50, 20);
        private static Renderer renderer = new Renderer();

        private static int WC = 0;
        private static int DC = 0;

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
                if (!prevPostDict.ContainsKey(item))
                {
                    toCheckNeighbors.Enqueue(item);
                    prevPostDict.Add(item, current);
                    visited.Add(item);

                    WC++;
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

        public static void SearchInWidthMain()
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
                SearchInWidth2(visited, prevPosDict, toCheckNeighbors, grid.GetNeighbors);
                
                SetGrid(visited, heroePos, targetPos, '+');
                renderer.RenderGrid(grid);
            }

            var path = GetPathInWidth(targetPos, heroePos, prevPosDict);
            SetGrid(path, heroePos, targetPos, 'P');
            renderer.RenderGrid(grid);
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
                var newPointCost = cost + grid.GetCostForPoint(item);
                if (!pointCost.ContainsKey(item) || newPointCost < pointCost[item])
                {
                    pointCost[item] = newPointCost;
                    toCheckNeighbors.Enqueue((item, newPointCost));
                    prevPostDict.Add(item, (current.Item1, newPointCost));
                    visited.Add(item);

                    DC++;
#if FALSE
                    SetGrid(visited, heroePos, targetPos, '+');
                    renderer.RenderGrid(_grid);
#endif
                }
            }

            toCheckNeighbors.OrderBy(o => o.Item2);
        }

        public static List<Point> GetPathInDijkstra(Point targetPos, 
            Point heroePos,
            Dictionary<Point, (Point, int)> prevPosDictWithCost)
        {
            var current = targetPos;
            var path = new List<Point>() { current };
            while (current != heroePos)
            {
                current = prevPosDictWithCost[current].Item1;
                path.Add(current);
            }
            return path;
        }

        public static void DijkstraMain()
        {
            Console.Clear();
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

                if (visited.Contains(targetPos))
				       {
                    SetGrid(visited, heroePos, targetPos, '+');
                    break;
                }

                DijkstraSearch(visited, 
                    prevPosDictWithCost, 
                    costForPoint,
                    toCheckNeighborsWithCost,
                    grid.GetNeighbors);

                SetGrid(visited, heroePos, targetPos, '+');
                renderer.RenderGrid(grid);
            }

            var path = GetPathInDijkstra(targetPos, heroePos, prevPosDictWithCost);
            SetGrid(path, heroePos, targetPos, 'P');
            renderer.RenderGrid(grid);
        }

        static void Main(string[] args)
        {
            Console.Clear();
            var heroePos = new Point(23, 3);
            var targetPos = new Point(50, 20);

            grid = new Grid(25, 54);
            grid.FillGrid();
            grid.FillVerticalBlock(2, 25, 12, Grid.WALL);
            grid.FillVerticalBlock(1, 35, 10, Grid.WALL);
            //grid.FillHorizontalBlock(10, 2, 26, Grid.WALL);
            grid.FillHorizontalBlock(10, 15, 10, Grid.WALL);
            grid.FillHorizontalBlock(10, 1, 18, Grid.WALL);
            grid.FillHorizontalBlock(2, 15, 10, Grid.WALL);
            grid.FillHorizontalBlock(5, 2, 10, Grid.WALL);
            grid.FillHorizontalBlock(8, 40, 12, Grid.WALL);
            grid.FillHorizontalBlock(20, 23, 13, Grid.WALL);
            grid[heroePos] = 'H';
            grid[targetPos] = 'T';

            //renderer.RenderGrid(grid);
            //SearchInWidthMain();
            //DijkstraMain();

            var finder = new PathFinder(grid);
            var path = finder.GetPath(heroePos, targetPos);
            SetGrid(finder.Visited, heroePos, targetPos, '+');
            SetGrid(path, heroePos, targetPos, 'P');

            renderer.RenderGrid(grid);
            Console.WriteLine($"Path length: {path.Count}");

            Console.ReadLine();
        }

        public static void SetGrid(List<Point> list, 
            Point heroePos, 
            Point targetPos,
            char c)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (item == heroePos)
                {
                    continue;
                }
                else if (item == targetPos)
                {
                    grid[item] = 'R';
                }
                else if (grid[item] == Grid.FOREST)
                {
                    continue;
                }
                else
                {
                    grid[item] = c;
                }
            }
        }
    }
}
