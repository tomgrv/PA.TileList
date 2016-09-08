using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using PA.TileList.Quantified;
using PA.TileList.Contextual;
using PA.TileList.Circular;
using PA.Utilities;
using System.Drawing.Text;
using PA.TileList.Drawing.Graphics2D;

namespace PA.TileList.Drawing.Circular
{
    public static class ProfileExtentions
    {
        public static RectangleD<Bitmap> GetImage(this CircularProfile p, int width, int height, RectangleF inner, ScaleMode mode = ScaleMode.NONE, Pen radiusPen = null, Pen arcPen = null, Pen extraPen = null)
        {
            return p.GetImage(new RectangleD<Bitmap>(new Bitmap(width, height), new PointF(inner.X - (mode.HasFlag(ScaleMode.CENTER) ? inner.Width / 2f : 0f), inner.Y - (mode.HasFlag(ScaleMode.CENTER) ? inner.Height / 2f : 0f)), inner.Size), mode, radiusPen, arcPen, extraPen);
        }


        public static RectangleD<Bitmap> GetImage(this CircularProfile p, int width, int height, ScaleMode mode = ScaleMode.NONE, Pen radiusPen = null, Pen arcPen = null, Pen extraPen = null)
        {
            return p.GetImage(new RectangleD<Bitmap>(new Bitmap(width, height), mode.HasFlag(ScaleMode.CENTER) ? new PointF(-width / 2f, -height / 2f) : PointF.Empty, new SizeF(width, height)), mode, radiusPen, arcPen, extraPen);
        }


        public static RectangleD<U> GetImage<U>(this CircularProfile p, RectangleD<U> image, ScaleMode mode = ScaleMode.ALL, Pen radiusPen = null, Pen arcPen = null, Pen extraPen = null)
            where U : Image
        {

            using (GraphicsD g = image.GetGraphicsD(mode, extraPen))
            {

                float maxsize = (float)p.GetMaxRadius() * 2f;
                float minsize = (float)p.GetMinRadius() * 2f;
                float midsize = (float)p.Radius * 2f;

                if (extraPen != null)
                {
                    g.Graphics.DrawLine(extraPen, g.Portion.Inner.Left, g.OffsetY, g.Portion.Inner.Right, g.OffsetY);
                    g.Graphics.DrawLine(extraPen, g.OffsetX, g.Portion.Inner.Top, g.OffsetX, g.Portion.Inner.Bottom);
                    g.Graphics.DrawLine(extraPen, g.Portion.Inner.Left, g.Portion.Inner.Top, g.Portion.Inner.Right, g.Portion.Inner.Bottom);
                    g.Graphics.DrawLine(extraPen, g.Portion.Inner.Right, g.Portion.Inner.Top, g.Portion.Inner.Left, g.Portion.Inner.Bottom);
                    g.Graphics.DrawEllipse(extraPen, g.OffsetX - midsize * g.ScaleX / 2f, g.OffsetY - midsize * g.ScaleY / 2f, midsize * g.ScaleX, midsize * g.ScaleY);
                }

                CircularProfile.ProfileStep last = p.GetFirst();

                foreach (CircularProfile.ProfileStep current in p.Profile)
                {
                    double ad = 180f * -last.Angle / Math.PI;
                    double sw = 180f * -(current.Angle - last.Angle) / Math.PI;

                    float lastRadius = (float)last.Radius;

                    if (lastRadius > 0f)
                    {
                        g.Graphics.DrawArc(arcPen ?? Pens.Green, g.OffsetX - lastRadius * g.ScaleX, g.OffsetY - lastRadius * g.ScaleY, lastRadius * g.ScaleX * 2f, lastRadius * g.ScaleY * 2f, (float)ad, (float)sw);
                    }

                    if (!current.Radius.NearlyEquals(last.Radius))
                    {
                        double x1 = g.OffsetX + (double)g.ScaleX * lastRadius * Math.Cos(current.Angle);
                        double y1 = g.OffsetY - (double)g.ScaleY * lastRadius * Math.Sin(current.Angle);
                        double x2 = g.OffsetX + (double)g.ScaleX * current.Radius * Math.Cos(current.Angle);
                        double y2 = g.OffsetY - (double)g.ScaleY * current.Radius * Math.Sin(current.Angle);
                        g.Graphics.DrawLine(radiusPen ?? Pens.Orange, (float)x1, (float)y1, (float)x2, (float)y2);
                    }

                    last = current;
                }
            }

            return image;
        }
    }
}