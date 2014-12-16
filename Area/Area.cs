using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList
{
    public class Area : IArea, IEnumerable<ICoordinate>
    {
        public static Area Unitary = new Area(0, 0, 0, 0);

        public Coordinate Min { get; set; }
        public Coordinate Max { get; set; }
        public ushort SizeX { get { return (ushort)(this.Max.X - this.Min.X + 1); } }
        public ushort SizeY { get { return (ushort)(this.Max.Y - this.Min.Y + 1); } }

        public Area(Coordinate Min, Coordinate Max)
        {
            this.Min = Min;
            this.Max = Max;
        }

        public Area(int MinX, int MinY, int MaxX, int MaxY)
        {
            this.Min = new Coordinate(MinX, MinY);
            this.Max = new Coordinate(MaxX, MaxY);
        }

        public void Offset(ICoordinate c)
        {
            this.Offset(c.X, c.Y);
        }

        public void Offset(int shiftX, int shiftY)
        {
            this.Min.Offset(shiftX, shiftY);
            this.Max.Offset(shiftX, shiftY);
        }

        public ICoordinate Center()
        {
            return new Coordinate((int)(this.SizeX / 2f + this.Min.X), (int)(this.SizeY / 2f + this.Min.Y));
        }

        public bool Contains(ICoordinate c)
        {
            return this.Contains(c.X, c.Y);
        }

        public bool Contains(int x, int y)
        {
            return this.Min.X <= x && x <= this.Max.X && this.Min.Y <= y && y <= this.Max.Y;
        }

        public bool Contains(IArea b)
        {
            return this.Min.X <= b.Min.X && b.Max.X <= this.Max.X && this.Min.Y <= b.Min.Y && b.Max.Y <= this.Max.Y;
        }

        public static bool operator ==(Area a, IArea b)
        {
            return (a.Min == b.Min) && (a.Max == b.Max);
        }

        public static bool operator !=(Area a, IArea b)
        {
            return (a.Min != b.Min) || (a.Max != b.Max);
        }

        public override bool Equals(object obj)
        {
            return obj is IArea ? (this.Min == (obj as IArea).Min) || (this.Max == (obj as IArea).Max) : base.Equals(obj);
        }

        public override string ToString()
        {
            return this.Min.X + "," + this.Min.Y + ";" + this.Max.X + "," + this.Max.Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public IEnumerator<ICoordinate> GetEnumerator()
        {
            return this.GetInnerCoordinates().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetInnerCoordinates().GetEnumerator();
        }

        private IEnumerable<ICoordinate> GetInnerCoordinates()
        {
            for (int x = (this.Min.X); x <= (this.Max.X); x++)
            {
                for (int y = (this.Min.Y); y <= (this.Max.Y); y++)
                {
                    yield return new Coordinate(x, y);
                }
            }
        }
    }
}
