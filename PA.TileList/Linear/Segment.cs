using System;

namespace PA.TileList.Linear
{
    public class Segment<T>
        where T : ICoordinate
    {
        public Segment(T o, ICoordinate p)
        {
            Origin = o;
            Point = p;
        }

        public T Origin { get; }
        public ICoordinate Point { get; }

        public Coordinate Vector()
        {
            return Point.ToCoordinate() - Origin;
        }

        public bool Contains(ICoordinate p)
        {
            return p.X <= Math.Max(Origin.X, Point.X)
                   && p.X >= Math.Min(Origin.X, Point.X)
                   && p.Y <= Math.Max(Origin.Y, Point.Y)
                   && p.Y >= Math.Min(Origin.Y, Point.Y);
        }
    }
}