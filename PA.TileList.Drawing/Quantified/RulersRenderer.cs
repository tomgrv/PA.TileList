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

		public RectangleD<Bitmap> Render(IQuantifiedTile<T> obj, int width, int height, ScaleMode mode, RectangleF? visible)
		{
			return this.Render(obj, new Bitmap(width, height), mode, visible);
		}

		public RectangleD<Bitmap> Render(IQuantifiedTile<T> obj, Bitmap baseImage, ScaleMode mode, RectangleF? visible)
		{
			return this.Render(obj, baseImage, new RectangleD(obj.GetOrigin(), obj.GetSize()), mode, visible);
		}

		public RectangleD<Bitmap> Render(IQuantifiedTile<T> obj, int width, int height, RectangleF inner, ScaleMode mode, RectangleF? visible)
		{
			return this.Render(obj, new RectangleD<Bitmap>(new Bitmap(width, height), inner, mode), visible);
		}

		public RectangleD<Bitmap> Render(IQuantifiedTile<T> obj, RectangleD<Bitmap> portion, RectangleF? visible)
		{
			return this.Render(obj, new Bitmap(portion.Item), portion as RectangleD, portion.Mode, visible);
		}

		private RectangleD<Bitmap> Render(IQuantifiedTile<T> obj, Bitmap image, RectangleD portion, ScaleMode mode, RectangleF? visible)
		{
			var rendered = new RectangleD<Bitmap>(image, portion, mode);

			using (var g = rendered.GetGraphicsD())
			{
				DrawSteps(g, this._steps, Direction.Vertical);
				DrawSteps(g, this._steps, Direction.Horizontal);

			}

			return rendered;
		}

		private static void DrawSteps(GraphicsD g, float[] steps, Direction d)
		{
			var min = 0f;
			var max = 0f;
			var scale = 1f;

			//g.Graphics.FillRectangle(Brushes.Pink, 
			//    new Rectangle((int)(g.OffsetX + g.Portion.Inner.X),  (int)(g.OffsetY + g.Portion.Inner.Y),
			//    (int)(g.Portion.Inner.Width ), (int)(g.Portion.Inner.Height )));

			switch (d)
			{
				case Direction.Vertical:
					min = g.Portion.Inner.Top;
					max = g.Portion.Inner.Bottom;
					scale = g.ScaleY;
					g.Graphics.DrawLine(Pens.Blue, g.OffsetX, min * scale, g.OffsetX, max * scale);
					break;

				case Direction.Horizontal:
					min = g.Portion.Inner.Left;
					max = g.Portion.Inner.Right;
					scale = g.ScaleX;
					g.Graphics.DrawLine(Pens.Blue, min * scale, g.OffsetY, max * scale, g.OffsetY);
					break;

			}

			for (var i = 0; i < steps.Length; i++)
			{
				var step = steps[i] * scale;
				var size = (i + 1f) / scale;
				var start = 0f;

				while (start < min)
					start += step;

				while (start > min)
					start -= step;

				for (var position = start; position < max; position += step)
					switch (d)
					{
						case Direction.Vertical:
							if (i == 0)
								g.Graphics.DrawString(Math.Round(position / scale).ToString(),
														  new Font(FontFamily.GenericSansSerif, 20 * g.ScaleY), Brushes.Black,
																  g.OffsetX - size, position + g.OffsetY);
							g.Graphics.DrawLine(Pens.Black, g.OffsetX - 10 * size, position + g.OffsetY, g.OffsetX + 10 * size,
								position + g.OffsetY);
							break;
						case Direction.Horizontal:
							if (i == 0)
								g.Graphics.DrawString(Math.Round(position / scale).ToString(),
														  new Font(FontFamily.GenericSansSerif, 20 * g.ScaleY), Brushes.Black,
															  position + g.OffsetX, g.OffsetY - size);
							g.Graphics.DrawLine(Pens.Black, position + g.OffsetX, g.OffsetY - 10 * size, position + g.OffsetX,
								g.OffsetY + 10 * size);
							break;
					}
			}
		}
	}
}
