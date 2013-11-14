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
            return this.X + "," + this.Y ;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    } 
}
