﻿using System;
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
using PA.TileList.Quantified;
using System.Diagnostics;

namespace PA.TileList.Drawing.Tests.TileList.Extensions
{
    [TestFixture]
    public class CircularTests
    {
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

        [Test]
        [Category("Image hash")]
        public void Profile()
        {
            var p = new CircularProfile(1500);

            var i = p.RenderImage(1000, 1000, new RectangleF(-2000, -2000, 4000, 4000), ScaleMode.STRETCH, new CircularProfileRenderer());

            var signature = i.Item.GetSignature();
            Assert.AreEqual("B1FF0A62F65DD493C2781D6D9FB57C4F588F9B0E767EEBAC6219E01EA5A5DF4D", signature, "Image hash");
        }

        [Test]
        public void ProfileForTest()
        {
            var p = GetTestProfile(1400);

            var i = p.RenderImage(1000, 1000, new RectangleF(-2000, -2000, 4000, 4000), ScaleMode.STRETCH, new CircularProfileRenderer(Pens.Red, Pens.Red, Pens.Pink));

            var signature = i.Item.GetSignature();
            Assert.AreEqual("DAA3296DC2EE2A6682DFFBD8425ED029E34004676D6AB80E67DBB691E85CD2E0", signature, "Image hash");
        }

        [Test]
        [Category("Image hash")]
        public void ProfileWith0()
        {
            var search = new CircularProfile(1000);

            for (var a = 0; a < 4; a++)
            {
                var a0 = -13f * Math.PI / 12f + a * Math.PI / 2f;
                var a1 = -11f * Math.PI / 12f + a * Math.PI / 2f;
                search.AddProfileStep(a0, 0);
                search.AddProfileStep(a1, 1000);
            }

            var signature =
                search.RenderImage(1000, 1000, new RectangleF(-1000, -1000, 2000, 2000), ScaleMode.STRETCH, new CircularProfileRenderer())
                    .Item.GetSignature();
            Assert.AreEqual("98AE8580E2596469A774C97BEE234564E96281C519BFFED621FBB8CC2A63F6D8", signature, "Image hash");
        }

        [Test]
        [Category("Image hash")]
        public void ProfileWithFlat()
        {
            var search = new CircularProfile(1000);

            search.AddProfileFlatByLength(0, 1500);
            search.AddProfileFlatByLength(Math.PI, 500, 0.0001, 500);
            search.AddProfileFlat(Math.PI / 2f, 200, 1000);
            search.AddProfileStep(-Math.PI / 4, 1000);
            search.AddProfileStep(-3 * Math.PI / 4, 800);

            var signature =
                search.RenderImage(1000, 1000, new RectangleF(-1000, -1000, 2000, 2000), ScaleMode.STRETCH, new CircularProfileRenderer())
                    .Item.GetSignature();
            Assert.AreEqual("EED4365394FDB98CE5A4566244C50FA9925A28F54F8561533295FAC5E4B91FE4", signature, "Image hash");
        }

        [Test]
        [Category("Image hash")]
        public void SelectionInsideProfile()
        {
            const float factor = 1f;



            var tile = MainTile.GetTile(factor)
                .Flatten<SubTile, Item>();

            tile.Reference.Context.Color = Color.Violet;

            Assert.AreEqual(3025, tile.Count(), "Initial item count");

            var p = GetTestProfile(1400);

            var change = tile.Reference;

            var q = tile.Filter(p, new SelectionConfiguration(SelectionPosition.Inside, true));

            q.Reference.Context.Color = Color.Pink;

            var pi = p.RenderImage(5000, 5000, ScaleMode.STRETCH, new CircularProfileRenderer(Pens.Red, Pens.Red, Pens.Pink));
            //var i = q.RenderImage(pi, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(s.Width, s.Height, z.X + "\n" + z.Y), Pens.Blue));
            q.DrawImage(pi, new QuantifiedRenderer<IContextual<Item>>((z, g, m) => z.Context.Draw(g, z.X + "\n" + z.Y), Pens.Blue));

            // var signature1 = pi.Item.GetSignature();
            var signature2 = pi.Item.GetSignature();

            Assert.AreEqual(true, tile.Reference.GetHashCode() == q.Reference.GetHashCode(), "HashCode Changed");
            Assert.AreEqual(q.Reference, change, "Reference Changed");
            Assert.IsNotNull(q.Reference, "Reference is null");
            Assert.AreEqual(1819, q.Count(), "Selected item count");

            //      Assert.AreEqual("ADE22DBF99F378AEE20F993BF51705756AFFF2539CA8D6CC5CCA7266C9F2B551", signature, "Image hash");
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

            var q = tile.Filter(p, new SelectionConfiguration(SelectionPosition.Inside, true));

            //var i = q.GetImage(5000, 5000, (z, s) => z.Context.ToBitmap(50, 50, z.X + "\n" + z.Y));
            //var pi = p.GetImage(i);
            //string signature = pi.Item.GetSignature();

            Assert.AreEqual(q.Reference, change, "Reference Changed");
            Assert.IsNotNull(q.Reference, "Reference is null");


            foreach (var tt in tile.Except(q))
                tt.Context.Color = Color.Transparent;

            // Assert.AreEqual(23467, q.Count(), "Selected item count");

            var pi = p.RenderImage(5000, 5000, tile.GetBounds(), ScaleMode.STRETCH, new CircularProfileRenderer(Pens.Red, Pens.Red, Pens.Pink));
            //var i = tile.RenderImage(pi, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(50, 50, z.X + "\n" + z.Y), Pens.Blue));
           tile.DrawImage(pi, new QuantifiedRenderer<IContextual<Item>>((z, g, m) => z.Context.Draw(g, z.X + "\n" + z.Y), Pens.Blue));
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

            var q = tile.Filter(p, new SelectionConfiguration(SelectionPosition.Under, true));

            //Assert.AreNotEqual(change, q.Reference, "Reference Changed");
            //Assert.IsNotNull(q.Reference, "Reference is null");

            q.Reference.Context.Color = Color.Pink;

            var i = q.RenderImage(5000, 2000, new RectangleF(-2000, -2000, 4000, 4000), ScaleMode.XYRATIO | ScaleMode.STRETCH, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(50, 50, z.X + "\n" + z.Y), Pens.Red, Pens.Blue));
            
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

            var q = tile.Filter(p, new SelectionConfiguration(SelectionPosition.Outside, true));

            Assert.IsNotNull(q.Reference, "Reference is null");
            Assert.AreNotEqual(q.Reference, change, "Reference Changed");

            q.Reference.Context.Color = Color.Pink;

            Assert.IsTrue(q.Contains(q.Reference), "Reference is contained");
            // Assert.AreEqual(change, q.Reference, "test");

            var pi = p.RenderImage(5000, 5000, new RectangleF(-2000, -2000, 4000, 4000), ScaleMode.STRETCH, new CircularProfileRenderer(Pens.Red, Pens.Red, Pens.Pink));
           q.DrawImage(pi, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(50, 50, z.X + "\n" + z.Y), Pens.Red));


            var signature = pi.Item.GetSignature();
            //
            Assert.AreEqual(960, q.Count(), "Selected item count");

            //    Assert.AreEqual("ADE22DBF99F378AEE20F993BF51705756AFFF2539CA8D6CC5CCA7266C9F2B551", signature, "Image hash");
        }
    }
}