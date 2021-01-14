using System;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using PA.TileList.Circular;
using PA.TileList.Contextual;
using PA.TileList.Drawing.Circular;
using PA.TileList.Drawing.Graphics2D;
using PA.TileList.Drawing.Quantified;
using PA.TileList.Selection;
using PA.TileList.Tests.Utils;

namespace PA.TileList.Drawing.Tests.TileList.Extensions
{
    [TestFixture]
    public class CircularTests
    {
        public CircularProfileRenderer cpr = new CircularProfileRenderer(Pens.Red, Pens.Red, Pens.Pink);

        public static CircularProfile GetTestProfile(double radius, double stepping = 1f, double resolution = 1f)
        {
            var p = new CircularProfile(radius);

            p.AddProfileFlat(-Math.PI / 2, radius - 100, 100, stepping);
            p.AddProfileFlat(7 * Math.PI / 4, radius - 200, 100, stepping);
            //p.AddProfileFlat(-Math.PI / 4, radius - 200, 100, stepping);
            p.AddProfileFlat(0, radius - 300, 100, stepping, resolution);
            p.AddProfileFlat(Math.PI / 3f, radius - 400, 200, stepping, resolution);
            p.AddProfileFlat(2f * Math.PI / 3f, radius - 500, 400, stepping, resolution);

            return p;
        }

        public static CircularProfile GetZeroProfile(double radius = 1000, double stepping = 1f, double resolution = 1f)
        {
            var p = new CircularProfile(radius);

            for (var a = 0; a < 4; a++)
            {
                var a0 = -13f * Math.PI / 12f + a * Math.PI / 2f;
                var a1 = -11f * Math.PI / 12f + a * Math.PI / 2f;
                p.AddProfileStep(a0, 0);
                p.AddProfileStep(a1, radius);
            }

            return p;
        }


        public static CircularProfile GetFlatProfile(double radius = 1000, double stepping = 1f, double resolution = 1f)
        {
            var p = new CircularProfile(radius);

            p.AddProfileFlatByLength(0, 1500);
            p.AddProfileFlatByLength(Math.PI, 500, 0.0001, 500);
            p.AddProfileFlat(Math.PI / 2f, 800, radius);
            p.AddProfileStep(-Math.PI / 4, radius);
            p.AddProfileStep(-3 * Math.PI / 4, radius);

            return p;
        }

        public static CircularProfile GetSimpleProfile(double radius = 1000, double stepping = 1f,
            double resolution = 1f)
        {
            var p = new CircularProfile(radius);

            return p;
        }

        [Test]
        public void CircularProfile()
        {
            var pro = new CircularProfile(1500);

            var i = pro.RenderImage(1000, 1000, new RectangleF(-2000, -2000, 4000, 4000), ScaleMode.STRETCH, cpr);

            i.SaveDebugImage();
        }

        [Test]
        public void ProfileForTest()
        {
            var p = GetTestProfile(1400);

            var i = p.RenderImage(1000, 1000, new RectangleF(-2000, -2000, 4000, 4000), ScaleMode.STRETCH, cpr);

            i.SaveDebugImage();
        }

        [Test]
        public void ProfileWith0()
        {
            var p = GetZeroProfile();

            var i = p.RenderImage(1000, 1000, new RectangleF(-1000, -1000, 2000, 2000), ScaleMode.STRETCH, cpr);

            i.SaveDebugImage();
        }

        [Test]
        [Category("Image hash")]
        public void ProfileWithFlat()
        {
            var p = GetFlatProfile();

            var i = p.RenderImage(1000, 1000, new RectangleF(-1000, -1000, 2000, 2000), ScaleMode.STRETCH, cpr);

            i.SaveDebugImage();
        }

        [Test]
        [Category("Image hash")]
        public void SelectionInsideProfile()
        {
            var scs = new SelectionConfiguration(SelectionPosition.Inside, false);
            var pro = GetTestProfile(1400);
            var count = 0;

            var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
            scs.OptimizeResolution(tile, pro);

            tile.Reference.Context.Color = Color.Violet;

            Assert.AreEqual(3025, tile.Count(), "Initial item count");

            var change = tile.Reference;

            foreach (var c in tile.Take(pro, scs))
            {
                tile.Find(c).Context.Color = Color.Brown;
                count++;
            }

            tile.GetDebugGraphic(pro, cpr, scs).SaveDebugImage();
        }

