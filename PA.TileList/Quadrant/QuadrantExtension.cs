using System;
using System.Collections.Generic;
using System.Linq;
using PA.TileList.Contextual;
using PA.TileList.Cropping;
using PA.TileList.Linear;
using PA.TileList.Quantified;
using PA.TileList.Tile;

namespace PA.TileList.Quadrant
{
    public static class QuadrantExtension
    {
        #region ToTopLeftPositive
        [Obsolete]
        public static void ToTopLeftPositive<T>(this IQuadrant<T> zl, IZone a, ref int x, ref int y)
                    where T : class, ICoordinate
        {
            switch (zl.Quadrant)
            {
                case Quadrant.TopRight:
                    x = -(x - a.Min.X - a.SizeX + 1);
                    y = y - a.Min.Y;
                    break;
                case Quadrant.TopLeft:
                    x = x - a.Min.X;
                    y = y - a.Min.Y;
                    break;
                case Quadrant.BottomLeft:
                    x = x - a.Min.X;
                    y = -(y - a.Min.Y - a.SizeY + 1);
                    break;
                case Quadrant.BottomRight:
                    x = -(x - a.Min.X - a.SizeX + 1);
                    y = -(y - a.Min.Y - a.SizeY + 1);
                    break;
            }
        }

        #endregion

        #region FirstOrDefault
        [Obsolete]
        public static T FirstOrDefault<R, T>(this IQuadrant<T> zl, int x, int y, bool flattenQuadrant = false)
                    where T : class, ICoordinate
        {
            IZone a = new Zone(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);

            if (flattenQuadrant)
            {
                a = zl.GetZone();
                zl.ToTopLeftPositive(a, ref x, ref y);
            }

            return zl.FirstOrDefault(delegate (T item)
            {
                var ix = item.X;
                var iy = item.Y;

                if (flattenQuadrant)
                    zl.ToTopLeftPositive(a, ref ix, ref iy);

                return (ix == x) && (iy == y);
            });
        }

        #endregion

        public static string ToString<R, T>(this R zl)
            where R : IQuadrant<T>
            where T : ICoordinate
        {
            return zl.X + "," + zl.Y + " - " + zl.Quadrant;
        }

        #region ChangeQuadrant

        public static void SetQuadrant<T>(this IQuadrant<T> zl, Quadrant target)
            where T : class, ICoordinate
        {
            var a = zl.GetZone();

            foreach (var item in zl)
                item.SetQuadrant(a, zl.Quadrant, target);
        }

        public static void SetQuadrant<T>(this T item, IZone a, Quadrant source, Quadrant target)
            where T : class, ICoordinate
        {
            item.ChangeQuadrant(a, source, target).DeContextualize();
        }

        public static void SetQuadrant<T>(this T item, IQuadrant<T> source, Quadrant target)
            where T : class, ICoordinate
        {
            item.ChangeQuadrant(source.Zone, source.Quadrant, target).DeContextualize();
        }

        #region IEnumerable Change

        public static IEnumerable<IContextual<T>> ChangeQuadrant<T>(this IQuadrant<T> c,
            Quadrant target = Quadrant.Array)
            where T : class, ICoordinate
        {
            foreach (var e in c)
                yield return e.ChangeQuadrant(c.Zone, c.Quadrant, target);
        }

        public static IEnumerable<IContextual<T>> ChangeQuadrant<T>(this IEnumerable<T> c, IZone a, Quadrant source,
            Quadrant target = Quadrant.Array)
            where T : class, ICoordinate
        {
            foreach (var e in c)
                yield return e.ChangeQuadrant(a, source, target);
        }

        public static IEnumerable<IContextual<T>> ChangeQuadrant<T>(this IEnumerable<IContextual<T>> c, IZone a,
            Quadrant source, Quadrant target = Quadrant.Array)
            where T : class, ICoordinate
        {
            foreach (var e in c)
                yield return e.ChangeQuadrant(a, source, target);
        }

        #endregion

        #region ITile Change

        public static Tile<IContextual<T>> ChangeQuadrant<T>(this ITile<T> c, Quadrant source,
            Quadrant target = Quadrant.Array)
            where T : class, ICoordinate
        {
            return c.AsEnumerable()
                .ChangeQuadrant(c.Zone, source, target)
                .ToTile(c.IndexOf(c.Reference));
        }

        public static Tile<IContextual<T>> ChangeQuadrant<T>(this ITile<IContextual<T>> c, Quadrant source,
            Quadrant target = Quadrant.Array)
            where T : class, ICoordinate
        {
            return c.AsEnumerable()
                .ChangeQuadrant(c.Zone, source, target)
                .ToTile(c.IndexOf(c.Reference));
        }

        #endregion

        #region IQuantifiedTile Change

