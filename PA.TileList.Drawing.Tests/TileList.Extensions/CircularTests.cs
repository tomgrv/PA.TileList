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
        [Category("Visual")]
        public void CircularProfile()
        {
            var pro = new CircularProfile(1500);

            var i = pro.RenderImage(1000, 1000, new RectangleF(-2000, -2000, 4000, 4000), ScaleMode.STRETCH, cpr);

            i.SaveDebugImage();
        }

        [Test]
        [Category("Visual")]
        public void ProfileForTest()
        {
            var p = GetTestProfile(1400);

            var i = p.RenderImage(1000, 1000, new RectangleF(-2000, -2000, 4000, 4000), ScaleMode.STRETCH, cpr);

            i.SaveDebugImage();
        }

        [Test]
        [Category("Visual")]
        public void ProfileWith0()
        {
            var p = GetZeroProfile();

            var i = p.RenderImage(1000, 1000, new RectangleF(-1000, -1000, 2000, 2000), ScaleMode.STRETCH, cpr);

            i.SaveDebugImage();
        }

        [Test]
                [Category("Visual")]
        public void ProfileWithFlat()
        {
            var p = GetFlatProfile();

            var i = p.RenderImage(1000, 1000, new RectangleF(-1000, -1000, 2000, 2000), ScaleMode.STRETCH, cpr);

            i.SaveDebugImage();
        }

        [Test]
        [Category("Trustable")]
        public void SelectionInsideProfile_SmallTile()
        {
            var scs = new SelectionConfiguration(SelectionPosition.Inside, false);
            var pro = GetTestProfile(1400);

            var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
      //      scs.OptimizeResolution(tile, pro);

           

            Assert.AreEqual(3025, tile.Count, "Initial item count");

            var change = tile.Reference;
            var q = tile.Take(pro, scs);

            foreach (var c in q)
            {
                tile.Find(c).Context.Color = Color.Brown;
            }

            tile.Reference.Context.Color = Color.Violet;

            tile.GetDebugGraphic(pro, cpr, scs).SaveDebugImage();

            // TG 13/12/2022
            Assert.AreEqual(739,q.Count(), "Selected item count");

        }

        [Test]
        [Category("Trustable")]
        public void SelectionInsideProfile_MediumTile()
        {
            var scs = new SelectionConfiguration(SelectionPosition.Inside, false);
            var pro = GetTestProfile(1400);

            var tile = MainTile.GetTile(5).Flatten<SubTile, Item>();
         //   scs.OptimizeResolution(tile, pro);

            tile.Reference.Context.Color = Color.Violet;

            Assert.AreEqual(65025, tile.Count, "Initial item count");

            var change = tile.Reference;
            var q = tile.Take(pro, scs);

            foreach (var c in q)
            {
                tile.Find(c).Context.Color = Color.Brown;
            }

            tile.GetDebugGraphic(pro, cpr, scs).SaveDebugImage();

            // TG 13/12/2022
            Assert.AreEqual(739,q.Count(), "Selected item count");
        }

        [Test]
        [Category("Trustable")]
        public void SelectionOnProfile()
        {
            var scs = new SelectionConfiguration(SelectionPosition.Under, 0.1f);
            var pro = GetTestProfile(1400);

            var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
        //    scs.OptimizeResolution(tile, pro);

            tile.Reference.Context.Color = Color.Violet;

            Assert.AreEqual(3025, tile.Count, "Initial item count");

            var change = tile.Reference;
            var q = tile.Take(pro, scs);

            foreach (var c in q)
            {
                tile.Find(c).Context.Color = Color.Brown;
            }

            tile.GetDebugGraphic(pro, cpr, scs).SaveDebugImage();

            // TG 13/12/2022
            Assert.AreEqual(Color.Brown,tile.Find(13,-10).Context.Color, "specific item not selected");
            Assert.AreEqual(Color.Brown,tile.Find(-6,18).Context.Color, "specific item not selected");
            Assert.AreEqual(170,q.Count(), "Selected item count");
        }

        [Test]
        [Category("Image hash")]
        public void SelectionOutsideProfile()
        {
            var scs = new SelectionConfiguration(SelectionPosition.Under, 0.1f);
            var pro = GetTestProfile(1400);
            var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();

            Assert.AreEqual(3025, tile.Count, "Initial item count");

            var change = tile.Reference;

            var q = tile.Keep(pro, new SelectionConfiguration(SelectionPosition.Outside));
            
            q.Reference.Context.Color = Color.Pink;
            q.GetDebugGraphic(pro, cpr, scs).SaveDebugImage();

            // TG 13/12/2022
            Assert.IsNotNull(q.Reference, "Reference is null");
            Assert.AreNotSame(q.Reference, change, "Reference still same");
            Assert.AreEqual(q.Reference.X, -22, "Wrong Reference X ");
            Assert.AreEqual(q.Reference.Y, -22, "Wrong Reference Y");
            Assert.IsTrue(q.Contains(q.Reference), "Reference is not contained");
            Assert.AreEqual(2156, q.Count, "Selected item count");
        }
    }
}