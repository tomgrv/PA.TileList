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
using System.Xml.Linq;

namespace PA.TileList.Drawing.Circular
{
    public static class ProfileExtentions
    {
        public static RectangleD<Bitmap> GetImage(this CircularProfile p, int width, int height, RectangleF inner, ScaleMode mode = ScaleMode.NONE, Pen radiusPen = null, Pen arcPen = null, Pen extraPen = null)
        {
            return p.GetImage(new RectangleD<Bitmap>(new Bitmap(width, height), inner, mode), radiusPen, arcPen, extraPen);
        }


        public static RectangleD<Bitmap> GetImage(this CircularProfile p, int width, int height, ScaleMode mode = ScaleMode.NONE, Pen radiusPen = null, Pen arcPen = null, Pen extraPen = null)
        {
            return p.GetImage(new RectangleD<Bitmap>(new Bitmap(width, height), new PointF(-width / 2f, -height / 2f), new SizeF(width, height), mode), radiusPen, arcPen, extraPen);
        }

        /// <summary>
        /// Gets the CircularProfile image in TopLeft coordinates
        /// </summary>
        /// <returns>The image.</returns>
        /// <param name="p">P.</param>
        /// <param name="image">Image.</param>
        /// <param name="radiusPen">Radius pen.</param>
        /// <param name="arcPen">Arc pen.</param>
        /// <param name="extraPen">Extra pen.</param>
        /// <typeparam name="U">The 1st type parameter.</typeparam>
        public static RectangleD<U> GetImage<U>(this CircularProfile p, RectangleD<U> image, Pen radiusPen = null, Pen arcPen = null, Pen extraPen = null)
            where U : Image
        {
            using (GraphicsD g = image.GetGraphicsD(extraPen))
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
                    g.DrawStep(last, current);
                    last = current;
                }

                g.DrawStep(last, p.GetLast());
            }

            return image;
        }

        public static void DrawStep(this GraphicsD g, CircularProfile.ProfileStep last, CircularProfile.ProfileStep current, Pen radiusPen = null)
        {
            var ad = 180f * g.ScaleAngle(last.Angle) / Math.PI;
            var sw = 180f * (current.Angle - last.Angle) / Math.PI;

            if (last.Radius > 0f)
            {
                var x = g.OffsetX - g.ScaleX * last.Radius;
                var y = g.OffsetY - g.ScaleY * last.Radius;
                var w = g.ScaleX * last.Radius * 2f;
                var h = g.ScaleY * last.Radius * 2f;
                g.Graphics.DrawArc(radiusPen ?? Pens.Green, (float)x, (float)y, (float)w, (float)h, (float)ad, (float)sw);
            }

            if (!current.Radius.NearlyEquals(last.Radius))
            {
                var x1 = g.OffsetX + g.ScaleX * last.Radius * Math.Cos(current.Angle);
                var y1 = g.OffsetY + g.ScaleY * last.Radius * Math.Sin(current.Angle);
                var x2 = g.OffsetX + g.ScaleX * current.Radius * Math.Cos(current.Angle);
                var y2 = g.OffsetY + g.ScaleY * current.Radius * Math.Sin(current.Angle);
                g.Graphics.DrawLine(radiusPen ?? Pens.Orange, (float)x1, (float)y1, (float)x2, (float)y2);
            }
        }

        public static double ScaleAngle(this GraphicsD g, double angle)
        {
            while (angle < -Math.PI)
            {
                angle += 2 * Math.PI;
            }

            while (angle > Math.PI)
            {
                angle -= 2 * Math.PI;
            }

            double a = Math.Atan2(g.ScaleY * Math.Tan(angle), g.ScaleX);

            if (angle > Math.PI / 2f)
                return a + Math.PI;

            if (angle < -Math.PI / 2f)
                return a - Math.PI;

            return a;
        }
    }
}