        public static QuantifiedTile<IContextual<T>> ChangeQuadrant<T>(this IQuantifiedTile<T> c, Quadrant source,
            Quadrant target = Quadrant.Array)
            where T : class, ICoordinate
        {
            var offsetX = c.RefOffsetX;
            var offsetY = c.RefOffsetY;

            switch (source)
            {
                case Quadrant.TopRight:
                    offsetX = -offsetX;
                    //offsetY =offsetY;
                    break;
                case Quadrant.TopLeft:
                    //offsetX = offsetX;
                    //offsetY =offsetY;
                    break;
                case Quadrant.BottomLeft:
                    //offsetX = offsetX;
                    offsetY = -offsetY;
                    break;
                case Quadrant.BottomRight:
                    offsetX = -offsetX;
                    offsetY = -offsetY;
                    break;
            }

            switch (target)
            {
                case Quadrant.TopRight:
                    offsetX = -offsetX;
                    //offsetY = offsetY ;
                    break;
                case Quadrant.TopLeft:
                    //offsetX = offsetX ;
                    //offsetY = offsetY ;
                    break;
                case Quadrant.BottomLeft:
                    //offsetX = offsetX;
                    offsetY = -offsetY;
                    break;
                case Quadrant.BottomRight:
                    offsetX = -offsetX;
                    offsetY = -offsetY;
                    break;
            }

            return c.AsTile()
                .ChangeQuadrant(source, target)
                .ToQuantified(c.ElementSizeX, c.ElementSizeY, c.ElementStepX, c.ElementStepY, offsetX, offsetY);
        }


        public static QuantifiedTile<IContextual<T>> ChangeQuadrant<T>(this IQuantifiedTile<IContextual<T>> c,
            Quadrant source, Quadrant target = Quadrant.Array)
            where T : class, ICoordinate
        {
            var offsetX = c.RefOffsetX;
            var offsetY = c.RefOffsetY;

            switch (source)
            {
                case Quadrant.TopRight:
                    offsetX = -offsetX;
                    //offsetY =offsetY;
                    break;
                case Quadrant.TopLeft:
                    //offsetX = offsetX;
                    //offsetY =offsetY;
                    break;
                case Quadrant.BottomLeft:
                    //offsetX = offsetX;
                    offsetY = -offsetY;
                    break;
                case Quadrant.BottomRight:
                    offsetX = -offsetX;
                    offsetY = -offsetY;
                    break;
            }

            switch (target)
            {
                case Quadrant.TopRight:
                    offsetX = -offsetX;
                    //offsetY = offsetY ;
                    break;
                case Quadrant.TopLeft:
                    //offsetX = offsetX ;
                    //offsetY = offsetY ;
                    break;
                case Quadrant.BottomLeft:
                    //offsetX = offsetX;
                    offsetY = -offsetY;
                    break;
                case Quadrant.BottomRight:
                    offsetX = -offsetX;
                    offsetY = -offsetY;
                    break;
            }

            return c.AsTile()
                .ChangeQuadrant(source, target)
                .ToQuantified(c.ElementSizeX, c.ElementSizeY, c.ElementStepX, c.ElementStepY, offsetX, offsetY);
        }

        #endregion

        #region ICoordinate Change 

        public static IContextual<T> ChangeQuadrant<T>(this T e, IZone a, Quadrant source,
            Quadrant target = Quadrant.Array)
            where T : class, ICoordinate
        {
            return Contextual<T>.FromContext(e).ChangeQuadrant(a, source, target);
        }

        public static IContextual<T> ChangeQuadrant<T>(this IContextual<T> e, IZone a, Quadrant source,
            Quadrant target = Quadrant.Array)
            where T : class, ICoordinate
        {
            var item = new Contextual<T>(e.X, e.Y, e.Context);

            // Source ==> Array
            switch (source)
            {
                case Quadrant.TopRight:
                    item.X = -item.X + a.Min.X + a.SizeX - 1;
                    item.Y = item.Y - a.Min.Y;
                    break;
                case Quadrant.TopLeft:
                    item.X = item.X - a.Min.X;
                    item.Y = item.Y - a.Min.Y;
                    break;
                case Quadrant.BottomLeft:
                    item.X = item.X - a.Min.X;
                    item.Y = -item.Y + a.Min.Y + a.SizeY - 1;
                    break;
                case Quadrant.BottomRight:
                    item.X = -item.X + a.Min.X + a.SizeX - 1;
                    item.Y = -item.Y + a.Min.Y + a.SizeY - 1;
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

            return item;
        }

        #endregion

        #endregion

        #region FirstOrAdd
        [Obsolete]
        public static T FirstOrAdd<T, L>(this IQuadrant<T> zl, int x, int y, Quadrant q, bool flattenQuadrant = false)
                    where T : class, ICoordinate, IQuadrant<L>
                    where L : class, ICoordinate
        {
            var unique = zl.FirstOrDefault<IQuadrant<T>, T>(x, y, flattenQuadrant);

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
        [Obsolete]
        public static T FirstOrAdd<T>(this IQuadrant<T> zl, int x, int y, bool flattenQuadrant = false)
                    where T : class, ICoordinate
        {
            var unique = zl.FirstOrDefault<IQuadrant<T>, T>(x, y, flattenQuadrant);

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
    }
}