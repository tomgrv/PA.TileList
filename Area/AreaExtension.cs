using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList
{
    public static class AreaExtension
    {
        public static ICoordinate Center<T>(this T a)
            where T : IArea
        {
            return new Coordinate((int)(a.SizeX / 2f + a.Min.X), (int)(a.SizeY / 2f + a.Min.Y));
        }

        public static bool Contains<T>(this T a, ICoordinate c)
             where T : IArea
        {
            return Contains(a, c.X, c.Y);
        }

        public static bool Contains<T>(this T a, int x, int y)
             where T : IArea
        {
            return a.Min.X <= x && x <= a.Max.X && a.Min.Y <= y && y <= a.Max.Y;
        }

        public static bool Contains<T>(this T a, IArea b)
             where T : IArea
        {
            return a.Min.X <= b.Min.X && b.Max.X <= a.Max.X && a.Min.Y <= b.Min.Y && b.Max.Y <= a.Max.Y;
        }

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
