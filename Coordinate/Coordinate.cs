using System;
using System.Collections.Generic;
using System.Text;

namespace PA.TileList
{
    public class Coordinate : ICoordinate, ICloneable
    {
        public static Coordinate Zero = new Coordinate(0, 0);

        public int X { get; set; }
        public int Y { get; set; }

        public Coordinate(int x, int y)
        {
            this.X = x;
            this.Y = y;
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

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        public static double GetLength(ICoordinate a, ICoordinate b)
        {
            if (a.X == b.X)
            {
                return Math.Abs(b.Y - a.Y);
            }

            if (a.Y == b.Y)
            {
                return Math.Abs(b.X - a.X);
            }

            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

        public static double GetPerimeter(params ICoordinate[] list)
        {
            double p = 0;

            for (int i = 0; i < list.Length - 1; i++)
            {
                p += GetLength(list[i], list[i + 1]);
            }

            p += GetLength(list[list.Length - 1], list[0]);

            return p;
        }
    }
}
