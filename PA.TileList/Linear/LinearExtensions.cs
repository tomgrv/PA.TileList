using System;
using System.Collections.Generic;
using System.Linq;

namespace PA.TileList.Linear
{
    public static class LinearExtensions
    {
        public static IEnumerable<T> GetLine<T>(this IEnumerable<T> list, T p1, T p2, bool exact = true)
            where T : ICoordinate
        {
            // y = a*x+b
            var a = (p2.Y - p1.Y) / (double) (p2.X - p1.X);
            var b = -a * p1.X + p1.Y;

            if (exact)
                return list.Where(p => p.Y == a * p.X + b);
            return list.Where(p => p.Y == Math.Round(a * p.X + b, 0));
        }
    }
}