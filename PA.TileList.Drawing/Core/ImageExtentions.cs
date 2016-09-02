using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using PA.TileList.Quantified;
using PA.TileList.Contextual;
using PA.TileList.Geometrics.Circular;
using System.Security.Cryptography;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text.RegularExpressions;

namespace PA.TileList.Drawing.Core
{
    public static class ImageExtentions
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

        public static GraphicsD GetGraphicsD<U>(this RectangleD<U> image, ScaleMode mode, Pen extraPen = null)
            where U : Image
        {
            float scaleX = (float)image.Item.Width / image.Outer.Width;
            float scaleY = (float)image.Item.Height / image.Outer.Height;

            if (mode.HasFlag(ScaleMode.NOSTRETCH))
            {
                float scale = Math.Min(scaleX, scaleY);
                scaleX = scale;
                scaleY = scale;
            }

            // Zone definition
            var outerZone = new RectangleF(image.Outer.X * scaleX, image.Outer.Y * scaleY, image.Outer.Width * scaleX, image.Outer.Height * scaleY);
            var innerZone = new RectangleF(image.Inner.X * scaleX, image.Inner.Y * scaleY, image.Inner.Width * scaleX, image.Inner.Height * scaleY);

            // Extract graphic
            Graphics g = Graphics.FromImage(image.Item);

            // Update
            g.TranslateTransform((image.Item.Width - image.Inner.Width * scaleX) / 2f, (image.Item.Height - image.Inner.Height * scaleY) / 2f);
            g.TranslateTransform(-outerZone.Left, -outerZone.Top);
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            // Extra trace
            if (extraPen != null)
            {
                g.DrawRectangle(extraPen, outerZone.Left + 1f, outerZone.Top + 1f, outerZone.Width - 1f, outerZone.Height - 1f);
                g.DrawRectangle(extraPen, innerZone.Left + 1f, innerZone.Top + 1f, innerZone.Width - 1f, innerZone.Height - 1f);

                g.DrawLine(extraPen, outerZone.Left, outerZone.Top, outerZone.Right, outerZone.Bottom);
                g.DrawLine(extraPen, outerZone.Right, outerZone.Top, outerZone.Left, outerZone.Bottom);

                g.DrawLine(extraPen, innerZone.Left, innerZone.Top, innerZone.Right, innerZone.Bottom);
                g.DrawLine(extraPen, innerZone.Right, innerZone.Top, innerZone.Left, innerZone.Bottom);
            }

            return new GraphicsD(g, scaleX, scaleY, innerZone, outerZone);
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

        public static byte[] GetRawData(this Image image)
        {
            var converter = new ImageConverter();
            return converter.ConvertTo(image, typeof(byte[])) as byte[];
        }

        public static string GetSignature(this Image image, string tag = null)
        {
            using (var sha = new MD5CryptoServiceProvider())
            {
                byte[] hash = sha.ComputeHash(image.GetRawData());
                string key = BitConverter.ToString(hash).Replace("-", String.Empty);

#if DEBUG
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();

                System.Diagnostics.StackFrame sf = st.GetFrames().FirstOrDefault(s => s.GetMethod().GetCustomAttributes(false)
                    .Any(i => i.ToString().EndsWith("TestAttribute")));

                var p = System.IO.Directory.GetCurrentDirectory();

                if (sf != null)
                {
                    string name = sf.GetMethod().Name + (tag != null ? "_" + tag : string.Empty);
                    image.Save(name + "_" + key + ".png", System.Drawing.Imaging.ImageFormat.Png);
                }
#endif

                return key;
            }
        }
    }


}
