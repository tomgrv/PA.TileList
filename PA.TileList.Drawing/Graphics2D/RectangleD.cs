using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PA.TileList.Drawing.Graphics2D
{
    public class RectangleD
    {
        public RectangleF Inner { get; private set; }

        public RectangleF Outer { get; private set; }

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
            {
                throw new ArgumentOutOfRangeException("inner", "Outer RectangleF must contain Inner RectangleF");
            }

            this.Inner = inner;
            this.Outer = outer;
        }
    }

    public class RectangleD<T> : RectangleD
    {
        public T Item { get; private set; }

        public RectangleD(T item, float x, float y, float width, float height)
            : base(x, y, width, height)
        {
            this.Item = item;
        }

        public RectangleD(T item, PointF p, SizeF s)
            : base(p, s)
        {
            this.Item = item;
        }

        public RectangleD(T item, RectangleD portion)
            : base(portion.Outer, portion.Inner)
        {
            this.Item = item;
        }

        public RectangleD(T item, RectangleF inner)
            : base(inner)
        {
            this.Item = item;
        }

        public RectangleD(T item, RectangleF outer, RectangleF inner)
            : base(outer, inner)
        {
            this.Item = item;
        }
    }
}
