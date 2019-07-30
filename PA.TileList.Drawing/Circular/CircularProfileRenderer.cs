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

namespace PA.TileList.Drawing.Circular
{
	public class CircularProfileRenderer : AbstractBitmapRenderer<CircularProfile>, IRenderer<CircularProfile, Bitmap>
	{
		private Pen _radiusPen;
		private Pen _arcPen;
		private Pen _extraPen;

		public CircularProfileRenderer(Color c, float width = 1f, Pen extraPen = null)
		{
			var p = new Pen(c, width);

			this._radiusPen = p;
			this._arcPen = p;
			this._extraPen = extraPen;
		}

		public CircularProfileRenderer(Pen radiusPen = null, Pen arcPen = null, Pen extraPen = null)
		{
			this._radiusPen = radiusPen;
			this._arcPen = arcPen;
			this._extraPen = extraPen;
		}

		public override void Draw(CircularProfile obj, RectangleD<Bitmap> portion)
		{
			using (var g = portion.GetGraphicsD())
			{
				var maxsize = (float)obj.GetMaxRadius() * 2f;
				var minsize = (float)obj.GetMinRadius() * 2f;
				var midsize = (float)obj.Radius * 2f;

				if (this._extraPen != null)
				{
					g.Draw(this._extraPen);

					g.Graphics.DrawEllipse(this._extraPen, g.OffsetX - maxsize * g.ScaleX / 2f, g.OffsetY - maxsize * g.ScaleY / 2f, maxsize * g.ScaleX, maxsize * g.ScaleY);
					g.Graphics.DrawEllipse(this._extraPen, g.OffsetX - minsize * g.ScaleX / 2f, g.OffsetY - minsize * g.ScaleY / 2f, minsize * g.ScaleX, minsize * g.ScaleY);
				}

				var last = obj.GetFirst();

				foreach (var current in obj.Profile)
				{
					DrawStep(g, last, current);
					last = current;
				}

				DrawStep(g, last, obj.GetLast());
			}


		}


		public override RectangleD<Bitmap> Render(CircularProfile obj, Bitmap baseImage, ScaleMode mode)
		{
			var m = 2 * obj.GetMaxRadius();

			var s = (mode == ScaleMode.STRETCH) ? new SizeF((float)m, (float)m) : new SizeF(baseImage.Width, baseImage.Height);
			var p = new PointF(-s.Width / 2f, -s.Height / 2f);
			var r = new RectangleD<Bitmap>(baseImage, p, s, mode);
			this.Draw(obj, r);
			return r;

		}
	
		private void DrawStep(GraphicsD g, CircularProfile.ProfileStep last, CircularProfile.ProfileStep current)
		{
			var ad = 180f * ScaleAngle(g, last.Angle) / Math.PI;
			var sw = 180f * (current.Angle - last.Angle) / Math.PI;

			if (last.Radius > 0f)
			{
				var x = g.OffsetX - g.ScaleX * last.Radius;
				var y = g.OffsetY - g.ScaleY * last.Radius;
				var w = g.ScaleX * last.Radius * 2f;
				var h = g.ScaleY * last.Radius * 2f;
				g.Graphics.DrawArc(this._arcPen ?? Pens.Black, (float)x, (float)y, (float)w, (float)h, (float)ad, (float)sw);
			}

			if (!(Math.Abs(current.Radius - last.Radius) < float.Epsilon))
			{
				var x1 = g.OffsetX + g.ScaleX * last.Radius * Math.Cos(current.Angle);
				var y1 = g.OffsetY + g.ScaleY * last.Radius * Math.Sin(current.Angle);
				var x2 = g.OffsetX + g.ScaleX * current.Radius * Math.Cos(current.Angle);
				var y2 = g.OffsetY + g.ScaleY * current.Radius * Math.Sin(current.Angle);
				g.Graphics.DrawLine(this._radiusPen ?? Pens.Black, (float)x1, (float)y1, (float)x2, (float)y2);
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