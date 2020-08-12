using AStar.src.PriorityQueue;
using System.Collections.Generic;

namespace AStar.src.AStar
{
	class PathFinder : IPathFinder
	{
        private Grid _grid;
        public List<Point> Visited { get; private set; }

		public PathFinder(Grid otherGrid)
		{
            _grid = otherGrid;
            Visited = new List<Point>();
        }

        public List<Point> GetPath(Point from, Point to)
		{
            var prevPosition = new Dictionary<Point, Point>();
            var toCheck = new SimplePriorityQueue<Point, double>();
            var costForPoint = new Dictionary<Point, int>();

            Visited.Add(from);
            toCheck.Enqueue(from, 0);
            costForPoint[from] = 0;

            while (toCheck.Count > 0)
            {
                //AStar
                var currentPoint = toCheck.Dequeue();
                var neighbors = _grid.GetNeighbors(currentPoint);

                if (currentPoint == to)
                {
                    break;
                }

                foreach (var item in neighbors)
                {
                    var prevCost = costForPoint.ContainsKey(item) ? costForPoint[item] : 0;
                    var newCost = prevCost + _grid.GetCostForPoint(item);

                    //Point(30, 0) has less priority
                    if (item == new Point(30, 2))
                    {
                        var t = 9;
                    }

                    if (!costForPoint.ContainsKey(item) || 
                        newCost < costForPoint[item])
                    {
                        costForPoint[item] = newCost;
                        var priority = newCost
                            + _grid.GetDistance(to, item) 
                            //+ _grid.GetAccurateDistance(from, item)
                            ;
                        toCheck.Enqueue(item, priority);
                        prevPosition[item] = currentPoint;

                        Visited.Add(item);
                    }
                }
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
