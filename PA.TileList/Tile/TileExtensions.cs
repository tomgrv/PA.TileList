using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PA.TileList.Linear;

namespace PA.TileList.Tile
{
    public static class TileExtensions
    {
        public static T ElementAt<T>(this IEnumerable<T> list, ICoordinate c)
        where T : class, ICoordinate
        {
            return list.First(e => e.X == c.X && e.Y == c.Y);
        }

        public static Tile<T> ToTile<T>(this IEnumerable<T> c, int referenceIndex = 0)
            where T : class, ICoordinate
        {
            return new Tile<T>(c, referenceIndex);
        }

        public static ITile<T> AsTile<T>(this IEnumerable<T> l, int referenceIndex = 0)
            where T : class, ICoordinate
        {
            return l as ITile<T> ?? l.ToTile(referenceIndex);
        }
    }
}
