using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList.Extensions
{
    public static class ListExtensions
    {
        public static IEnumerable<T> WhereOrDefault<T>(this IEnumerable<T> c, Func<T, bool> predicate)        
        {
            return predicate is Func<T, bool> ? c.Where(predicate) : c.AsEnumerable();
        }
    }
}
