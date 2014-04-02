using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList.Extensions
{
    public static class TileExtensions
    {
        public static ITile<T> ToTile<T>(this IEnumerable<T> c, int referenceIndex = 0)
          where T : ICoordinate
        {
            return new Tile<T>(c.GetArea(), c, referenceIndex);
        }

       
    }
}
