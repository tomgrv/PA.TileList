using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList.Extensions
{
    public static class TileExtensions
    {
        public static Tile<T> ToTile<T>(this IEnumerable<T> c, int referenceIndex = 0)
            where T : ICoordinate
        {
            return new Tile<T>(c, referenceIndex);
        }

        public static ITile<T> AsTile<T>(this IEnumerable<T> l, int referenceIndex = 0)
            where T : ICoordinate
        {
            return l as ITile<T> ?? l.ToTile(referenceIndex);
        }

        public static ITile<T> AsTile<T>(this IEnumerable<T> l, IArea a, int referenceIndex = 0)
            where T : ICoordinate
        {
            return l as ITile<T> ?? l.ToTile(referenceIndex);
        }
    }
}
