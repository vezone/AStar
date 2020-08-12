using System.Collections.Generic;

namespace AStar.src.AStar
{
	public interface IPathFinder
	{
		List<Point> GetPath(Point from, Point to);
	}
}