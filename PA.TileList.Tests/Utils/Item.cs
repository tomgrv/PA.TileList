﻿//
// MainTile.cs
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

using System.Drawing;
using PA.TileList.Linear;
using PA.TileList.Drawing.Graphics2D;
using System;
using PA.TileList.Cacheable;
using System.Collections.Generic;

namespace PA.TileList.Tests.Utils
{
	public class Item : Coordinate, ICacheable
	{
		public Item(int x, int y, Color c)
			: base(x, y)
		{
			this.Color = c;
			this._changed = new Dictionary<object, bool>();
			this._default = false;
		}

		public Color Color { get; set; }
		#region Cache

		private Dictionary<object, bool> _changed;
		private bool _default;

		public bool IsCached()
		{
			return IsCachedBy(null);
		}

		public bool IsCachedBy(object t)
		{
			if (!_changed.ContainsKey(t))
			{
				_changed.Add(t, _default);
			}

			return _changed[t];
		}

		public void NotifyCached()
		{
			NotifyCachedBy(null);
		}

		public void NotifyCachedBy(object t)
		{
			if (!_changed.ContainsKey(t))
			{
				_changed[t] = _default;
			}
			else
			{
				_changed.Add(t, _default);
			}
		}

		#endregion

		public Bitmap ToBitmap(int w, int h, string s)
		{
			var b = new Bitmap(w, h);

			using (var g = Graphics.FromImage(b))
			{
				g.Clip = new Region(new RectangleF(0, 0, w, h));
				this.Draw(g, s);
			}

			return b;
		}

		public Bitmap ToBitmap(SizeF s, ICoordinate c)
		{
			var b = new Bitmap((int)s.Width, (int)s.Height);

			using (var g = Graphics.FromImage(b))
			{
				g.Clip = new Region(new RectangleF(PointF.Empty, s));
				this.Draw(g, c.X + "\n" + c.Y);
			}

			return b;
		
		}

		public void Draw(Graphics g, String s)
		{
			var r = g.ClipBounds;

			g.FillRectangle(new SolidBrush(Color.Pink), r.X, r.Y, r.Width, r.Height);
			g.FillRectangle(new SolidBrush(this.Color), r.X + 1, r.Y + 1, r.Width - 2, r.Height - 2);
			g.DrawString(s, new Font(FontFamily.GenericSansSerif, r.Height / 3f), Brushes.Gray, r.X, r.Y);
		}
	}
}