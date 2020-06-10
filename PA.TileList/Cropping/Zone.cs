using System.Collections;
using System.Collections.Generic;
using PA.TileList.Linear;

namespace PA.TileList.Cropping
{
    public class Zone : IZone, IEnumerable<Coordinate>
    {
        public static Zone Unitary = new Zone(0, 0, 0, 0);

        public Zone(IZone a)
        {
            this.Min = a.Min;
            this.Max = a.Max;
        }

        public Zone(Coordinate Min, Coordinate Max)
        {
            this.Min = Min;
            this.Max = Max;
        }

        public Zone(int MinX, int MinY, int MaxX, int MaxY)
        {
            this.Min = new Coordinate(MinX, MinY);
            this.Max = new Coordinate(MaxX, MaxY);
        }

        public Coordinate Min { get; set; }
        public Coordinate Max { get; set; }

        public ushort SizeX
        {
            get { return (ushort)(this.Max.X - this.Min.X + 1); }
        }

        public ushort SizeY
        {
            get { return (ushort)(this.Max.Y - this.Min.Y + 1); }
        }

        public void Offset(ICoordinate c)
        {
            this.Offset(c.X, c.Y);
        }

        public Coordinate Center()
        {
            return new Coordinate((int)(this.SizeX / 2f + this.Min.X), (int)(this.SizeY / 2f + this.Min.Y));
        }

        public bool Contains(ICoordinate c)
        {
            return this.Contains(c.X, c.Y);
        }

        public bool Contains(IZone b)
        {
            return (this.Min.X <= b.Min.X) && (b.Max.X <= this.Max.X) && (this.Min.Y <= b.Min.Y) && (b.Max.Y <= this.Max.Y);
        }

        public IEnumerator<Coordinate> GetEnumerator()
        {
            return this.GetInnerCoordinates().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetInnerCoordinates().GetEnumerator();
        }

        public void Offset(int shiftX, int shiftY)
        {
            this.Min.Offset(shiftX, shiftY);
            this.Max.Offset(shiftX, shiftY);
        }

        public bool Contains(int x, int y)
        {
            return (this.Min.X <= x) && (x <= this.Max.X) && (this.Min.Y <= y) && (y <= this.Max.Y);
        }

        public static bool operator ==(Zone a, IZone b)
        {
            return (a.Min == b.Min) && (a.Max == b.Max);
        }

        public static bool operator !=(Zone a, IZone b)
        {
            return (a.Min != b.Min) || (a.Max != b.Max);
        }

        public override bool Equals(object obj)
        {
            return obj is IZone
                ? (this.Min == (obj as IZone).Min) || (this.Max == (obj as IZone).Max)
                : base.Equals(obj);
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

        private IEnumerable<Coordinate> GetInnerCoordinates()
        {
            for (var x = this.Min.X; x <= this.Max.X; x++)
                for (var y = this.Min.Y; y <= this.Max.Y; y++)
                    yield return new Coordinate(x, y);
        }

        public static Zone From(IZone a)
        {
            return new Zone(a.Min, a.Max);
        }
    }
}