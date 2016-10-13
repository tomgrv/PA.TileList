using System;
using System.Drawing;

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
    }
}