        [Test]
        [Category("Image hash")]
        public void SelectionMediumTile()
        {
            const float factor = 5f;

            var tile = MainTile.GetTile(factor)
                .Flatten<SubTile, Item>();

            Assert.AreEqual(65025, tile.Count(), "Initial item count");

            var p = GetTestProfile(1000);

            var change = tile.Reference;

            var q = tile.Filter(p, new SelectionConfiguration(SelectionPosition.Inside));

            //var i = q.GetImage(5000, 5000, (z, s) => z.Context.ToBitmap(50, 50, z.X + "\n" + z.Y));
            //var pi = p.GetImage(i);
            //string signature = pi.Item.GetSignature();

            Assert.AreEqual(q.Reference, change, "Reference Changed");
            Assert.IsNotNull(q.Reference, "Reference is null");


            foreach (var tt in tile.Except(q))
                tt.Context.Color = Color.Transparent;

            // Assert.AreEqual(23467, q.Count(), "Selected item count");

            var pi = p.RenderImage(5000, 5000, tile.GetBounds(), ScaleMode.STRETCH,
                new CircularProfileRenderer(Pens.Red, Pens.Red, Pens.Pink));
            //var i = tile.RenderImage(pi, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(50, 50, z.X + "\n" + z.Y), Pens.Blue));
            tile.DrawImage(pi,
                new QuantifiedRenderer<IContextual<Item>>((z, g, m) => z.Context.Draw(g, z.X + "\n" + z.Y), Pens.Blue));
            var signature_1 = pi.Item.GetSignature();
            //    Assert.AreEqual("E63318A4278EED31907E0374B728F045285D43B6FBE0955A1622BFCFBB7AF5B8", signature_1, "Image hash");

            //var j = tile.GetImage(5000, 5000, (z, s) => z.Context.ToBitmap(50, 50, z.X + "\n" + z.Y));
            //var pj = p.GetImage(j);
            //string signature_2 = pj.Item.GetSignature();


            //      Assert.AreEqual("E63318A4278EED31907E0374B728F045285D43B6FBE0955A1622BFCFBB7AF5B8", signature_2, "Image hash");
        }

        [Test]
        [Category("Image hash")]
        public void SelectionOnProfile()
        {
            const float factor = 1f;

            var tile = MainTile.GetTile(factor)
                .Flatten<SubTile, Item>();

            Assert.AreEqual(3025, tile.Count(), "Initial item count");

            var p = GetTestProfile(1400);

            var change = tile.Reference;

            var q = tile.Filter(p, new SelectionConfiguration(SelectionPosition.Under));

            //Assert.AreNotEqual(change, q.Reference, "Reference Changed");
            //Assert.IsNotNull(q.Reference, "Reference is null");

            q.Reference.Context.Color = Color.Pink;

            var i = q.RenderImage(5000, 2000, new RectangleF(-2000, -2000, 4000, 4000),
                ScaleMode.XYRATIO | ScaleMode.STRETCH,
                new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(50, 50, z.X + "\n" + z.Y),
                    Pens.Red, Pens.Blue));

            p.DrawImage(i, new CircularProfileRenderer(Pens.Red, Pens.Aquamarine, Pens.Green));

            var signature = i.Item.GetSignature();
            //
            //Assert.AreEqual(246, q.Count(), "Selected item count");

            //    Assert.AreEqual("ADE22DBF99F378AEE20F993BF51705756AFFF2539CA8D6CC5CCA7266C9F2B551", signature, "Image hash");
        }

        [Test]
        [Category("Image hash")]
        public void SelectionOutsideProfile()
        {
            const float factor = 1f;

            var tile = MainTile.GetTile(factor).Flatten<SubTile, Item>();

            Assert.AreEqual(3025, tile.Count(), "Initial item count");

            var p = GetTestProfile(1400);

            var change = tile.Reference;

            var q = tile.Filter(p, new SelectionConfiguration(SelectionPosition.Outside));

            Assert.IsNotNull(q.Reference, "Reference is null");
            Assert.AreNotEqual(q.Reference, change, "Reference Changed");

            q.Reference.Context.Color = Color.Pink;

            Assert.IsTrue(q.Contains(q.Reference), "Reference is contained");
            // Assert.AreEqual(change, q.Reference, "test");

            var pi = p.RenderImage(5000, 5000, new RectangleF(-2000, -2000, 4000, 4000), ScaleMode.STRETCH,
                new CircularProfileRenderer(Pens.Red, Pens.Red, Pens.Pink));
            q.DrawImage(pi,
                new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(50, 50, z.X + "\n" + z.Y),
                    Pens.Red));


            var signature = pi.Item.GetSignature();
            //
            Assert.AreEqual(960, q.Count(), "Selected item count");

            //    Assert.AreEqual("ADE22DBF99F378AEE20F993BF51705756AFFF2539CA8D6CC5CCA7266C9F2B551", signature, "Image hash");
        }
    }
}