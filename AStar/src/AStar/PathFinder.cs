using AStar.src.PriorityQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar.src.AStar
{
	class PathFinder : IPathFinder
	{
        private Grid _grid;
        public List<Point> Visited { get; private set; }

		public PathFinder(Grid otherGrid)
		{
            _grid = otherGrid;
        }

        private void GreedySearch(
            Point target,
            Dictionary<Point, Point> prevPosition,
            SimplePriorityQueue<Point, double> toCheckNeighbors)
        {
            var current = toCheckNeighbors.Dequeue();
            var neighbors = _grid.GetNeighbors(current);

            foreach (var item in neighbors)
            {
                if (!prevPosition.ContainsKey(item))
                {
                    var distance = _grid.GetAccurateDistance(item, target);
                    toCheckNeighbors.Enqueue(item, distance);
                    prevPosition.Add(item, current);
                    Visited.Add(item);
                }
            }
        }

        public List<Point> GetPath(Point from, Point to)
		{
            Visited = new List<Point>();
            var prevPosition = new Dictionary<Point, Point>();
            var toCheckNeighborsWithCost = new SimplePriorityQueue<Point, double>();
            var costForPoint = new Dictionary<Point, int>();

            Visited.Add(from);
            toCheckNeighborsWithCost.Enqueue(from, 0);
            costForPoint[from] = 0;

            bool run = true;
            while (run)
            {
                if (toCheckNeighborsWithCost.Count <= 0)
                {
                    break;
                }

                if (Visited.Contains(to))
                {
                    break;
                }

                GreedySearch(to, prevPosition, toCheckNeighborsWithCost);
            }

            var current = to;
            var path = new List<Point>() { current };
            while (current != from)
            {
                current = prevPosition[current];
                path.Add(current);
            }

            return path;
        }
	}
}
