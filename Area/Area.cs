using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList
{
    public class Area : IArea, ICloneable
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

        public static bool operator ==(Area a1, IArea a2)
        {
            return (a1.Min == a2.Min) && (a1.Max == a2.Max);
        }

        public static bool operator !=(Area a1, IArea a2)
        {
            return (a1.Min != a2.Min) || (a1.Max != a2.Max);
        }

        public override bool Equals(object obj)
        {
            return (typeof(IArea).IsAssignableFrom(obj.GetType())) ? (this.Min == (obj as IArea).Min) || (this.Max == (obj as IArea).Max) : base.Equals(obj);
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
    }
}
