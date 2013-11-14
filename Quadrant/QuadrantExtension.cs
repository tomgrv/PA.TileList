using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList
{
    public static class QuadrantExtension
    {
        #region ChangeQuadrant

        public static void SetCoordinatesInQuadrant<T>(this IQuadrant<T> zl, Quadrant target)
            where T : ICoordinate
        {
            IArea a = zl.GetArea();

            foreach (T item in zl)
            {
                zl.SetCoordinatesInQuadrant(target, item, a);
            }
        }

        public static void SetCoordinatesInQuadrant<T>(this IQuadrant<T> zl, Quadrant target, ICoordinate item)
          where T : ICoordinate
        {
            zl.SetCoordinatesInQuadrant<T>(target, item, zl.GetArea());
        }

        internal static void SetCoordinatesInQuadrant<T>(this IQuadrant<T> zl, Quadrant target, ICoordinate item, IArea a)
           where T : ICoordinate
        {
            // Source ==> Array
            switch (zl.Quadrant)
            {
                case Quadrant.TopRight:
                    item.X = -(item.X - a.Min.X - a.SizeX + 1);
                    item.Y = (item.Y - a.Min.Y);
                    break;
                case Quadrant.TopLeft:
                    item.X = (item.X - a.Min.X);
                    item.Y = (item.Y - a.Min.Y);
                    break;
                case Quadrant.BottomLeft:
                    item.X = (item.X - a.Min.X);
                    item.Y = -(item.Y - a.Min.Y - a.SizeY + 1);
                    break;
                case Quadrant.BottomRight:
                    item.X = -(item.X - a.Min.X - a.SizeX + 1);
                    item.Y = -(item.Y - a.Min.Y - a.SizeY + 1);
                    break;
            }

            // Array ==> Target
            switch (target)
            {
                case Quadrant.TopRight:
                    item.X = -item.X + a.Min.X + a.SizeX - 1;
                    item.Y = item.Y + a.Min.Y;
                    break;
                case Quadrant.TopLeft:
                    item.X = item.X + a.Min.X;
                    item.Y = item.Y + a.Min.Y;
                    break;
                case Quadrant.BottomLeft:
                    item.X = item.X + a.Min.X;
                    item.Y = -item.Y + a.Min.Y + a.SizeY - 1;
                    break;
                case Quadrant.BottomRight:
                    item.X = -item.X + a.Min.X + a.SizeX - 1;
                    item.Y = -item.Y + a.Min.Y + a.SizeY - 1;
                    break;
            }
        }


        public static decimal ChangeQuadrantFromOriginX(Quadrant source, decimal x, Quadrant target)
        {
            decimal xx = 0;

            // Source ==> Array
            switch (source)
            {
                case Quadrant.TopRight:
                    xx = -x;
                    //y = y;
                    break;
                case Quadrant.TopLeft:
                    xx = x;
                    //y = y;
                    break;
                case Quadrant.BottomLeft:
                    xx = x;
                    // y = -y;
                    break;
                case Quadrant.BottomRight:
                    xx = -x;
                    //y = -y;
                    break;
            }

            // Array ==> Target
            switch (target)
            {
                case Quadrant.TopRight:
                    xx = -x;
                    //y = y;
                    break;
                case Quadrant.TopLeft:
                    xx = x;
                    //y = y;
                    break;
                case Quadrant.BottomLeft:
                    xx = x;
                    // y = -y;
                    break;
                case Quadrant.BottomRight:
                    xx = -x;
                    //y = -y;
                    break;
            }

            return xx;
        }

        public static decimal ChangeQuadrantFromOriginY(Quadrant source, decimal y, Quadrant target)
        {
            decimal yy = 0;

            // Source ==> Array
            switch (source)
            {
                case Quadrant.TopRight:
                    //xx = -x;
                    yy = y;
                    break;
                case Quadrant.TopLeft:
                    // xx = x;
                    yy = y;
                    break;
                case Quadrant.BottomLeft:
                    //xx = x;
                    yy = -y;
                    break;
                case Quadrant.BottomRight:
                    // xx = -x;
                    yy = -y;
                    break;
            }

            // Array ==> Target
            switch (target)
            {
                case Quadrant.TopRight:
                    //xx = -x;
                    yy = y;
                    break;
                case Quadrant.TopLeft:
                    // xx = x;
                    yy = y;
                    break;
                case Quadrant.BottomLeft:
                    //xx = x;
                    yy = -y;
                    break;
                case Quadrant.BottomRight:
                    // xx = -x;
                    yy = -y;
                    break;
            }

            return yy;
        }



        #endregion

        #region ToTopLeftPositive

        [Obsolete]
        public static void ToTopLeftPositive<T>(this IQuadrant<T> zl, IArea a, ref int x, ref int y, Quadrant q) where T : ICoordinate
        {
            switch (q)
            {
                case Quadrant.TopRight:
                    x = -(x - a.Min.X - a.SizeX + 1);
                    y = (y - a.Min.Y);
                    break;
                case Quadrant.TopLeft:
                    x = (x - a.Min.X);
                    y = (y - a.Min.Y);
                    break;
                case Quadrant.BottomLeft:
                    x = (x - a.Min.X);
                    y = -(y - a.Min.Y - a.SizeY + 1);
                    break;
                case Quadrant.BottomRight:
                    x = -(x - a.Min.X - a.SizeX + 1);
                    y = -(y - a.Min.Y - a.SizeY + 1);
                    break;
            }
        }

        public static void ToTopLeftPositive<T>(this IQuadrant<T> zl, IArea a, ref int x, ref int y) where T : ICoordinate
        {
            switch (zl.Quadrant)
            {
                case Quadrant.TopRight:
                    x = -(x - a.Min.X - a.SizeX + 1);
                    y = (y - a.Min.Y);
                    break;
                case Quadrant.TopLeft:
                    x = (x - a.Min.X);
                    y = (y - a.Min.Y);
                    break;
                case Quadrant.BottomLeft:
                    x = (x - a.Min.X);
                    y = -(y - a.Min.Y - a.SizeY + 1);
                    break;
                case Quadrant.BottomRight:
                    x = -(x - a.Min.X - a.SizeX + 1);
                    y = -(y - a.Min.Y - a.SizeY + 1);
                    break;
            }
        }

        public static void ToTopLeftPositive<T>(this IQuadrant<T> zl, IArea a, ref T item) where T : ICoordinate
        {
            switch (zl.Quadrant)
            {
                case Quadrant.TopRight:
                    item.X = -(item.X - a.Min.X - a.SizeX + 1);
                    item.Y = (item.Y - a.Min.Y);
                    break;
                case Quadrant.TopLeft:
                    item.X = (item.X - a.Min.X);
                    item.Y = (item.Y - a.Min.Y);
                    break;
                case Quadrant.BottomLeft:
                    item.X = (item.X - a.Min.X);
                    item.Y = -(item.Y - a.Min.Y - a.SizeY + 1);
                    break;
                case Quadrant.BottomRight:
                    item.X = -(item.X - a.Min.X - a.SizeX + 1);
                    item.Y = -(item.Y - a.Min.Y - a.SizeY + 1);
                    break;
            }
        }

        #endregion

        #region FromTopLeftPositive

        public static void FromTopLeftPositive<T>(this IQuadrant<T> zl, ref int x, ref int y) where T : ICoordinate
        {
            IArea a = zl.GetArea();

            switch (zl.Quadrant)
            {
                case Quadrant.TopRight:
                    x = -x + a.Min.X + a.SizeX - 1;
                    y = y + a.Min.Y;
                    break;
                case Quadrant.TopLeft:
                    x = x + a.Min.X;
                    y = y + a.Min.Y;
                    break;
                case Quadrant.BottomLeft:
                    x = x + a.Min.X;
                    y = -y + a.Min.Y + a.SizeY - 1;
                    break;
                case Quadrant.BottomRight:
                    x = -x + a.Min.X + a.SizeX - 1;
                    y = -y + a.Min.Y + a.SizeY - 1;
                    break;
            }
        }

        #endregion

        #region FirstOrDefault


        public static T FirstOrDefault<R, T>(this IQuadrant<T> zl, int x, int y, bool flattenQuadrant = false)
            where T : ICoordinate
        {
            IArea a = new Area(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);

            if (flattenQuadrant)
            {
                a = zl.GetArea();
                zl.ToTopLeftPositive(a, ref x, ref y);
            }

            return zl.FirstOrDefault(delegate(T item)
            {
                int ix = item.X;
                int iy = item.Y;

                if (flattenQuadrant)
                {
                    zl.ToTopLeftPositive(a, ref ix, ref iy);
                }

                return (ix == x && iy == y);
            });
        }


        #endregion

        #region FirstOrAdd

        public static T FirstOrAdd<T, L>(this IQuadrant<T> zl, int x, int y, Quadrant q, bool flattenQuadrant = false)
            where T : class,ICoordinate, IQuadrant<L>
            where L : class,ICoordinate
        {
            T unique = zl.FirstOrDefault<IQuadrant<T>, T>(x, y, flattenQuadrant);

            if (unique == null)
            {
                unique = Activator.CreateInstance<T>();
                unique.X = x;
                unique.Y = y;
                unique.SetQuadrant(q);
                zl.Add(unique);
            }

            return unique;
        }

        public static T FirstOrAdd<T>(this IQuadrant<T> zl, int x, int y, bool flattenQuadrant = false)
            where T : class,ICoordinate
        {
            T unique = zl.FirstOrDefault<IQuadrant<T>, T>(x, y, flattenQuadrant);

            if (unique == null)
            {
                unique = Activator.CreateInstance<T>();
                unique.X = x;
                unique.Y = y;
                zl.Add(unique);
            }

            return unique;
        }

        #endregion

        #region Fill

        public static void Fill<T, U>(this IQuadrant<T> zl, ushort SizeX, ushort SizeY, U motif, Quadrant q = Quadrant.Array, double ShiftX = 0, double ShiftY = 0)
            where T : class,ICoordinate
            where U : T, ICloneable
        {

            int StartX;
            int StartY;

            switch (q)
            {
                case Quadrant.Array:
                    StartX = 0;
                    StartY = 0;
                    break;

                case Quadrant.TopRight:
                    StartX = Convert.ToInt32(ShiftX - SizeX / 2f - 0.1);
                    StartY = Convert.ToInt32(ShiftY - SizeY / 2f - 0.1);
                    throw new NotImplementedException("Quadrant not VERIFIED");

                case Quadrant.TopLeft:
                    StartX = Convert.ToInt32(ShiftX - SizeX / 2f - 0.1);
                    StartY = Convert.ToInt32(ShiftY - SizeY / 2f + 0.1);
                    throw new NotImplementedException("Quadrant not VERIFIED");

                case Quadrant.BottomLeft:
                    StartX = Convert.ToInt32(ShiftX - SizeX / 2f + 0.1);
                    StartY = Convert.ToInt32(ShiftY - SizeY / 2f + 0.1);
                    break;

                case Quadrant.BottomRight:
                    StartX = Convert.ToInt32(ShiftX - SizeX / 2f + 0.1);
                    StartY = Convert.ToInt32(ShiftY - SizeY / 2f - 0.1);
                    throw new NotImplementedException("Quadrant not VERIFIED");

                default:
                    throw new NotSupportedException("Quadrant not supported");
            }

            for (int i = StartX; i < (StartX + SizeX); i++)
            {
                for (int j = StartY; j < (StartY + SizeY); j++)
                {
                    T clone = (T)motif.Clone();

                    T item = zl.FirstOrDefault<IQuadrant<T>, T>(i, j);
                    if (item is T)
                    {
                        zl.Remove(item);
                    }

                    clone.X = i;
                    clone.Y = j;

                    zl.Add(clone);
                }
            }
        }

        public static void Fill<T, U>(this IQuadrant<T> zl, IArea a, U motif, Quadrant q)
            where T : class,ICoordinate
            where U : T, ICloneable
        {
            for (int i = a.Min.X; i <= a.Max.X; i++)
            {
                for (int j = a.Min.Y; j <= a.Max.Y; j++)
                {

                    T item = zl.FirstOrDefault<IQuadrant<T>, T>(i, j);
                    if (item is T)
                    {
                        zl.Remove(item);
                    }

                    T clone = (T)motif.Clone();
                    clone.X = i;
                    clone.Y = j;

                    zl.Add(clone);
                }
            }
        }

        #endregion

        public static void Add<R, T>(this R zl, T item, IArea a = null)
            where R : IQuadrant<T>
            where T : ICoordinate
        {
            if (a == null) { a = zl.GetArea(); }

            zl.SetCoordinatesInQuadrant(zl.Quadrant, item, a);
            zl.Add(item);
        }

        public static T Contextualize<R, T>(this R zl, T item, IArea a = null)
            where R : IQuadrant<T>, ICoordinate
            where T : ICoordinate, ICloneable
        {
            if (a == null) { a = zl.GetArea(); }

            T clone = (T)item.Clone();

            zl.SetCoordinatesInQuadrant<T>(zl.Quadrant, clone, a);

            int OffsetX;
            int OffsetY;

            switch (zl.Quadrant)
            {
                case Quadrant.Array:
                case Quadrant.TopLeft:
                    OffsetX = zl.X * a.SizeX;
                    OffsetY = -zl.Y * a.SizeY;
                    break;
                case Quadrant.TopRight:
                    OffsetX = -zl.X * a.SizeX;
                    OffsetY = -zl.Y * a.SizeY;
                    break;

                case Quadrant.BottomLeft:
                    OffsetX = zl.X * a.SizeX;
                    OffsetY = zl.Y * a.SizeY;
                    break;
                case Quadrant.BottomRight:
                    OffsetX = -zl.X * a.SizeX;
                    OffsetY = zl.Y * a.SizeY;
                    break;
                default:
                    throw new NotSupportedException("Quadrant not supported");
            }

            switch (zl.Quadrant)
            {
                case Quadrant.Array:
                case Quadrant.TopLeft:
                    clone.X += OffsetX;
                    clone.Y -= OffsetY;
                    break;
                case Quadrant.TopRight:
                    clone.X -= OffsetX;
                    clone.Y -= OffsetY;
                    break;

                case Quadrant.BottomLeft:
                    clone.X += OffsetX;
                    clone.Y += OffsetY;
                    break;
                case Quadrant.BottomRight:
                    clone.X -= OffsetX;
                    clone.Y += OffsetY;
                    break;
                default:
                    throw new NotSupportedException("Quadrant not supported");
            }
            return clone;
        }

        public static T[,] ToArray<R, T>(this R zl)
            where R : IQuadrant<T>
            where T : ICoordinate
        {
            IArea a = zl.GetArea();

            T[,] array = new T[a.SizeX, a.SizeY];

            foreach (T item in zl)
            {
                int x = item.X;
                int y = item.Y;

                zl.ToTopLeftPositive(a, ref x, ref y, zl.Quadrant);

                try
                {
                    array[x, y] = item;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error in Tile<T>.ToArray(): " + e.Message);
                }
            }

            return array;
        }

        public static string ToString<R, T>(this R zl)
            where R : IQuadrant<T>, ICoordinate
            where T : ICoordinate
        {
            return zl.X + "," + zl.Y + " ; " + zl.Quadrant;
        }
    }
}
