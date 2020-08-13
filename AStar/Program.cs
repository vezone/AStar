using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AStar.src;
using AStar.src.AStar;

namespace sandbox_project
{
    class Program
    {
        private static Grid _grid;
        private static Renderer _renderer = new Renderer();

        static void Main(string[] args)
        {
            Console.Clear();
            var heroePos = new Point(23, 3);
            var targetPos = new Point(50, 20);

            _grid = new Grid(25, 54);
            _grid.FillGrid();
            _grid.FillVerticalBlock(2, 25, 12, Grid.WALL);
            _grid.FillVerticalBlock(1, 35, 10, Grid.WALL);
            _grid.FillHorizontalBlock(10, 15, 10, Grid.WALL);
            _grid.FillHorizontalBlock(10, 1, 18, Grid.WALL);
            _grid.FillHorizontalBlock(2, 15, 10, Grid.WALL);
            _grid.FillHorizontalBlock(5, 2, 10, Grid.WALL);
            _grid.FillHorizontalBlock(8, 40, 12, Grid.WALL);
            _grid.FillHorizontalBlock(20, 23, 13, Grid.WALL);
            _grid[heroePos] = 'H';
            _grid[targetPos] = 'T';

            _grid.AddRule((Point to) => {
                if (_grid[to] == Grid.SPACE)
				{
                    return 1;
				}
                return null;
            });

            var finder = new PathFinder(_grid);
            var path = finder.GetPath(heroePos, targetPos);
            SetGrid(finder.Visited, heroePos, targetPos, '+');
            SetGrid(path, heroePos, targetPos, 'P');

            _renderer.RenderGrid(_grid);
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
                    _grid[item] = 'R';
                }
                else if (_grid[item] == Grid.FOREST)
                {
                    continue;
                }
                else
                {
                    _grid[item] = c;
                }
            }
        }
    }
}
