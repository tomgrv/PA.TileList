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
            Min = a.Min;
            Max = a.Max;
        }

        public Zone(Coordinate Min, Coordinate Max)
        {
            this.Min = Min;
            this.Max = Max;
        }

        public Zone(int MinX, int MinY, int MaxX, int MaxY)
        {
            Min = new Coordinate(MinX, MinY);
            Max = new Coordinate(MaxX, MaxY);
        }

        public Coordinate Min { get; set; }
        public Coordinate Max { get; set; }

        public ushort SizeX => (ushort) (Max.X - Min.X + 1);

        public ushort SizeY => (ushort) (Max.Y - Min.Y + 1);

        public void Offset(ICoordinate c)
        {
            Offset(c.X, c.Y);
        }

        public Coordinate Center()
        {
            return new Coordinate((int) (SizeX / 2f + Min.X), (int) (SizeY / 2f + Min.Y));
        }

        public bool Contains(ICoordinate c)
        {
            return Contains(c.X, c.Y);
        }

        public bool Contains(IZone b)
        {
            return Min.X <= b.Min.X && b.Max.X <= Max.X && Min.Y <= b.Min.Y && b.Max.Y <= Max.Y;
        }

        public IEnumerator<Coordinate> GetEnumerator()
        {
            return GetInnerCoordinates().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetInnerCoordinates().GetEnumerator();
        }

        public void Offset(int shiftX, int shiftY)
        {
            Min.Offset(shiftX, shiftY);
            Max.Offset(shiftX, shiftY);
        }

        public bool Contains(int x, int y)
        {
            return Min.X <= x && x <= Max.X && Min.Y <= y && y <= Max.Y;
        }

        public static bool operator ==(Zone a, IZone b)
        {
            return a.Min == b.Min && a.Max == b.Max;
        }

        public static bool operator !=(Zone a, IZone b)
        {
            return a.Min != b.Min || a.Max != b.Max;
        }

        public override bool Equals(object obj)
        {
            return obj is IZone
                ? Min == (obj as IZone).Min || Max == (obj as IZone).Max
                : base.Equals(obj);
        }

        public override string ToString()
        {
            return Min.X + "," + Min.Y + ";" + Max.X + "," + Max.Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private IEnumerable<Coordinate> GetInnerCoordinates()
        {
            for (var x = Min.X; x <= Max.X; x++)
            for (var y = Min.Y; y <= Max.Y; y++)
                yield return new Coordinate(x, y);
        }

        public static Zone From(IZone a)
        {
            return new Zone(a.Min, a.Max);
        }
    }
}