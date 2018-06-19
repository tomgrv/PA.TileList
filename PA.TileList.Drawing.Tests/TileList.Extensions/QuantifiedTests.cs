using System;
using System.Drawing;
using NUnit.Framework;
using PA.TileList.Circular;
using PA.TileList.Contextual;
using PA.TileList.Cropping;
using PA.TileList.Drawing.Circular;
using PA.TileList.Drawing.Graphics2D;
using PA.TileList.Drawing.Linear;
using PA.TileList.Drawing.Quantified;
using PA.TileList.Linear;
using PA.TileList.Quantified;
using PA.TileList.Selection;
using PA.TileList.Tests.Utils;
using PA.TileList.Tile;

namespace PA.TileList.Drawing.Tests.TileList.Extensions
{
    [TestFixture]
    public class QuantifiedTests
    {
        private void TestCoordinates<T>(IQuantifiedTile<T> q, Rectangle r, RectangleD<Bitmap> i, Action<T> a, Color cl)
            where T : class, ICoordinate
        {
            foreach (
                var c in
                q.SelectCoordinates(new RectangularProfile(r.Left, r.Top, r.Right, r.Bottom),
                    new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under), false))
            {
                this.DrawPoints(q, r, i, cl);


                var z = q.ElementAt(c);

                if (z != null)
                    a(z);
            }

            using (var g = i.GetGraphicsD())
            {
                g.Graphics.FillRectangle(Brushes.Black, r.Left * g.ScaleX, r.Top * g.ScaleY, r.Width * g.ScaleX,
                    r.Height * g.ScaleY);
            }
        }


        public void DrawPoints<T>(IQuantifiedTile<T> list, Rectangle r, RectangleD<Bitmap> i, Color cl)
            where T : ICoordinate
        {
            double minX = r.Left;
            double minY = r.Top;
            double maxX = r.Right;
            double maxY = r.Bottom;

            var pointsInX = Math.Max(1, (int)Math.Ceiling(list.ElementStepX / (maxX - minX))) + 1;
            var pointsInY = Math.Max(1, (int)Math.Ceiling(list.ElementStepY / (maxY - minY))) + 1;

            var g = i.GetGraphicsD();

            foreach (var c in list.Zone)
                c.GetPoints(list, pointsInX, pointsInY,
                    (xc, yc, xc2, yc2) =>
                    {
                        g.Graphics.FillRectangle(new SolidBrush(cl), ((float)xc - 1f) * g.ScaleX,
                            ((float)yc - 1f) * g.ScaleY, 3f * g.ScaleX, 3f * g.ScaleY);
                    });
        }

        [Test]
        public void Coordinates()
        {
            var tile = MainTile.GetTile(1);

            var t1 = tile
                .Flatten<SubTile, Item>();


			var coord1 = t1.GetCoordinateAt(1000, 500);
			Assert.IsNull(coord1);

			var coord2 = t1.GetCoordinateAt(1001, 500);
            var item = t1.ElementAt(coord2);
            item.Context.Color = Color.Red;

            Assert.AreEqual(item.X, coord2.X);
            Assert.AreEqual(item.Y, coord2.Y);
			Assert.AreEqual(23,coord2.X);
			Assert.AreEqual(11,coord2.Y);
        }

        [Test]
        public void CoordinatesIn()
        {
            var t0 = new Tile<Item>(new Zone(0, 0, 100, 100), new Item(0, 0, Color.Red));
            t0.Fill(
                c =>
                    (c.X > 25) && (c.X < 75) && (c.Y > 30) && (c.Y < 60)
                        ? new Item(c.X, c.Y, c.X == c.Y ? Color.Yellow : Color.Green)
                        : new Item(c.X, c.Y, Color.Red));

            var q0 = t0.ToQuantified(10, 10);

            var signature0 = q0.RenderImage(1000, 1000, ScaleMode.STRETCH, new QuantifiedRenderer<Item>((z, s) =>
                  z.ToBitmap(100, 50, z.X + "\n" + z.Y)), null).Item.GetSignature();


            foreach (
                var c in
                q0.SelectCoordinates(new RectangularProfile(250, 250, 600, 600),
                    new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under)))
                t0.Find(c).Color = Color.Chocolate;

