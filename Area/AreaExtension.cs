using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList
{
    public static class AreaExtension
    {
        public static T RefreshArea<T>(this T list)
           where T : ITile
        {
            list.UpdateArea();
            return list;
        }

        public static Area GetArea<T>(this IEnumerable<T> list)
            where T : ICoordinate
        {
            var area = new Area(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);

            foreach (T item in list)
            {
                if (item.X < area.Min.X)
                {
                    area.Min.X = item.X;
                }

                if (item.X > area.Max.X)
                {
                    area.Max.X = item.X;
                }

                if (item.Y < area.Min.Y)
                {
                    area.Min.Y = item.Y;
                }

                if (item.Y > area.Max.Y)
                {
                    area.Max.Y = item.Y;
                }
            }

            return area;
        }
    }
}
