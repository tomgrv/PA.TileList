using System.Drawing;
using NUnit.Framework;
using PA.TileList.Contextual;
using PA.TileList.Cropping;
using PA.TileList.Drawing.Graphics2D;
using PA.TileList.Drawing.Quantified;
using PA.TileList.Extensions;
using PA.TileList.Quantified;
using PA.TileList.Tests.Utils;
using PA.TileList.Tile;

namespace PA.TileList.Tests
{
    [TestFixture]
    public class TileTests
    {
        [Test]
        [Category("Image hash")]
        public void Crop()
        {
            var t0 = new Tile<Item>(new Zone(0, 0, 100, 100), new Item(0, 0, Color.Red));

            t0.Fill(
                c =>
                    (c.X > 25) && (c.X < 75) && (c.Y > 30) && (c.Y < 60)
                        ? new Item(c.X, c.Y, c.X == c.Y ? Color.Yellow : Color.Green)
                        : new Item(c.X, c.Y, Color.Red));

            var q0 = t0.AsQuantified();

            var c0 = t0.GetChecksum(i => i.Color.Name);
            //  var s0 = q0.GetImage(1000, 1000, (z, s) => z.ToBitmap(100, 50, z.X + "\n" + z.Y)).Item.GetSignature();

            // Assert.AreEqual("BFE39DA3858C0A979B54F99442B397DA", s0, "Image hash");
            Assert.AreEqual("0,0;100,100", t0.GetZone().ToString(), "Area");

            var crop1 = t0.Take(new Zone(25, 30, 75, 60));
            var t1 = new Tile<Item>(crop1);

            //string signature1 = t1.AsQuantified().GetImage(1000, 1000, (z, s) =>
            //    z.ToBitmap(100, 50, z.X + "\n" + z.Y)).Item.GetSignature();

            //Assert.AreEqual("742D809F5440028ED7F86072C4FC2FA9", signature1, "Image hash");
            Assert.AreEqual("25,30;75,60", t1.GetZone().ToString(), "Area");


            var crop2 = t0.Take(t => t.Color != Color.Yellow);
            var t2 = new Tile<Item>(crop2);

            //string signature2 = t2.AsQuantified().GetImage(1000, 1000, (z, s) =>
            //   z.ToBitmap(100, 50, z.X + "\n" + z.Y)).Item.GetSignature();

            //Assert.AreEqual("6A226FC4E4EC36837BA5042FBFE8D923", signature2, "Image hash");
            Assert.AreEqual("31,31;59,59", t2.GetZone().ToString(), "Area");
        }
    }
}