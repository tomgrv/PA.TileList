using System;
using System.Drawing;

namespace PA.TileList.Drawing.Graphics2D
{
    public class RectangleD
    {
        public RectangleD(float x, float y, float width, float height)
        {
            this.Inner = new RectangleF(x, y, width, height);
            this.Outer = new RectangleF(x, y, width, height);
        }

        public RectangleD(PointF p, SizeF s)
        {
            this.Inner = new RectangleF(p, s);
            this.Outer = new RectangleF(p, s);
        }

        public RectangleD(RectangleF inner)
        {
            this.Inner = inner;
            this.Outer = inner;
        }

        public RectangleD(RectangleF outer, RectangleF inner)
        {
            if (!outer.Contains(inner))
                throw new ArgumentOutOfRangeException("inner", "Outer RectangleF must contain Inner RectangleF");

            this.Inner = inner;
            this.Outer = outer;
        }

        public RectangleF Inner { get; }

        public RectangleF Outer { get; }
    }

    public class RectangleD<T> : RectangleD
    {
        public RectangleD(T item, float x, float y, float width, float height, ScaleMode mode = ScaleMode.NONE)
            : base(x, y, width, height)
        {
            this.Item = item;
            this.Mode = mode;
        }

        public RectangleD(T item, PointF p, SizeF s, ScaleMode mode = ScaleMode.NONE)
            : base(p, s)
        {
            this.Item = item;
            this.Mode = mode;
        }

        public RectangleD(T item, RectangleD portion, ScaleMode mode = ScaleMode.NONE)
            : base(portion.Outer, portion.Inner)
        {
            this.Item = item;
            this.Mode = mode;
        }

        public RectangleD(T item, RectangleF inner, ScaleMode mode = ScaleMode.NONE)
            : base(inner)
        {
            this.Item = item;
            this.Mode = mode;
        }

        public RectangleD(T item, RectangleF outer, RectangleF inner, ScaleMode mode = ScaleMode.NONE)
            : base(outer, inner)
        {
            this.Item = item;
            this.Mode = mode;
        }

        public T Item { get; private set; }

        public ScaleMode Mode { get; private set; }
    }
}