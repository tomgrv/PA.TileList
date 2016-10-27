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
using PA.TileList.Linear;
using System.Linq;

namespace PA.TileList.Drawing.Quantified
{
    public class RulersRenderer<T> : IRenderer<IQuantifiedTile<T>, Bitmap>
    where T : ICoordinate
    {
        private float[] _steps;

        private enum Direction
        {
            Vertical,
            Horizontal
        }

        public RulersRenderer(float[] steps)
        {
            this._steps = steps;
        }

        public RectangleD<Bitmap> Render(IQuantifiedTile<T> obj, int width, int height, ScaleMode mode)
        {
            return this.Render(obj, new Bitmap(width, height), mode);
        }

        public RectangleD<Bitmap> Render(IQuantifiedTile<T> obj, Bitmap baseImage, ScaleMode mode)
        {
            return this.Render(obj, baseImage, new RectangleD(obj.GetOrigin(), obj.GetSize()), mode);
        }

        public RectangleD<Bitmap> Render(IQuantifiedTile<T> obj, int width, int height, RectangleF inner, ScaleMode mode)
        {
            return this.Render(obj, new RectangleD<Bitmap>(new Bitmap(width, height), inner, mode));
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
                if (rendered.Mode.HasFlag(ScaleMode.XYRATIO))
                {
                    DrawSteps(g.Graphics, this._steps, rendered.Inner.Left, rendered.Inner.Right, g.OffsetX, Direction.Horizontal,
                        g.ScaleX);
                    DrawSteps(g.Graphics, this._steps, rendered.Inner.Top, rendered.Inner.Bottom, g.OffsetY, Direction.Vertical,
                          g.ScaleY);
                }
                else
                {
                    DrawSteps(g.Graphics, this._steps, g.Portion.Inner.Left, g.Portion.Inner.Right, g.OffsetX,
                        Direction.Horizontal, g.ScaleX);
                    DrawSteps(g.Graphics, this._steps, g.Portion.Inner.Top, g.Portion.Inner.Bottom, g.OffsetY,
                        Direction.Vertical, g.ScaleY);
                }
            }

            return rendered;
        }

        private static void DrawSteps(Graphics g, float[] steps, float min, float max, float offset, Direction d,
            float scale = 1)
        {
            switch (d)
            {
                case Direction.Horizontal:
                    g.DrawLine(Pens.Black, min * scale, 0, max * scale, 0);
                    break;
                case Direction.Vertical:
                    g.DrawLine(Pens.Black, 0, min * scale, 0, max * scale);
                    break;
            }

            for (var i = 0; i < steps.Length; i++)
            {
                float start = 0;
                var step = steps[i] * scale;
                var size = (i + 1f) / scale;

                while (start < min * scale)
                    start += step;

                while (start > min * scale)
                    start -= step;

                for (var position = start + step; position < max * scale; position += step)
                    switch (d)
                    {
                        case Direction.Vertical:
                            if (i == 0)
                                g.DrawString(Math.Round(position / scale).ToString(),
                                    new Font(FontFamily.GenericSansSerif, 10 / scale), Brushes.Black, offset - size,
                                    position + offset);
                            g.DrawLine(Pens.Black, offset - 10 * size, position + offset, offset + 10 * size,
                                position + offset);
                            break;
                        case Direction.Horizontal:
                            if (i == 0)
                                g.DrawString(Math.Round(position / scale).ToString(),
                                    new Font(FontFamily.GenericSansSerif, 10 / scale), Brushes.Black, position + offset,
                                    offset - size);
                            g.DrawLine(Pens.Black, position + offset, offset - 10 * size, position + offset,
                                offset + 10 * size);
                            break;
                    }
            }
        }
    }
}
