namespace AStar.src
{
    public struct Point
    {
        public int X;
        public int Y;
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            return true; 
        }

        public static bool operator== (Point a, Point b)
        {
            if (a.X == b.X && a.Y == b.Y)
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }
    }
}