            var signature1 = q0.RenderImage(1000, 1000, ScaleMode.STRETCH, new QuantifiedRenderer<Item>((z, s) =>
                   z.ToBitmap(100, 50, z.X + "\n" + z.Y)), null).Item.GetSignature();

            foreach (
                var c in
                q0.SelectCoordinates(new RectangularProfile(52, 52, 62, 62),
                    new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under)))
                t0.Find(c).Color = Color.White;

            var signature2 = q0.RenderImage(1000, 1000, ScaleMode.STRETCH, new QuantifiedRenderer<Item>((z, s) =>
                  z.ToBitmap(100, 50, z.X + "\n" + z.Y)), null).Item.GetSignature();

            foreach (
                var c in
                q0.SelectCoordinates(new RectangularProfile(12, 12, 13, 13),
                    new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under)))
                t0.Find(c).Color = Color.Black;

            var signature3 = q0.RenderImage(1000, 1000, ScaleMode.STRETCH, new QuantifiedRenderer<Item>((z, s) =>
                  z.ToBitmap(100, 50, z.X + "\n" + z.Y)), null).Item.GetSignature();
        }


        [Test]
        [Category("Image hash")]
        public void CoordinatesIn2()
        {
            const float factor = 1f;

            var tile = MainTile.GetTile(factor)
                .Flatten<SubTile, Item>();

            var p = new CircularProfile(1400);

            var r1 = new Rectangle(-100, -100, 200, 200);
            var r2 = new Rectangle(-525, -525, 500, 10);
            var r3 = new Rectangle(-450, -450, 20, 20);

            var q = tile.Filter(p, new SelectionConfiguration(SelectionPosition.Inside), true);

            var i = q.RenderImage(5000, 5000, ScaleMode.ALL, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(50, 50, z.X + "\n" + z.Y),
                Pens.Crimson), null);

            this.TestCoordinates(tile, r1, i, z => z.Context.Color = Color.Violet, Color.Aqua);
            this.TestCoordinates(tile, r2, i, z => z.Context.Color = Color.Violet, Color.OrangeRed);
            this.TestCoordinates(tile, r3, i, z => z.Context.Color = Color.Violet, Color.Blue);

            var ii = q.RenderImage(i, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(50, 50, z.X + "\n" + z.Y)), null);

            var pi = p.RenderImage(ii, new CircularProfileRenderer(), null);

            var pj = q.RenderImage(pi, new RulersRenderer<IContextual<Item>>(new[] { 100f, 500f }), null);

            var signature = pj.Item.GetSignature();
        }

        [Test]        
        [Category("Image hash")]
        public void FirstOrDefault()
        {
            var tile = MainTile.GetTile(1);

            var t1 = tile
                .Flatten<SubTile, Item>();

			var coord1 = t1.GetCoordinateAt(27.4, 38);
			Assert.AreEqual(3, coord1.X);
			Assert.AreEqual(4, coord1.Y);
            var item1 = t1.ElementAt(coord1);
            item1.Context.Color = Color.Red;

			var coord2 = t1.GetCoordinateAt(0, 0);
			Assert.IsNull(coord2);

            var i1 = t1.RenderImage(2000, 2000, ScaleMode.STRETCH, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(100, 50, z.X + "\n" + z.Y)), null);
            var signature = t1.RenderImage(i1, new RulersRenderer<IContextual<Item>>(new[] { 100f, 500f }), null).Item.GetSignature();
           // Assert.AreEqual("A56EBC8E87772EA73D38342AF45FF00B5489A22DB73E7ED5996C6AF7EEE3DE0A", signature, "Image hash");
        }

        [Test]
        [Category("Image hash")]
        public void Rulers()
        {
            var tile = MainTile.GetTile(1);

            var t1 = tile
                .Flatten<SubTile, Item>();

            t1.Reference.Context.Color = Color.Lavender;

            var item = t1.GetCoordinateAt(500, 1000);
            //item.Context.Color = Color.Red;

            var i1 = t1.RenderImage(2000, 2000, ScaleMode.STRETCH, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(100, 100, z.X + "\n" + z.Y)), null);

            var p = new CircularProfile(1000);

            var i2 = p.RenderImage(i1, new CircularProfileRenderer(null, null, Pens.DarkViolet), null);

            var signature = t1.RenderImage(i2, new RulersRenderer<IContextual<Item>>(new[] { 100f, 500f }), null).Item.GetSignature();
            Assert.AreEqual("9272D2C42A039C2122B649DAD516B390A3A2A3C51BA861B6E615F27BA0F1BDA3", signature, "Image hash");
        }


        [Test]
        [Category("Image hash")]
        public void Rectangle()
        {
            var tile = MainTile.GetTile(1);

            var t1 = tile
                .Flatten<SubTile, Item>();

            t1.Reference.Context.Color = Color.Lavender;

            var item = t1.GetCoordinateAt(502, 1000);

            t1.Find(item).Context.Color = Color.Pink;

            var i1 = t1.RenderImage(2000, 2000, ScaleMode.STRETCH, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(100, 100, z.X + "\n" + z.Y)), null);

            var p = new CircularProfile(1000);

            var i2 = p.RenderImage(i1, new CircularProfileRenderer(null, null, Pens.DarkViolet), null);

            var r = new RectangleF(500, 1000, 500,100);

            var signature = t1.RenderImage(i2, new RectangleRenderer<IContextual<Item>>(r), null).Item.GetSignature();
            Assert.AreEqual("9272D2C42A039C2122B649DAD516B390A3A2A3C51BA861B6E615F27BA0F1BDA3", signature, "Image hash");
        }

        [Test]
        [Category("Image hash")]
        public void RulersFirst()
        {
            var tile = MainTile.GetTile(1);

            var t1 = tile
                .Flatten<SubTile, Item>();

            t1.Reference.Context.Color = Color.Lavender;

            var item = t1.GetCoordinateAt(500, 1000);
            //item.Context.Color = Color.Red;

            var r1 = t1.RenderImage(2000, 2000, ScaleMode.STRETCH, new RulersRenderer<IContextual<Item>>(new[] { 100f, 500f }), null);

            //            var i1 = t1.RenderImage(r1, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(100, 100, z.X + "\n" + z.Y)));

            var p = new CircularProfile(1000);

            var i2 = p.RenderImage(r1, new CircularProfileRenderer(null, null, Pens.DarkViolet), null);

            var signature = i2.Item.GetSignature();

            //    var signature = t1.RenderImage(i2, new RulersRenderer<IContextual<Item>>(new[] { 100f, 500f })).Item.GetSignature();
            Assert.AreEqual("9272D2C42A039C2122B649DAD516B390A3A2A3C51BA861B6E615F27BA0F1BDA3", signature, "Image hash");
        }

        [Test]
        [Category("Image hash")]
        public void ImbricatedRendering()
        {
            float factor = 1;

            var tile = MainTile.GetTile(factor);
            var flat = tile.Flatten<SubTile, Item>();

            var signature1 =
                        tile.ToBitmap(5000, 5000, new RectangleF(-2000, -2000, 4000, 4000)).GetSignature("imbricated");

            var signature2 =
                       flat.RenderImage(5000, 5000, new RectangleF(-2000, -2000, 4000, 4000), ScaleMode.STRETCH, new QuantifiedRenderer<IContextual<Item>>(
                                (z2, s2) =>
                                {
                                    return z2.Context.ToBitmap((int)s2.Width, (int)s2.Height, z2.X + "\n" + z2.Y);
                                }), null
                        ).Item.GetSignature("flattened");

        }
    }
}