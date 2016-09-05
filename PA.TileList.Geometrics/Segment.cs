using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PA.TileList.Geometrics;
using PA.TileList.Extensions;

namespace PA.TileList.Geometrics
{
    public class Segment<T>
        where T : ICoordinate
    {
        public T Origin { get; private set; }
        public ICoordinate Point { get; private set; }

        public Segment(T o, ICoordinate p)
        {
            this.Origin = o;
            this.Point = p;
        }

        public Coordinate Vector()
        {
            return this.Point.ToCoordinate() - this.Origin;
        }

        public bool Contains(ICoordinate p)
        {
            return p.X <= Math.Max(this.Origin.X, this.Point.X)
                && p.X >= Math.Min(this.Origin.X,this.Point.X)
                && p.Y <= Math.Max(this.Origin.Y, this.Point.Y)
                && p.Y >= Math.Min(this.Origin.Y, this.Point.Y);
        }

     
    }

}
