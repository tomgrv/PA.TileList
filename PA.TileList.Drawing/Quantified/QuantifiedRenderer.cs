//
// QuantifierRenderer.cs
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
using PA.TileList.Linear;
using System.Linq;
using PA.TileList.Cacheable;

namespace PA.TileList.Drawing.Quantified
{
    public class QuantifiedRenderer<T> : IRenderer<IQuantifiedTile<T>, Bitmap>
    where T : ICoordinate
    {
        private Pen _portionPen;
        private Pen _extraPen;
        private Func<T, SizeF, Bitmap> _getImagePortion;
        private Action<T, Graphics, ScaleMode> _drawImagePortion;

        public QuantifiedRenderer(Func<T, SizeF, Bitmap> getImagePortion, Pen portionPen = null, Pen extraPen = null)
        {
            this._getImagePortion = getImagePortion;
            this._portionPen = portionPen;
            this._extraPen = extraPen;
        }

        public QuantifiedRenderer(Action<T, Graphics, ScaleMode> drawImagePortion, Pen portionPen = null, Pen extraPen = null)
        {
            this._drawImagePortion = drawImagePortion;
            this._portionPen = portionPen;
            this._extraPen = extraPen;
        }

        public RectangleD<Bitmap> Render(IQuantifiedTile<T> obj, Bitmap baseImage, ScaleMode mode)
        {
            var s = mode.HasFlag(ScaleMode.STRETCH) ? obj.GetSize() : new SizeF(baseImage.Width, baseImage.Height);
            var p = mode.HasFlag(ScaleMode.STRETCH) ? obj.GetOrigin() : new PointF(-baseImage.Width / 2f, -baseImage.Height / 2f);

            return this.Render(obj, baseImage, new RectangleD(p, s), mode);
        }

        public RectangleD<Bitmap> Render(IQuantifiedTile<T> obj, int width, int height, ScaleMode mode)
        {
            return this.Render(obj, new Bitmap(width, height), mode);
        }

        public RectangleD<Bitmap> Render(IQuantifiedTile<T> obj, int width, int height, RectangleF inner, ScaleMode mode)
        {
            return this.Render(obj, new Bitmap(width, height), new RectangleD(inner), mode);
        }

        public RectangleD<Bitmap> Render(IQuantifiedTile<T> obj, RectangleD<Bitmap> portion)
        {
            return this.Render(obj, new Bitmap(portion.Item), portion as RectangleD, portion.Mode);
        }

        private RectangleD<Bitmap> Render(IQuantifiedTile<T> obj, Bitmap image, RectangleD portion, ScaleMode mode)
        {
            var rendered = new RectangleD<Bitmap>(image, portion, mode);

            using (var g = rendered.GetGraphicsD())
            {
                if (this._extraPen != null)
                {
                    g.Draw(this._extraPen);
                }

                foreach (var subportion in obj.GetPortions(g, mode).Where(p => (p.Outer.Height >= 1f) && (p.Outer.Width >= 1f)))// && (visible?.IntersectsWith(p.Outer) ?? true)))
                {
                    if (typeof(ICacheable).IsAssignableFrom(typeof(T)))
                    {
                        var item = subportion.Item as ICacheable;

                        if (item.IsCachedBy(this))
                        {
                            this.RenderPortion(subportion, g, mode);
                            item.NotifyCachedBy(this);
                        }
                    }
                    else
                    {
                        this.RenderPortion(subportion, g, mode);
                    }
                }
            }

            return rendered;
        }

        private void RenderPortion(RectangleD<T> subportion, GraphicsD g, ScaleMode mode)
        {
            if (mode.HasFlag(ScaleMode.PXLSNAP))
            {
                subportion.Round();
            }

            if (this._getImagePortion != null)
            {
                using (var subportionBitmap = this._getImagePortion(subportion.Item, subportion.Inner.Size))
                {
                    if (subportionBitmap != null)
                    {
                        g.Graphics.DrawImage(subportionBitmap, subportion.Inner);
                    }
                }
            }

            if (this._drawImagePortion != null)
            {
                g.Graphics.Clip = new Region(subportion.Inner);
                this._drawImagePortion(subportion.Item, g.Graphics, mode);
            }

            if (this._portionPen != null)
            {
                var dot = this._portionPen.Clone() as Pen;
                dot.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

                g.Graphics.DrawRectangle(this._portionPen, Rectangle.Round(subportion.Outer));
                g.Graphics.DrawRectangle(dot, Rectangle.Round(subportion.Inner));
            }
        }
    }
}
