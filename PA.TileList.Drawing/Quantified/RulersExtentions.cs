using System;
using System.Drawing;
using PA.TileList.Drawing.Graphics2D;
using PA.TileList.Linear;
using PA.TileList.Quantified;

namespace PA.TileList.Drawing.Quantified
{
    public static class RulersExtentions
    {
        public static RectangleD<Bitmap> GetRulers<T>(this IQuantifiedTile<T> c, int width, int height, float[] steps,
            ScaleMode mode = ScaleMode.NONE)
            where T : ICoordinate
        {
            var b = c.GetBounds();
            return c.GetRulers(new RectangleD<Bitmap>(new Bitmap(width, height), b, b, mode), steps);
        }

        public static RectangleD<U> GetRulers<T, U>(this IQuantifiedTile<T> c, RectangleD<U> image, float[] steps)
            where T : ICoordinate
            where U : Image
        {
            using (var g = image.GetGraphicsD(null))
            {
                if (image.Mode.HasFlag(ScaleMode.XYRATIO))
                {
                    g.Graphics.DrawSteps(steps, image.Inner.Left, image.Inner.Right, g.OffsetX, Direction.Horizontal,
                        g.ScaleX);
                    g.Graphics.DrawSteps(steps, image.Inner.Top, image.Inner.Bottom, g.OffsetY, Direction.Vertical,
                        g.ScaleY);
                }
                else
                {
                    g.Graphics.DrawSteps(steps, g.Portion.Inner.Left, g.Portion.Inner.Right, g.OffsetX,
                        Direction.Horizontal, g.ScaleX);
                    g.Graphics.DrawSteps(steps, g.Portion.Inner.Top, g.Portion.Inner.Bottom, g.OffsetY,
                        Direction.Vertical, g.ScaleY);
                }
            }

            return image;
        }

        private static void DrawSteps(this Graphics g, float[] steps, float min, float max, float offset, Direction d,
            float scale = 1)
        {
            switch (d)
            {
                case Direction.Horizontal:
                    g.DrawLine(Pens.Black, min*scale, 0, max*scale, 0);
                    break;
                case Direction.Vertical:
                    g.DrawLine(Pens.Black, 0, min*scale, 0, max*scale);
                    break;
            }

            for (var i = 0; i < steps.Length; i++)
            {
                float start = 0;
                var step = steps[i]*scale;
                var size = (i + 1f)/scale;

                while (start < min*scale)
                    start += step;

                while (start > min*scale)
                    start -= step;

                for (var position = start + step; position < max*scale; position += step)
                    switch (d)
                    {
                        case Direction.Vertical:
                            if (i == 0)
                                g.DrawString(Math.Round(position/scale).ToString(),
                                    new Font(FontFamily.GenericSansSerif, 10/scale), Brushes.Black, offset - size,
                                    position + offset);
                            g.DrawLine(Pens.Black, offset - 10*size, position + offset, offset + 10*size,
                                position + offset);
                            break;
                        case Direction.Horizontal:
                            if (i == 0)
                                g.DrawString(Math.Round(position/scale).ToString(),
                                    new Font(FontFamily.GenericSansSerif, 10/scale), Brushes.Black, position + offset,
                                    offset - size);
                            g.DrawLine(Pens.Black, position + offset, offset - 10*size, position + offset,
                                offset + 10*size);
                            break;
                    }
            }
        }

        private enum Direction
        {
            Vertical,
            Horizontal
        }
    }
}