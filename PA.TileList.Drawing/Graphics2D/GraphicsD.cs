using System;
using System.Diagnostics.Contracts;
using System.Drawing;

namespace PA.TileList.Drawing.Graphics2D
{
    public class GraphicsD : IDisposable
    {
        public GraphicsD(Graphics g, float scaleX, float scaleY, RectangleF outer, RectangleF inner)
        {
            ScaleX = scaleX;
            ScaleY = scaleY;
            OffsetX = 0;
            OffsetY = 0;
            Graphics = g;
            Portion = new RectangleD(outer, inner);
        }

        public GraphicsD(Graphics g, float scaleX, float scaleY, RectangleF outer, RectangleF inner,
            float offsetX, float offsetY)
        {
            ScaleX = scaleX;
            ScaleY = scaleY;
            OffsetX = offsetX;
            OffsetY = offsetY;
            Graphics = g;
            Portion = new RectangleD(outer, inner);
        }

        public float ScaleX { get; }

        public float ScaleY { get; }

        public float OffsetX { get; }

        public float OffsetY { get; }

        public Graphics Graphics { get; }

        public RectangleD Portion { get; }

        #region IDisposable implementation

        public void Dispose()
        {
            Graphics.Dispose();
        }

        #endregion

        #region Debug

        public void Draw(Pen p)
        {
            Contract.Requires(p != null);

            var o = new SizeF(OffsetX, OffsetY);
            var r = new RectangleF(Portion.Inner.Location + o, Portion.Inner.Size);
            var i = new Pen(Color.Black, p.Width);

            // Diagonal bars = Inner Zone
            Graphics.DrawRectangle(p, r.X, r.Y, r.Width, r.Height);
            Graphics.DrawLine(p, r.Left, r.Top, r.Right, r.Bottom);
            Graphics.DrawLine(p, r.Left, r.Bottom, r.Right, r.Top);

            // Vertical Bars  = Offset
            Graphics.DrawLine(i, OffsetX, r.Top, OffsetX, r.Bottom);
            Graphics.DrawLine(i, r.Left, OffsetY, r.Right, OffsetY);
        }

        public void DrawPortion<T>(RectangleD<T> portion, Pen p)
        {
            Contract.Requires(p != null);

            var i = new Pen(Color.Black, p.Width);

            // Vertical Bars  = Outer
            Graphics.DrawRectangle(i, portion.Outer.X, portion.Outer.Y, portion.Outer.Width, portion.Outer.Height);
            Graphics.DrawLine(i, portion.Outer.X + portion.Outer.Width / 2, portion.Outer.Top,
                portion.Outer.X + portion.Outer.Width / 2, portion.Outer.Bottom);
            Graphics.DrawLine(i, portion.Outer.Left, portion.Outer.Y + portion.Outer.Height / 2, portion.Outer.Right,
                portion.Outer.Y + portion.Outer.Height / 2);

            // Diagonal bars = Inner Zone
            Graphics.DrawRectangle(p, portion.Inner.X, portion.Inner.Y, portion.Inner.Width, portion.Inner.Height);
            Graphics.DrawLine(p, portion.Inner.Left, portion.Inner.Top, portion.Inner.Right, portion.Inner.Bottom);
            Graphics.DrawLine(p, portion.Inner.Left, portion.Inner.Bottom, portion.Inner.Right, portion.Inner.Top);
        }

        #endregion
    }
}