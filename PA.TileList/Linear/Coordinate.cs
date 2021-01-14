namespace PA.TileList.Linear
{
    public class Coordinate : ICoordinate
    {
        public static Coordinate Zero = new Coordinate(0, 0);

        public static int Dim = 2;

        public int[] Coordinates;

        public Coordinate(int x, int y)
        {
            Coordinates = new int[2];
            X = x;
            Y = y;
        }
#if DEBUG
        public object Tag { get; set; }
#endif

        public int X
        {
            get => Coordinates[0];
            set => Coordinates[0] = value;
        }

        public int Y
        {
            get => Coordinates[1];
            set => Coordinates[1] = value;
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }


        public virtual object Clone(int x, int y)
        {
            var c = MemberwiseClone() as ICoordinate;
            c.X = x;
            c.Y = y;
            return c;
        }

        public void Offset(ICoordinate c)
        {
            Offset(c.X, c.Y);
        }

        public void Offset(int shiftX, int shiftY)
        {
            X += shiftX;
            Y += shiftY;
        }

        public static Coordinate operator -(Coordinate a, ICoordinate b)
        {
            return new Coordinate(a.X - b.X, a.Y - b.Y);
        }

        public static Coordinate operator +(Coordinate a, ICoordinate b)
        {
            return new Coordinate(a.X + b.X, a.Y + b.Y);
        }

        public override string ToString()
        {
            return X + "," + Y;
        }
    }
}