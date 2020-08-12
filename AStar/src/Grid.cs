﻿using AStar.src.AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStar.src
{
    public class Grid
    {
        private char[][] _data;
        private int _rowLength;
        private int _columnLength;

        public const char SPACE = '-';
        public const char VISITED = '+';
        public const char WALL = 'W';
        public const char TARGET = 'T';
        public const char FROM = 'A';
        public const char FOREST = 'F';

        public int RowLength => _rowLength;
        public int ColumnLength => _columnLength;

        public Grid(int rowLength, int columnLength)
        {
            _data = new char[rowLength][];
            for (int i = 0; i < rowLength; i++)
            {
                _data[i] = new char[columnLength];
            }
            _rowLength = rowLength;
            _columnLength = columnLength;
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
            for (int y = 0; y < RowLength; y++)
            {
                for (int x = 0; x < ColumnLength; x++)
                {
                    this[new Point(x, y)] = SPACE;
                }
            }
        }

        public void FillChar(int ypos, int xpos, char c)
        {
            this[new Point(xpos, ypos)] = c;
        }

        public void FillVerticalBlock(int ypos, int xpos, int rowLength, char c)
        {
            if (ypos + rowLength >= RowLength)
            {
                return;
            }

            for (int y = ypos; y < ypos + rowLength; y++)
            {
                this[new Point(xpos, y)] = c;
            }
        }

        public void FillHorizontalBlock(int ypos, int xpos, int colLength, char c)
        {
            if (xpos + colLength >= ColumnLength)
            {
                return;
            }

            for (int x = xpos; x < xpos + colLength; x++)
            {
                this[new Point(x, ypos)] = c;
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
            var list = new List<Point>();

            var p0 = new Point(point.X, point.Y - 1);
            var p1 = new Point(point.X + 1, point.Y);
            var p2 = new Point(point.X, point.Y + 1);
            var p3 = new Point(point.X - 1, point.Y);

            if (IsPointInGrid(p0) && IsPointNotWall(p0)) list.Add(p0);
            if (IsPointInGrid(p1) && IsPointNotWall(p1)) list.Add(p1);
            if (IsPointInGrid(p2) && IsPointNotWall(p2)) list.Add(p2);
            if (IsPointInGrid(p3) && IsPointNotWall(p3)) list.Add(p3);

            return list;
        }

        public List<Point> GetWideNeighbors(Point point)
        {
            var list = new List<Point>();
            var p0 = new Point(point.X - 1, point.Y - 1);
            var p1 = new Point(point.X, point.Y - 1);
            var p2 = new Point(point.X + 1, point.Y - 1);
            var p3 = new Point(point.X + 1, point.Y);
            var p4 = new Point(point.X + 1, point.Y + 1);
            var p5 = new Point(point.X, point.Y - 1);
            var p6 = new Point(point.X - 1, point.Y - 1);
            var p7 = new Point(point.X - 1, point.Y);

            if (IsPointInGrid(p0) && IsPointNotWall(p0)) list.Add(p0);
            if (IsPointInGrid(p1) && IsPointNotWall(p1)) list.Add(p1);
            if (IsPointInGrid(p2) && IsPointNotWall(p2)) list.Add(p2);
            if (IsPointInGrid(p3) && IsPointNotWall(p3)) list.Add(p3);
            if (IsPointInGrid(p4) && IsPointNotWall(p4)) list.Add(p4);
            if (IsPointInGrid(p5) && IsPointNotWall(p5)) list.Add(p5);
            if (IsPointInGrid(p6) && IsPointNotWall(p6)) list.Add(p6);
            if (IsPointInGrid(p7) && IsPointNotWall(p7)) list.Add(p7);

            return list;
        }

        public int GetCostForPoint(Point to)
        {
            if (this[to] == SPACE)
            {
                return 1;
            }
            else if (this[to] == FOREST)
            {
                return 30;
            }

            return 2;
        }

        public int GetDistance(Point from, Point to)
		{
            //get distance by Manhattan formula
            return Math.Abs(from.X - to.X) + Math.Abs(from.Y - from.X);
		}

        public double GetAccurateDistance(Point from, Point to)
        {
            //get distance by Manhattan formula
            return Euclide.Distance(from, to);
        }
    }
}
