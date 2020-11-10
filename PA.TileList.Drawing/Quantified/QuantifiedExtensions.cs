using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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

        /// <summary>
        /// Gets the portions according to T dimensions
        /// </summary>
        /// <returns>The portions.</returns>
        /// <param name="tile">Tile.</param>
        /// <param name="g">The GraphicsD component representing drawing zone.</param>
        /// <param name="mode">Scale mode regarding specified drawing zone.</param>
        /// <typeparam name="T">IQuantifiedTile</typeparam>
        public static IEnumerable<RectangleD<T>> GetPortions<T>(this IQuantifiedTile<T> tile, GraphicsD g, ScaleMode mode, Func<T, bool> predicate = null)
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

            foreach (var e in (predicate is Func<T, bool> ? tile.Where(predicate) : tile))
            {
                var portionOuter = new RectangleF((e.X - refX) * stepX + offsetX, (e.Y - refY) * stepY + offsetY, stepX, stepY);
                var portionInner = new RectangleF(portionOuter.X + offX, portionOuter.Y + offY, sizeX, sizeY);
                yield return new RectangleD<T>(e, portionOuter, portionInner, mode);
            }
        }





        #endregion

        #region DrawPoints

        public static void DrawSelectionPoints<T, U>(this IQuantifiedTile<T> tile, ISelectionProfile profile,
                                       SelectionConfiguration config, RectangleD<Bitmap> i, Color selectedColor, Color notSelectedColor, bool drawPoints = true)
            where T : ICoordinate
            where U : Image
        {
            var g = i.GetGraphicsD();
            var o = new SizeF(g.OffsetX, g.OffsetY);
            var r = new RectangleF(g.Portion.Inner.Location + o, g.Portion.Inner.Size);

            var bs = new SolidBrush(selectedColor);
            var bn = new SolidBrush(notSelectedColor);

            // full mode / follows SelectionConfiguration
            profile.OptimizeProfile();

            foreach (var portions in tile.GetPortions(g, i.Mode))
            {
                var surface = portions.Item.Surface(tile, profile, config, g, bs, bn);

                g.Graphics.DrawRectangle(Pens.White, Rectangle.Round(config.UseFullSurface ? portions.Outer : portions.Inner));

#if DEBUG
                g.Graphics.DrawString(surface.ToString() + "%", new Font(FontFamily.GenericSansSerif, portions.Inner.Height / 4), portions.Item.Tag != null ? Brushes.Black : Brushes.Gray, portions.Inner.X, portions.Inner.Y);
#else
                g.Graphics.DrawString(surface.ToString() + "%", new Font(FontFamily.GenericSansSerif, portions.Inner.Height / 4), Brushes.Black , portions.Inner.X, portions.Inner.Y);
#endif 

            }
        }

        public static float Surface<T>(this T c, IQuantifiedTile tile, ISelectionProfile profile,
            SelectionConfiguration config, GraphicsD g, Brush selectedColor, Brush notSelectedColor, bool quickMode = true)
            where T : ICoordinate
        {
            Contract.Requires(tile != null, nameof(tile));
            Contract.Requires(profile != null, nameof(profile));
            Contract.Requires(config != null, nameof(config));

            if (quickMode)
            {
                var qsv = config.GetQuickSelectionVariant();
                var quick = c.CountPoints(tile, profile, qsv, g, selectedColor, notSelectedColor);

                if ((quick.Inside == 0 ^ quick.Outside == 0) && quick.Under == 0)
                {
                    return  100f * quick.CountSelected(config.SelectionType) / qsv.MaxSurface;
                }
            }

            var surface = c.CountPoints(tile, profile, config, g, selectedColor, notSelectedColor);
            return 100f * surface.CountSelected(config.SelectionType) / config.MaxSurface;
        }

        public static SelectionPoints CountPoints<T>(this T c, IQuantifiedTile tile, ISelectionProfile profile,
        SelectionConfiguration config, GraphicsD g, Brush selectedColor, Brush notSelectedColor)
        where T : ICoordinate
        {
            Contract.Requires(tile != null, nameof(tile));
            Contract.Requires(profile != null, nameof(profile));
            Contract.Requires(config != null, nameof(config));

            profile.OptimizeProfile();

            SelectionPoints p = new SelectionPoints();

            c.GetPoints(tile, config,
                                 (xc, yc) =>
                                 {
                                     var selected = false;

                                     switch (profile.Position(xc, yc, config))
                                     {
                                         case SelectionPosition.Inside:
                                             selected = config.SelectionType.HasFlag(SelectionPosition.Inside);
                                             p.Inside += 1;
                                             break;
                                         case SelectionPosition.Outside:
                                             selected = config.SelectionType.HasFlag(SelectionPosition.Outside);
                                             p.Outside += 1;
                                             break;
                                         case SelectionPosition.Under:
                                             selected = config.SelectionType.HasFlag(SelectionPosition.Under);
                                             p.Under += 1;
                                             break;
                                     }

                                     g.Graphics.FillRectangle(selected ? selectedColor : notSelectedColor, g.OffsetX + (float)xc[0] * g.ScaleX - 0.5f, g.OffsetY + (float)yc[0] * g.ScaleY - 1f, 2, 2);

                                 }, profile.GetValuesX, profile.GetValuesY);

            return p;
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
        public static IEnumerable<Coordinate> GetCoordinatesIn(this IQuantifiedTile list, RectangleF inner,
            bool strict = false)
        {
            var sc =
                new SelectionConfiguration(strict
                    ? SelectionPosition.Inside
                    : SelectionPosition.Inside | SelectionPosition.Under);

            return list.SelectCoordinates(new RectangularProfile(inner.Left, inner.Top, inner.Right, inner.Bottom), sc);
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