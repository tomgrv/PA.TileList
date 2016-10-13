using System;

namespace PA.TileList.Linear
{
    public class Segment<T>
        where T : ICoordinate
    {
        public Segment(T o, ICoordinate p)
        {
            this.Origin = o;
            this.Point = p;
        }

        public T Origin { get; }
        public ICoordinate Point { get; }

        public Coordinate Vector()
        {
            return this.Point.ToCoordinate() - this.Origin;
        }

        public bool Contains(ICoordinate p)
        {
            return (p.X <= Math.Max(this.Origin.X, this.Point.X))
                   && (p.X >= Math.Min(this.Origin.X, this.Point.X))
                   && (p.Y <= Math.Max(this.Origin.Y, this.Point.Y))
                   && (p.Y >= Math.Min(this.Origin.Y, this.Point.Y));
        }
    }
}