//
// GraphicExtensions.cs
//
// Author:
//       Thomas GERVAIS <thomas.gervais@gmail.com>
//
// Copyright (c) 2016 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace PA.TileList.Drawing.Graphics2D
{
    public static class GraphicExtensions
    {

        #region Renderer

        public static RectangleD<U> RenderImage<T, U>(this T c, U baseImage, ScaleMode mode, IRenderer<T, U> renderer)
           where U : Image
        {
            return renderer.Render(c, baseImage, mode);
        }

        public static RectangleD<U> RenderImage<T, U>(this T c, int width, int height, ScaleMode mode, IRenderer<T, U> renderer)
            where U : Image
        {
            return renderer.Render(c, width, height, mode);
        }

        public static RectangleD<U> RenderImage<T, U>(this T c, int width, int height, RectangleF inner, ScaleMode mode, IRenderer<T, U> renderer)
            where U : Image
        {
            return renderer.Render(c, width, height, inner, mode);
        }

        public static RectangleD<U> RenderImage<T, U>(this T c, RectangleD<U> image, IRenderer<T, U> renderer)
            where U : Image
        {
            return renderer.Render(c, image);
        }

        #endregion

        #region Renderer



        #endregion

        public static GraphicsD GetGraphicsD<U>(this RectangleD<U> image)
            where U : Image
        {
            var scaleX = image.Item.Width / image.Outer.Width;
            var scaleY = image.Item.Height / image.Outer.Height;

            if (image.Mode.HasFlag(ScaleMode.XYRATIO))
            {
                var scale = Math.Min(scaleX, scaleY);
                scaleX = scale;
                scaleY = scale;
            }

            // Zone definition
            var outerZone = new RectangleF(image.Outer.X * scaleX, image.Outer.Y * scaleY, image.Outer.Width * scaleX, image.Outer.Height * scaleY);
            var innerZone = new RectangleF(image.Inner.X * scaleX, image.Inner.Y * scaleY, image.Inner.Width * scaleX, image.Inner.Height * scaleY);

            // Extract graphic
            var g = Graphics.FromImage(image.Item);

            // Offset
            var offsetX = (image.Item.Width - image.Inner.Width * scaleX) / 2f - outerZone.Left;
            var offsetY = (image.Item.Height - image.Inner.Height * scaleY) / 2f - outerZone.Top;

            // Return Item
            return new GraphicsD(g, scaleX, scaleY, outerZone, innerZone, offsetX, offsetY);
        }


        public static byte[] GetRawData(this Image image)
        {
            var converter = new ImageConverter();
            return converter.ConvertTo(image, typeof(byte[])) as byte[];
        }

        public static string GetSignature(this Image image, string tag = null)
        {
            using (var sha = new MD5CryptoServiceProvider())
            {
                var hash = sha.ComputeHash(image.GetRawData());
                var key = BitConverter.ToString(hash).Replace("-", string.Empty);

#if DEBUG
                var st = new StackTrace();

                var sf =
                    st.GetFrames().FirstOrDefault(s => s.GetMethod().GetCustomAttributes(false)
                        .Any(i => i.ToString().EndsWith("TestAttribute")));

                var p = Directory.GetCurrentDirectory();

                if (sf != null)
                {
                    var name = sf.GetMethod().Name + (tag != null ? "_" + tag : string.Empty);
                    image.Save(Path.GetTempPath() + name + "_" + key + ".png", ImageFormat.Png);
                }
#endif

                return key;
            }
        }
    }
}