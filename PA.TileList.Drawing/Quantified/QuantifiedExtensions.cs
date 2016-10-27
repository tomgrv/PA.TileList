using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PA.TileList.Drawing.Graphics2D;
using PA.TileList.Linear;
using PA.TileList.Quantified;
using PA.TileList.Selection;

namespace PA.TileList.Drawing.Quantified
{
    public static class QuantifiedExtensions
    {
        #region Portion

        public static IEnumerable<RectangleD<T>> GetPortions<T>(this IQuantifiedTile<T> tile, GraphicsD g, ScaleMode mode)
            where T : ICoordinate
        {
            var sizeX = (float)tile.ElementSizeX * g.ScaleX;
            var sizeY = (float)tile.ElementSizeY * g.ScaleY;

            var stepX = (float)tile.ElementStepX * g.ScaleX;
            var stepY = (float)tile.ElementStepY * g.ScaleY;

            var offsetX = (float)tile.RefOffsetX * g.ScaleX + g.OffsetX;
            var offsetY = (float)tile.RefOffsetY * g.ScaleY + g.OffsetY;

            var refX = tile.Reference.X + 0.5f;
            var refY = tile.Reference.Y + 0.5f;

            var offX = (stepX - sizeX) / 2f;
            var offY = (stepY - sizeY) / 2f;

            foreach (var e in tile)
            {
                var portionOuter = new RectangleF((e.X - refX) * stepX + offsetX, (e.Y - refY) * stepY + offsetY, stepX, stepY);
                var portionInner = new RectangleF(portionOuter.X + offX, portionOuter.Y + offY, sizeX, sizeY);
                yield return new RectangleD<T>(e, portionOuter, portionInner, mode);
            }
        }

        #endregion

        #region AddOns

        /// <summary>
        ///     Gets the coordinates within specified RectangleF
        /// </summary>
        /// <returns>The coordinates in.</returns>
        /// <param name="list">List.</param>
        /// <param name="inner">Inner.</param>
        /// <param name="strict">If set to <c>true</c> strict.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static IEnumerable<ICoordinate> GetCoordinatesIn<T>(this IQuantifiedTile<T> list, RectangleF inner,
            bool strict = false)
            where T : ICoordinate
        {
            var sc =
                new SelectionConfiguration(strict
                    ? SelectionPosition.Inside
                    : SelectionPosition.Inside | SelectionPosition.Under);

            return list.SelectCoordinates(new RectangularProfile(inner.Left, inner.Top, inner.Right, inner.Bottom), sc,
                true);
        }

        #endregion

        #region Dimension



        public static SizeF GetSize<T>(this IQuantifiedTile<T> c)
            where T : ICoordinate
        {
            var x = c.ElementStepX * c.Zone.SizeX;
            var y = c.ElementStepY * c.Zone.SizeY;
            return new SizeF((float)x, (float)y);
        }

        public static PointF GetOrigin<T>(this IQuantifiedTile<T> c)
            where T : ICoordinate
        {

            var x = (c.Zone.Min.X - c.Reference.X - 0.5f) * c.ElementStepX + c.RefOffsetX;
            var y = (c.Zone.Min.Y - c.Reference.Y - 0.5f) * c.ElementStepY + c.RefOffsetY;
            return new PointF((float)x, (float)y);
        }

        public static RectangleF GetBounds<T>(this IQuantifiedTile<T> c)
            where T : ICoordinate
        {
            var o = c.GetOrigin();
            var s = c.GetSize();
            return new RectangleF(o, s);
        }

        #endregion

    }
}