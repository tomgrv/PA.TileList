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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using PA.TileList.Quantified;
using PA.TileList.Contextual;
using PA.TileList.Circular;
using PA.TileList.Linear;
using System.Security.Cryptography;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text.RegularExpressions;

namespace PA.TileList.Drawing.Graphics2D
{
    public static class GraphicExtensions
    {

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
            var g = Graphics.FromImage(image.Item);

            // Update
            g.TranslateTransform((image.Item.Width - image.Inner.Width * scaleX) / 2f, (image.Item.Height - image.Inner.Height * scaleY) / 2f);
            g.TranslateTransform(-outerZone.Left, -outerZone.Top);

            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
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
