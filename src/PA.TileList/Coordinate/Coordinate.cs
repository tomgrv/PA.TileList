
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.TileList
{
    public class Coordinate : ICoordinate
    {
        public static Coordinate Zero = new Coordinate(0, 0);

        public static int Dim = 2;

        public int[] Coordinates;

        public int X
        {
            get
            {
                return Coordinates[0];
            }
            set
            {
                Coordinates[0] = value;
            }
        }

        public int Y
        {
            get
            {
                return Coordinates[1];
            }
            set
            {
                Coordinates[1] = value;
            }
        }

        public Coordinate(int x, int y)
        {
            this.Coordinates = new int[2];
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

        public virtual ICoordinate Clone()
        {
            return this.MemberwiseClone() as ICoordinate;
        }


        public virtual ICoordinate Clone(int x, int y)
        {
            var c = this.MemberwiseClone() as ICoordinate;
            c.X = x;
            c.Y = y;
            return c;
        }


    }
}
