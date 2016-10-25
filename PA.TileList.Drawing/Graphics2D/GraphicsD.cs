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

            // Diagonal bars = Inner Zone
            this.Graphics.DrawRectangle(p, r.X, r.Y, r.Width, r.Height);
            this.Graphics.DrawLine(p, r.Left, r.Top, r.Right, r.Bottom);
            this.Graphics.DrawLine(p, r.Left, r.Bottom, r.Right, r.Top);

            // Vertical Bars  = Offset
            this.Graphics.DrawLine(p, this.OffsetX, r.Top, this.OffsetX, r.Bottom);
            this.Graphics.DrawLine(p, r.Left, this.OffsetY, r.Right, this.OffsetY);
        }

        #endregion
    }
}