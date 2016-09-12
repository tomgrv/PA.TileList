using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using PA.TileList.Quantified;
using PA.TileList.Contextual;
using PA.TileList.Linear;
using PA.TileList.Quantified;
using PA.TileList.Drawing.Graphics2D;


namespace PA.TileList.Drawing.Quantified
{
    public static class QuantifiedExtensions
    {
        #region Dimension

        public static SizeF GetSize<T>(this IQuantifiedTile<T> c, ScaleMode mode = ScaleMode.NONE)
         where T : ICoordinate
        {
            return new SizeF((float)(c.ElementStepX * c.Zone.SizeX), (float)(c.ElementStepY * c.Zone.SizeY));
        }

        public static PointF GetOrigin<T>(this IQuantifiedTile<T> c, ScaleMode mode)
            where T : ICoordinate
        {
            // var o = mode.HasFlag(ScaleMode.CENTER) ? 0.5f : 0;

            var x = c.Zone.Min.X - c.Reference.X - 0.5f;
            var y = c.Zone.Min.Y - c.Reference.Y - 0.5f;

            return new PointF((float)(x * c.ElementStepX), (float)(y * c.ElementStepY));
        }

        public static RectangleF GetBounds<T>(this IQuantifiedTile<T> c, ScaleMode mode = ScaleMode.NONE)
            where T : ICoordinate
        {
            PointF o = c.GetOrigin(mode);
            SizeF s = c.GetSize(mode);

            return new RectangleF(o, s);
        }

        #endregion

        #region BaseImage

        private static RectangleD<U> GetBaseImage<T, U>(this IQuantifiedTile<T> c, Size s, ScaleMode mode)
            where T : ICoordinate
            where U : Image
        {
            return new RectangleD<U>(new Bitmap(s.Width, s.Height) as U, c.GetOrigin(mode), c.GetSize(mode), mode);
        }

        private static RectangleD<U> GetBaseImage<T, U>(this IQuantifiedTile<T> c, int Width, int Height, ScaleMode mode)
            where T : ICoordinate
            where U : Image
        {
            return new RectangleD<U>(new Bitmap(Width, Height) as U, c.GetOrigin(mode), c.GetSize(mode), mode);
        }

        #endregion

        #region Image

        public static RectangleD<U> GetImage<T, U>(this IQuantifiedTile<T> c, Size s, Func<T, SizeF, U> getImagePart, Pen extraPen = null)
            where T : ICoordinate
            where U : Image
        {
            return c.GetImage<T, U>(c.GetBaseImage<T, U>(s, ScaleMode.NONE), getImagePart, extraPen);
        }

        public static RectangleD<U> GetImage<T, U>(this IQuantifiedTile<T> c, Size s, ScaleMode mode, Func<T, SizeF, U> getImagePart, Pen extraPen = null)
           where T : ICoordinate
           where U : Image
        {
            return c.GetImage<T, U>(c.GetBaseImage<T, U>(s, mode), getImagePart, extraPen);
        }

        public static RectangleD<U> GetImage<T, U>(this IQuantifiedTile<T> c, int Width, int Height, Func<T, SizeF, U> getImagePart, Pen extraPen = null)
            where T : ICoordinate
            where U : Image
        {
            return c.GetImage<T, U>(c.GetBaseImage<T, U>(Width, Height, ScaleMode.NONE), getImagePart,  extraPen);
        }

        public static RectangleD<U> GetImage<T, U>(this IQuantifiedTile<T> c, int Width, int Height, ScaleMode mode, Func<T, SizeF, U> getImagePart, Pen extraPen = null)
           where T : ICoordinate
           where U : Image
        {
            return c.GetImage<T, U>(c.GetBaseImage<T, U>(Width, Height, mode), getImagePart, extraPen);
        }

        public static RectangleD<U> GetImage<T, U>(this IQuantifiedTile<T> c, RectangleD<U> image, Func<T, SizeF, U> getImagePortion, Pen extraPen  = null)
            where T : ICoordinate
            where U : Image
        {
            using (GraphicsD g = image.GetGraphicsD(extraPen))
            {
                foreach (RectangleD<T> portion in c.GetPortions(g, image.Mode)
                    .Where(p => p.Outer.Height >= 1f && p.Outer.Width >= 1f))
                {
                    using (U partial = getImagePortion(portion.Item, portion.Inner.Size))
                    {
                        if (partial != null)
                        {
                            g.Graphics.DrawImage(partial, portion.Inner);

                            if (extraPen != null)
                            {
                                g.Graphics.DrawRectangle(extraPen, Rectangle.Round(portion.Outer));
                            }
                        }
                    }
                }
            }

            return image;
        }

        #endregion

        #region Portion

        public static IEnumerable<RectangleD<T>> GetPortions<T>(this IQuantifiedTile<T> tile, GraphicsD g, ScaleMode mode)
            where T : ICoordinate
        {
            float sizeX = (float)tile.ElementSizeX * g.ScaleX;
            float sizeY = (float)tile.ElementSizeY * g.ScaleY;

            float stepX = (float)tile.ElementStepX * g.ScaleX;
            float stepY = (float)tile.ElementStepY * g.ScaleY;

            float offsetX = (float)tile.RefOffsetX * g.ScaleX + g.OffsetX;
            float offsetY = (float)tile.RefOffsetY * g.ScaleY + g.OffsetY;

            float refX = tile.Reference.X + 0.5f;
            float refY = tile.Reference.Y + 0.5f;

            float offX = (stepX - sizeX) / 2f;
            float offY = (stepY - sizeY) / 2f;

            foreach (T e in tile)
            {
                var portionOuter = new RectangleF((e.X - refX) * stepX + offsetX, (e.Y - refY) * stepY + offsetY, stepX, stepY);
                var portionInner = new RectangleF(portionOuter.X + offX, portionOuter.Y + offY, sizeX, sizeY);
                yield return new RectangleD<T>(e, portionOuter, portionInner, mode);
            }
        }

        #endregion

        #region AddOns

        /// <summary>
        /// Gets the coordinates within specified RectangleF
        /// </summary>
        /// <returns>The coordinates in.</returns>
        /// <param name="list">List.</param>
        /// <param name="inner">Inner.</param>
        /// <param name="strict">If set to <c>true</c> strict.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static IEnumerable<ICoordinate> GetCoordinatesIn<T>(this IQuantifiedTile<T> list, RectangleF inner, bool strict = false)
            where T : ICoordinate
        {
            return list.GetCoordinatesIn(inner.Left, inner.Top, inner.Right, inner.Bottom, strict);
        }

        #endregion
    }


}
