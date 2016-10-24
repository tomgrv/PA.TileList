//
// EmptyClass.cs
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
using PA.TileList.Drawing.Graphics2D;
using PA.TileList.Quantified;
using System.Drawing;
using PA.TileList.Circular;
using PA.TileList.Linear;
using System.Linq;
using PA.Utilities;

namespace PA.TileList.Drawing.Circular
{
    public class CircularProfileRenderer : IRenderer<CircularProfile, Bitmap>
    {
        private Pen _radiusPen;
        private Pen _arcPen;
        private Pen _extraPen;


        public CircularProfileRenderer(Pen radiusPen = null, Pen arcPen = null, Pen extraPen = null)
        {
            this._radiusPen = radiusPen;
            this._arcPen = arcPen;
            this._extraPen = extraPen;
        }

        public RectangleD<Bitmap> Render(CircularProfile obj, RectangleD<Bitmap> portion)
        {
            return this.Render(obj, new Bitmap(portion.Item), portion as RectangleD, portion.Mode);
        }

        public RectangleD<Bitmap> Render(CircularProfile obj, int width, int height, ScaleMode mode)
        {
            var m = 2 * obj.GetMaxRadius();

            var s = (mode == ScaleMode.STRETCH) ? new SizeF((float)m, (float)m) : new SizeF(width, height);
            var p = new PointF(-s.Width / 2f, -s.Height / 2f);
            var r = new RectangleD<Bitmap>(new Bitmap(width, height), p, s, mode);

            return Render(obj, r);

        }

        public RectangleD<Bitmap> Render(CircularProfile obj, int width, int height, RectangleF inner, ScaleMode mode)
        {
            return Render(obj, new RectangleD<Bitmap>(new Bitmap(width, height), inner, mode));
        }

        private RectangleD<Bitmap> Render(CircularProfile obj, Bitmap image, RectangleD portion, ScaleMode mode)
        {
            var rendered = new RectangleD<Bitmap>(image, portion, mode);

            using (var g = rendered.GetGraphicsD())
            {
                var maxsize = (float)obj.GetMaxRadius() * 2f;
                var minsize = (float)obj.GetMinRadius() * 2f;
                var midsize = (float)obj.Radius * 2f;

                if (_extraPen != null)
                {
                    g.Draw(_extraPen);

                    g.Graphics.DrawEllipse(_extraPen, g.OffsetX - maxsize * g.ScaleX / 2f, g.OffsetY - maxsize * g.ScaleY / 2f, maxsize * g.ScaleX, maxsize * g.ScaleY);
                    g.Graphics.DrawEllipse(_extraPen, g.OffsetX - minsize * g.ScaleX / 2f, g.OffsetY - minsize * g.ScaleY / 2f, minsize * g.ScaleX, minsize * g.ScaleY);
                }

                var last = obj.GetFirst();

                foreach (var current in obj.Profile)
                {
                    DrawStep(g, last, current);
                    last = current;
                }

                DrawStep(g, last, obj.GetLast(), _radiusPen, _arcPen);
            }

            return rendered;
        }


        private static void DrawStep(GraphicsD g, CircularProfile.ProfileStep last,
            CircularProfile.ProfileStep current, Pen radiusPen = null, Pen arcPen = null)
        {
            var ad = 180f * ScaleAngle(g, last.Angle) / Math.PI;
            var sw = 180f * (current.Angle - last.Angle) / Math.PI;

            if (last.Radius > 0f)
            {
                var x = g.OffsetX - g.ScaleX * last.Radius;
                var y = g.OffsetY - g.ScaleY * last.Radius;
                var w = g.ScaleX * last.Radius * 2f;
                var h = g.ScaleY * last.Radius * 2f;
                g.Graphics.DrawArc(arcPen ?? Pens.Green, (float)x, (float)y, (float)w, (float)h, (float)ad,
                    (float)sw);
            }

            if (!current.Radius.NearlyEquals(last.Radius))
            {
                var x1 = g.OffsetX + g.ScaleX * last.Radius * Math.Cos(current.Angle);
                var y1 = g.OffsetY + g.ScaleY * last.Radius * Math.Sin(current.Angle);
                var x2 = g.OffsetX + g.ScaleX * current.Radius * Math.Cos(current.Angle);
                var y2 = g.OffsetY + g.ScaleY * current.Radius * Math.Sin(current.Angle);
                g.Graphics.DrawLine(radiusPen ?? Pens.Orange, (float)x1, (float)y1, (float)x2, (float)y2);
            }
        }

        private static double ScaleAngle(GraphicsD g, double angle)
        {
            while (angle < -Math.PI)
                angle += 2 * Math.PI;

            while (angle > Math.PI)
                angle -= 2 * Math.PI;

            var a = Math.Atan2(g.ScaleY * Math.Tan(angle), g.ScaleX);

            if (angle > Math.PI / 2f)
                return a + Math.PI;

            if (angle < -Math.PI / 2f)
                return a - Math.PI;

            return a;
        }
    }
}
