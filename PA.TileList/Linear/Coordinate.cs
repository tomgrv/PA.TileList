namespace PA.TileList.Linear
{
    public class Coordinate : ICoordinate
    {
#if DEBUG
        public object Tag { get; set; }
#endif

        public static Coordinate Zero = new Coordinate(0, 0);

        public static int Dim = 2;

        public int[] Coordinates;

        public Coordinate(int x, int y)
        {
            this.Coordinates = new int[2];
            this.X = x;
            this.Y = y;
        }

        public int X
        {
            get { return this.Coordinates[0]; }
            set { this.Coordinates[0] = value; }
        }

        public int Y
        {
            get { return this.Coordinates[1]; }
            set { this.Coordinates[1] = value; }
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }


        public virtual object Clone(int x, int y)
        {
            var c = this.MemberwiseClone() as ICoordinate;
            c.X = x;
            c.Y = y;
            return c;
        }

        public void Offset(ICoordinate c)
        {
            this.Offset(c.X, c.Y);
        }

        public void Offset(int shiftX, int shiftY)
        {
            this.X += shiftX;
            this.Y += shiftY;
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
            return this.X + "," + this.Y;
        }
    }
}