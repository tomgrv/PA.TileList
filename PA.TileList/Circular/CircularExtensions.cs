using System;
using System.Collections.Generic;
using System.Linq;
using PA.TileList.Extensions;
using PA.TileList.Linear;
using PA.TileList.Quantified;

namespace PA.TileList.Circular
{
    public static class CircularExtensions
    {
        public static IEnumerable<KeyValuePair<T, double>> Distance<T>(this IQuantifiedTile<T> tile,
            Func<T, bool> predicate = null)
            where T : ICoordinate
        {
            return
                tile.WhereOrDefault(predicate)
                    .Select(c => new KeyValuePair<T, double>(c, Math.Sqrt(tile.Distance2(c))));
        }

        internal static double Distance2<T>(this IQuantifiedTile<T> tile, T c)
            where T : ICoordinate
        {
            var testX = (c.X - tile.Reference.X) * tile.ElementStepX + tile.RefOffsetX;
            var testY = (c.Y - tile.Reference.Y) * tile.ElementStepY + tile.RefOffsetY;
            return Math.Pow(testX, 2d) + Math.Pow(testY, 2d);
        }
    }
}