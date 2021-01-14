using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using PA.TileList.Contextual;
using PA.TileList.Drawing.Graphics2D;
using PA.TileList.Drawing.Quantified;
using PA.TileList.Quantified;
using PA.TileList.Selection;

namespace PA.TileList.Tests.Utils
{
    public static class GraphicDebug
    {
        public static RectangleD<Bitmap> GetDebugGraphic<T, U>(this IQuantifiedTile<T> tile, U profile,
            IRenderer<U, Bitmap> renderer, SelectionConfiguration selection)
            where T : IContextual<Item>
            where U : ISelectionProfile
        {
            var img = tile.RenderImage(5000, 5000, ScaleMode.STRETCH,
                new QuantifiedRenderer<T>((z, s) => z.Context.ToBitmap(s, z)));

            tile.DrawSelectionPoints<T, Bitmap>(profile, selection, img, Color.Green, Color.Red);

            return img.Render(profile, renderer).Render(tile, new RulersRenderer<T>(new[] {100f, 500f}));
        }

        public static RectangleD<Bitmap> GetDebugGraphic<T>(this IQuantifiedTile<T> tile)
            where T : IContextual<Item>
        {
            return tile.RenderImage(5000, 5000, ScaleMode.STRETCH,
                    new QuantifiedRenderer<T>((z, s) => z.Context.ToBitmap(s, z)))
                .Render(tile, new RulersRenderer<T>(new[] {100f, 500f}));
        }

        public static void SaveDebugImage(this RectangleD<Bitmap> image, string tag = null)
        {
            image.Item.SaveDebugImage(tag);
        }

        public static void SaveDebugImage(this Bitmap image, string tag = null)
        {
            var stack = new StackTrace();

            var frame =
                stack.GetFrames().FirstOrDefault(s => s.GetMethod().GetCustomAttributes(false)
                    .Any(i => i.ToString().EndsWith("TestAttribute")));

            var p = Directory.GetCurrentDirectory();

            if (frame != null)
            {
                var name = frame.GetMethod().Name + "_" + frame.GetMethod().DeclaringType.Name +
                           (tag != null ? "_" + tag : string.Empty);
                image.Save(Path.GetTempPath() + name + ".png", ImageFormat.Png);
            }
        }
    }
}