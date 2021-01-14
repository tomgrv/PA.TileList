using System;
using System.Drawing;

namespace PA.TileList.Drawing.Graphics2D
{
    public class RectangleD
    {
        public RectangleD(float x, float y, float width, float height)
        {
            Inner = new RectangleF(x, y, width, height);
            Outer = new RectangleF(x, y, width, height);
        }

        public RectangleD(PointF p, SizeF s)
        {
            Inner = new RectangleF(p, s);
            Outer = new RectangleF(p, s);
        }

        public RectangleD(RectangleF inner)
        {
            Inner = inner;
            Outer = inner;
        }

        public RectangleD(RectangleF outer, RectangleF inner)
        {
            if (!outer.Contains(inner))
                throw new ArgumentOutOfRangeException(nameof(inner), nameof(outer) + " must contain " + nameof(inner));

            Inner = inner;
            Outer = outer;
        }

        public RectangleF Inner { get; private set; }

        public RectangleF Outer { get; private set; }


        public void Round()
        {
            Inner = new RectangleF(Round(Inner.X), Round(Inner.Y), Round(Inner.Width), Round(Inner.Height));
            Outer = new RectangleF(Round(Outer.X), Round(Outer.Y), Round(Outer.Width), Round(Outer.Height));
        }

        private float Round(float v)
        {
            return (float) Math.Round(v);
        }
    }

    public class RectangleD<T> : RectangleD
    {
        public RectangleD(T item, float x, float y, float width, float height, ScaleMode mode = ScaleMode.STRETCH)
            : base(x, y, width, height)
        {
            Item = item;
            Mode = mode;
        }

        public RectangleD(T item, PointF p, SizeF s, ScaleMode mode = ScaleMode.STRETCH)
            : base(p, s)
        {
            Item = item;
            Mode = mode;
        }

        public RectangleD(T item, RectangleD portion, ScaleMode mode = ScaleMode.STRETCH)
            : base(portion.Outer, portion.Inner)
        {
            Item = item;
            Mode = mode;
        }

        public RectangleD(T item, RectangleF inner, ScaleMode mode = ScaleMode.STRETCH)
            : base(inner)
        {
            Item = item;
            Mode = mode;
        }

        public RectangleD(T item, RectangleF outer, RectangleF inner, ScaleMode mode = ScaleMode.STRETCH)
            : base(outer, inner)
        {
            Item = item;
            Mode = mode;
        }

        public T Item { get; }

        public ScaleMode Mode { get; }
    }
}