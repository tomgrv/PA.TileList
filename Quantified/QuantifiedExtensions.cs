using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList.Quantified
{
    public static class QuantifiedExtensions
    {
        public static QuantifiedTile<T> ToQuantified<T>(this ITile<T> l)
            where T : ICoordinate
        {
            return new QuantifiedTile<T>(l);
        }

        public static QuantifiedTile<T> ToQuantified<T>(this ITile<T> l, double sizeX, double sizeY)
          where T : ICoordinate
        {
            return new QuantifiedTile<T>(l, sizeX, sizeY);
        }

        public static QuantifiedTile<T> ToQuantified<T>(this ITile<T> l, double sizeX, double sizeY, double stepX, double stepY)
           where T : ICoordinate
        {
            return new QuantifiedTile<T>(l, sizeX, sizeY, stepX, stepY);
        }

        public static QuantifiedTile<T> ToQuantified<T>(this ITile<T> l, double sizeX, double sizeY, double stepX, double stepY, double refOffsetX, double refOffsetY)
           where T : ICoordinate
        {
            return new QuantifiedTile<T>(l, sizeX, sizeY, stepX, stepY, refOffsetX, refOffsetY);
        }

        public static IQuantifiedTile<T> AsQuantified<T>(this ITile<T> l)
             where T : ICoordinate
        {
            return l as IQuantifiedTile<T> ?? l.ToQuantified();
        }

        public static IQuantifiedTile<T> AsQuantified<T>(this ITile<T> l, double sizeX, double sizeY)
            where T : ICoordinate
        {
            return l as IQuantifiedTile<T> ?? l.ToQuantified(sizeX, sizeY);
        }

        public static IQuantifiedTile<T> AsQuantified<T>(this ITile<T> l, double sizeX, double sizeY, double stepX, double stepY)
           where T : ICoordinate
        {
            return l as IQuantifiedTile<T> ?? l.ToQuantified(sizeX, sizeY, stepX, stepY);
        }

        public static IQuantifiedTile<T> AsQuantified<T>(this ITile<T> l, double sizeX, double sizeY, double stepX, double stepY, double refOffsetX, double refOffsetY)
           where T : ICoordinate
        {
            return l as IQuantifiedTile<T> ?? l.ToQuantified(sizeX, sizeY, stepX, stepY, refOffsetX, refOffsetY);
        }

    }
}
