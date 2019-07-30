using System;
using System.Drawing;
using System.Diagnostics.Contracts;

namespace PA.TileList.Drawing.Graphics2D
{
    public class GraphicsD : IDisposable
    {
        public GraphicsD(Graphics g, float scaleX, float scaleY, RectangleF outer, RectangleF inner)
        {
            this.ScaleX = scaleX;
            this.ScaleY = scaleY;
            this.OffsetX = 0;
            this.OffsetY = 0;
            this.Graphics = g;
            this.Portion = new RectangleD(outer, inner);
        }

        public GraphicsD(Graphics g, float scaleX, float scaleY, RectangleF outer, RectangleF inner,
            float offsetX, float offsetY)
        {
            this.ScaleX = scaleX;
            this.ScaleY = scaleY;
            this.OffsetX = offsetX;
            this.OffsetY = offsetY;
            this.Graphics = g;
            this.Portion = new RectangleD(outer, inner);
        }

        public float ScaleX { get; private set; }

        public float ScaleY { get; private set; }

        public float OffsetX { get; private set; }

        public float OffsetY { get; private set; }

        public Graphics Graphics { get; }

        public RectangleD Portion { get; private set; }

        #region IDisposable implementation

        public void Dispose()
        {
            this.Graphics.Dispose();
        }

        #endregion

        #region Debug

        public void Draw(Pen p)
        {
            Contract.Requires(p != null);

            var o = new SizeF(this.OffsetX, this.OffsetY);
            var r = new RectangleF(this.Portion.Inner.Location + o, this.Portion.Inner.Size);
			var i = new Pen(Color.Black, p.Width);
				
            // Diagonal bars = Inner Zone
            this.Graphics.DrawRectangle(p, r.X, r.Y, r.Width, r.Height);
            this.Graphics.DrawLine(p, r.Left, r.Top, r.Right, r.Bottom);
            this.Graphics.DrawLine(p, r.Left, r.Bottom, r.Right, r.Top);

            // Vertical Bars  = Offset
            this.Graphics.DrawLine(i, this.OffsetX, r.Top, this.OffsetX, r.Bottom);
            this.Graphics.DrawLine(i, r.Left, this.OffsetY, r.Right, this.OffsetY);
        }

		public void DrawPortion<T>(RectangleD<T> portion,Pen p)
		{
			Contract.Requires(p != null);

			var i = new Pen(Color.Black, p.Width);

			// Vertical Bars  = Outer
			this.Graphics.DrawRectangle(i, portion.Outer.X, portion.Outer.Y, portion.Outer.Width, portion.Outer.Height);
			this.Graphics.DrawLine(i,  portion.Outer.X + portion.Outer.Width/2, portion.Outer.Top,  portion.Outer.X + portion.Outer.Width/2,portion.Outer.Bottom);
			this.Graphics.DrawLine(i, portion.Outer.Left, portion.Outer.Y + portion.Outer.Height/2,  portion.Outer.Right, portion.Outer.Y + portion.Outer.Height/2);

            // Diagonal bars = Inner Zone
			this.Graphics.DrawRectangle(p,portion.Inner.X,portion.Inner.Y, portion.Inner.Width, portion.Inner.Height);
            this.Graphics.DrawLine(p, portion.Inner.Left, portion.Inner.Top, portion.Inner.Right, portion.Inner.Bottom);
            this.Graphics.DrawLine(p, portion.Inner.Left, portion.Inner.Bottom, portion.Inner.Right, portion.Inner.Top);

            
           
		}

        #endregion
    }
}