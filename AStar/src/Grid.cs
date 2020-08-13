using AStar.src.AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStar.src
{
    public class Grid
    {
        private char[][] _data;
        private List<GetCostRule> _rules;

        public delegate int? GetCostRule(Point to);
        public int RowLength { get; private set; }
        public int ColumnLength { get; private set; }

        public const char SPACE = '-';
        public const char VISITED = '+';
        public const char WALL = 'W';
        public const char TARGET = 'T';
        public const char FROM = 'A';
        public const char FOREST = 'F';

        private List<Point> _neighbors = new List<Point>();
        private Point _p0 = new Point(0, 0);
        private Point _p1 = new Point(0, 0);
        private Point _p2 = new Point(0, 0);
        private Point _p3 = new Point(0, 0);

        public Grid(int rowLength, int columnLength)
        {
            _rules = new List<GetCostRule>();
            _data = new char[rowLength][];
            for (int i = 0; i < rowLength; i++)
            {
                _data[i] = new char[columnLength];
            }
            RowLength = rowLength;
            ColumnLength = columnLength;
        }

        public char[] this[int index]
        {
            get
            {
                return _data[index];
            }
            set
            {
                _data[index] = value;
            }
        }

        public char this[Point point]
        {
            get
            {
                return _data[point.Y][point.X];
            }
            set
            {
                _data[point.Y][point.X] = value;
            }
        }

        public void FillGrid()
        {
            var point = new Point(0, 0);
            for (int y = 0; y < RowLength; y++)
            {
                for (int x = 0; x < ColumnLength; x++)
                {
                    point.X = x;
                    point.Y = y;
                    this[point] = SPACE;
                }
            }
        }

        public void FillChar(int ypos, int xpos, char c)
        {
            this[new Point(xpos, ypos)] = c;
        }

        public void FillVerticalBlock(int ypos, int xpos, int rowLength, char c)
        {
            if (ypos < 0 || xpos < 0
                || ypos + rowLength >= RowLength || xpos >= ColumnLength)
            {
                return;
            }

            var point = new Point(0, 0);
            for (int y = ypos; y < ypos + rowLength; y++)
            {
                point.X = xpos;
                point.Y = y;
                this[point] = c;
            }
        }

        public void FillHorizontalBlock(int ypos, int xpos, int colLength, char c)
        {
            if (ypos < 0 || xpos < 0
                || xpos + colLength >= ColumnLength || ypos >= RowLength)
            {
                return;
            }

            var point = new Point(0, 0);
            for (int x = xpos; x <= xpos + colLength; x++)
            {
                point.X = x;
                point.Y = ypos;
                this[point] = c;
            }
        }

        public void FillRectangle(int ypos, int xpos, int height, int width, char c)
		{
            if (ypos < 0 || xpos < 0 
                || ypos + height >= RowLength 
                || xpos + width >= ColumnLength)
            {
                return;
            }

            var point = new Point(0, 0);
            for (int y = ypos; y <= ypos + height; y++)
			{
                for (int x = xpos; x <= xpos + width; x++)
                {
                    point.X = x;
                    point.Y = y;
                    this[point] = c;
                }
			}
        }

        private bool IsPointInGrid(Point point)
        {
            if (point.X >= 0
                && point.Y >= 0 
                && point.X < ColumnLength 
                && point.Y < RowLength) 
            { 
                return true; 
            }

            return false;
        }

        private bool IsPointNotWall(Point point)
        {
            if (this[point] != WALL)
            {
                return true;
            }
            return false;
        }

        public List<Point> GetNeighbors(Point point)
        {
            _neighbors.Clear();

            _p0.X = point.X;
            _p0.Y = point.Y - 1;
            _p1.X = point.X + 1;
            _p1.Y = point.Y;
            _p2.X = point.X;
            _p2.Y = point.Y + 1;
            _p3.X = point.X - 1;
            _p3.Y = point.Y;

            if (IsPointInGrid(_p0) && IsPointNotWall(_p0)) _neighbors.Add(_p0);
            if (IsPointInGrid(_p1) && IsPointNotWall(_p1)) _neighbors.Add(_p1);
            if (IsPointInGrid(_p2) && IsPointNotWall(_p2)) _neighbors.Add(_p2);
            if (IsPointInGrid(_p3) && IsPointNotWall(_p3)) _neighbors.Add(_p3);

            return _neighbors;
        }

        public void AddRule(GetCostRule rule)
		{
            _rules.Add(rule);
        }

        public int GetCostForPoint(Point to)
        {
			foreach (var rule in _rules)
			{
                var result = rule(to);
                if (result != null)
				{
                    return (int)result;
				}
			}

            return 2;
        }

        public int GetDistance(Point from, Point to)
		{
            //get distance by Manhattan formula
            return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
		}

        public double GetAccurateDistance(Point from, Point to)
        {
            //get distance by Euclide formula
            return Math.Sqrt(Math.Pow(from.X - to.X, 2) + Math.Pow(from.Y - to.Y, 2));
        }
    }
}
