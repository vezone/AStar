namespace AStar.src.AStar
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
            var otherPoint = (Point)obj;
            return this == otherPoint; 
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

		public override int GetHashCode()
		{
			return (X << 16) ^ (Y << 8);
		}

		public override string ToString()
		{
			return $"{X}, {Y}";
		}
	}
}