﻿//
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

namespace PA.TileList.Drawing.Linear
{
    public class RectangularRenderer: IRenderer<RectangularProfile, Bitmap>
    {
        private Pen _rectPen;

        public RectangularRenderer(Color c, float width = 1f)
        {
            var p = new Pen(c, width);
            this._rectPen = p;
        }

        public RectangularRenderer(Pen rectPen = null)
        {
            this._rectPen = rectPen;
        }

        public RectangleD<Bitmap> Render(RectangularProfile obj, RectangleD<Bitmap> portion, RectangleF? visible)
        {
            return this.Render(obj, new Bitmap(portion.Item), portion as RectangleD, portion.Mode, visible);
        }

        public RectangleD<Bitmap> Render(RectangularProfile obj, int width, int height, ScaleMode mode, RectangleF? visible)
        {
            return this.Render(obj, new Bitmap(width, height), mode, visible);
        }

        public RectangleD<Bitmap> Render(RectangularProfile obj, Bitmap baseImage, ScaleMode mode, RectangleF? visible)
        {
            var w = Math.Abs(obj.xMax - obj.xMin);
            var h = Math.Abs(obj.yMax - obj.yMin);

            var m = Math.Max(w, h);

            var s = mode == ScaleMode.STRETCH ? new SizeF((float)m, (float)m) : new SizeF(baseImage.Width, baseImage.Height);
            var p = new PointF(-s.Width / 2f, -s.Height / 2f);
            var r = new RectangleD<Bitmap>(baseImage, p, s, mode);

            return this.Render(obj, r, visible);

        }

        public RectangleD<Bitmap> Render(RectangularProfile obj, int width, int height, RectangleF inner, ScaleMode mode, RectangleF? visible)
        {
            return this.Render(obj, new RectangleD<Bitmap>(new Bitmap(width, height), inner, mode), visible);
        }

        private RectangleD<Bitmap> Render(RectangularProfile obj, Bitmap image, RectangleD portion, ScaleMode mode, RectangleF? visible)
        {
            var rendered = new RectangleD<Bitmap>(image, portion, mode);

            using (var g = rendered.GetGraphicsD())
            {
                var min = g.Portion.Inner.Left;
                var max = g.Portion.Inner.Right;

                g.Graphics.DrawRectangle(this._rectPen ?? Pens.Blue, g.OffsetX + (float)obj.xMin * g.ScaleX, g.OffsetY + (float)obj.yMin * g.ScaleY,
                                                                    (float)(obj.xMax-obj.xMin) * g.ScaleX, (float)(obj.yMax - obj.yMin) * g.ScaleY);
            }

            return rendered;
        }

    }
}
