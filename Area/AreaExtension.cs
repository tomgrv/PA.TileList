using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList
{
    public static class AreaExtension
    {
    
        public static Area GetArea<T>(this IEnumerable<T> list) where T : ICoordinate
        {
            Area a = new Area(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);

            foreach (T item in list)
            {
                if (item.X < a.Min.X)
                {
                    a.Min.X = item.X;
                }

                if (item.X > a.Max.X)
                {
                    a.Max.X = item.X;
                }

                if (item.Y < a.Min.Y)
                {
                    a.Min.Y = item.Y;
                }

                if (item.Y > a.Max.Y)
                {
                    a.Max.Y = item.Y;
                }
            }

            return a;
        }
    }
}
