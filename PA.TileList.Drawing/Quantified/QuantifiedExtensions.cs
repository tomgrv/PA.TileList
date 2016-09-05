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
using PA.TileList.Circular;
using PA.TileList.Contextual;
using PA.TileList.Linear;
using PA.TileList.Quantified;
using PA.TileList.Drawing.Core;


namespace PA.TileList.Drawing.Quantified
{
    public static class QuantifiedExtensions
    {
        #region Dimension

        public static SizeF GetSize<T>(this IQuantifiedTile<T> c)
         where T : ICoordinate
        {
            return new SizeF((float)(c.ElementStepX * c.Zone.SizeX), (float)(c.ElementStepY * c.Zone.SizeY));
        }

        public static PointF GetOrigin<T>(this IQuantifiedTile<T> c, ScaleMode mode)
            where T : ICoordinate
        {
            float x = c.Zone.Min.X - (mode.HasFlag(ScaleMode.CENTER) ? c.Reference.X + 0.5f : c.Reference.X);
            float y = c.Zone.Min.Y - (mode.HasFlag(ScaleMode.CENTER) ? c.Reference.Y + 0.5f : c.Reference.Y);

            return new PointF((float)(x * c.ElementStepX), (float)(y * c.ElementStepY));
        }

        public static RectangleF GetBounds<T>(this IQuantifiedTile<T> c, ScaleMode mode = ScaleMode.ALL)
            where T : ICoordinate
        {
            PointF o = c.GetOrigin(mode);
            SizeF s = c.GetSize();

            return new RectangleF(o, s);
        }



        #endregion

        #region BaseImage

        private static RectangleD<U> GetBaseImage<T, U>(this IQuantifiedTile<T> c, Size s, ScaleMode mode)
            where T : ICoordinate
            where U : Image
        {
            return new RectangleD<U>(new Bitmap(s.Width, s.Height) as U, c.GetOrigin(mode), c.GetSize());
        }

        private static RectangleD<U> GetBaseImage<T, U>(this IQuantifiedTile<T> c, int Width, int Height, ScaleMode mode)
            where T : ICoordinate
            where U : Image
        {
            return new RectangleD<U>(new Bitmap(Width, Height) as U, c.GetOrigin(mode), c.GetSize());
        }

        #endregion

        #region Image

        public static RectangleD<U> GetImage<T, U>(this IQuantifiedTile<T> c, Size s, Func<T, SizeF, U> getImagePart)
            where T : ICoordinate
            where U : Image
        {
            return c.GetImage<T, U>(c.GetBaseImage<T, U>(s, ScaleMode.ALL), ScaleMode.ALL, getImagePart);
        }

        public static RectangleD<U> GetImage<T, U>(this IQuantifiedTile<T> c, int Width, int Height, Func<T, SizeF, U> getImagePart)
            where T : ICoordinate
            where U : Image
        {
            return c.GetImage<T, U>(c.GetBaseImage<T, U>(Width, Height, ScaleMode.ALL), ScaleMode.ALL, getImagePart);
        }

        public static RectangleD<U> GetImage<T, U>(this IQuantifiedTile<T> c, Size s, ScaleMode mode, Func<T, SizeF, U> getImagePart)
            where T : ICoordinate
            where U : Image
        {
            return c.GetImage<T, U>(c.GetBaseImage<T, U>(s, mode), mode, getImagePart);
        }

        public static RectangleD<U> GetImage<T, U>(this IQuantifiedTile<T> c, RectangleD<U> image, Func<T, SizeF, U> getImagePart)
            where T : ICoordinate
            where U : Image
        {
            return c.GetImage<T, U>(image, ScaleMode.ALL, getImagePart);
        }

        public static RectangleD<U> GetImage<T, U>(this IQuantifiedTile<T> c, RectangleD<U> image, ScaleMode mode, Func<T, SizeF, U> getImagePortion)
            where T : ICoordinate
            where U : Image
        {
            using (GraphicsD g = image.GetGraphicsD(mode))
            {
                foreach (RectangleD<T> portion in c.GetPortions(g, mode)
                    .Where(p => p.Outer.Height >= 1f && p.Outer.Width >= 1f))
                {
                    using (U partial = getImagePortion(portion.Item, portion.Inner.Size))
                    {
                        if (partial != null)
                        {
                            g.Graphics.DrawImage(partial, portion.Inner);
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

            float refX = mode.HasFlag(ScaleMode.CENTER) ? tile.Reference.X + 0.5f : tile.Reference.X;
            float refY = mode.HasFlag(ScaleMode.CENTER) ? tile.Reference.Y + 0.5f : tile.Reference.Y;

            float offX = (stepX - sizeX) / 2f;
            float offY = (stepY - sizeY) / 2f;

            foreach (T e in tile)
            {
                var portionO = new RectangleF((e.X - refX) * stepX + offsetX, (e.Y - refY) * stepY + offsetY, stepX, stepY);
                var portionI = new RectangleF(portionO.X + offX, portionO.Y + offY, sizeX, sizeY);
                yield return new RectangleD<T>(e, portionO, portionI);
            }
        }

        #endregion


    }


}
