//
// AbstractRenderer.cs
//
// Author:
//       Thomas <tomgrv@users.perspikapps.fr>
//
// Copyright (c) 2019 
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
using System.Drawing;

namespace PA.TileList.Drawing.Graphics2D
{
	public abstract class AbstractBitmapRenderer<T> : IRenderer<T, Bitmap>

	{
		public AbstractBitmapRenderer()
		{
		}

		public abstract void Draw(T obj, RectangleD<Bitmap> portion);

		public abstract RectangleD<Bitmap> Render(T obj, Bitmap baseImage, ScaleMode mode);

		public virtual RectangleD<Bitmap> Render(T obj, Bitmap baseImage,RectangleF inner, ScaleMode mode)
		{
			var r = new RectangleD<Bitmap>(baseImage, inner, mode);
			this.Draw(obj, r);
			return r;
		}

		public virtual RectangleD<Bitmap> Render(T obj, int width, int height, ScaleMode mode)
		{
			return this.Render(obj, new Bitmap(width, height), mode);
		}


		public virtual RectangleD<Bitmap> Render(T obj, int width, int height, RectangleF inner, ScaleMode mode)
		{
			var r = new RectangleD<Bitmap>(new Bitmap(width, height), inner, mode);
			this.Draw(obj, r);
			return r;
		}
	}
}
