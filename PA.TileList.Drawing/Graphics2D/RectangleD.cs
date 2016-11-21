using System;
using System.Drawing;
using System.Diagnostics.Contracts;

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
                throw new ArgumentOutOfRangeException(nameof(inner), nameof(outer) + " must contain " + nameof(inner));

            this.Inner = inner;
            this.Outer = outer;
        }

        public RectangleF Inner { get; private set; }

        public RectangleF Outer { get; private set; }


        public void Round()
        {
            this.Inner = new RectangleF(this.Round(Inner.X), this.Round(Inner.Y), this.Round(Inner.Width), this.Round(Inner.Height));
            this.Outer = new RectangleF(this.Round(Outer.X), this.Round(Outer.Y), this.Round(Outer.Width), this.Round(Outer.Height));
        }

        private float Round(float v)
        {
            return (float)Math.Round(v);
        }
    }

    public class RectangleD<T> : RectangleD
    {
        public RectangleD(T item, float x, float y, float width, float height, ScaleMode mode = ScaleMode.STRETCH)
            : base(x, y, width, height)
        {
            this.Item = item;
            this.Mode = mode;
        }

        public RectangleD(T item, PointF p, SizeF s, ScaleMode mode = ScaleMode.STRETCH)
            : base(p, s)
        {
            this.Item = item;
            this.Mode = mode;
        }

        public RectangleD(T item, RectangleD portion, ScaleMode mode = ScaleMode.STRETCH)
            : base(portion.Outer, portion.Inner)
        {
            this.Item = item;
            this.Mode = mode;
        }

        public RectangleD(T item, RectangleF inner, ScaleMode mode = ScaleMode.STRETCH)
            : base(inner)
        {
            this.Item = item;
            this.Mode = mode;
        }

        public RectangleD(T item, RectangleF outer, RectangleF inner, ScaleMode mode = ScaleMode.STRETCH)
            : base(outer, inner)
        {
            this.Item = item;
            this.Mode = mode;
        }

        public T Item { get; private set; }

        public ScaleMode Mode { get; private set; }

    }
}