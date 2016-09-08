using System;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using PA.TileList.Circular;
using PA.TileList.Contextual;
using PA.TileList.Cropping;
using PA.TileList.Drawing.Circular;
using PA.TileList.Drawing.Graphics2D;
using PA.TileList.Drawing.Quantified;
using PA.TileList.Linear;
using PA.TileList.Quantified;
using PA.TileList.Quadrant;
using PA.TileList.Tests;
using PA.TileList.Tests.Utils;
using PA.TileList.Tile;

namespace PA.TileList.Drawing.Tests
{
    [TestFixture]
    public class QuantifiedTests
    {
        [Test, Category("Image hash")]
        public void FirstOrDefault()
        {
            var tile = MainTile.GetTile(1);

            IQuantifiedTile<IContextual<Item>> t1 = tile
               .Flatten<SubTile, Item>();

            var item1 = t1.ElementAt(27.4, 38);
            item1.Context.Color = Color.Red;

            var item2 = t1.ElementAt(0, 0);
            item2.Context.Color = Color.Blue;

            var i1 = t1.GetImage(2000, 2000, (z, s) => z.Context.ToBitmap(100, 50, z.X + "\n" + z.Y));

            string signature = t1.GetRulers(i1, new float[] { 100f, 500f }).Item.GetSignature();
            Assert.AreEqual("A56EBC8E87772EA73D38342AF45FF00B5489A22DB73E7ED5996C6AF7EEE3DE0A", signature, "Image hash");
        }

        [Test]
        public void Coordinates()
        {
            var tile = MainTile.GetTile(1);

            var t1 = tile
               .Flatten<SubTile, Item>();

            var item = t1.ElementAt(1000, 500);
            item.Context.Color = Color.Red;

            ICoordinate coord = t1.GetCoordinateAt(1000, 500);

            Assert.AreEqual(item.X, coord.X);
            Assert.AreEqual(item.Y, coord.Y);
        }

        [Test]
        public void CoordinatesIn()
        {
            var t0 = new Tile<Item>(new Zone(0, 0, 100, 100), new Item(0, 0, Color.Red));
            t0.Fill(c => c.X > 25 && c.X < 75 && c.Y > 30 && c.Y < 60 ? new Item(c.X, c.Y, c.X == c.Y ? Color.Yellow : Color.Green) : new Item(c.X, c.Y, Color.Red));

            var q0 = t0.AsQuantified(10, 10);

            string signature0 = q0.GetImage(1000, 1000, (z, s) =>
                z.ToBitmap(100, 50, z.X + "\n" + z.Y)).Item.GetSignature();

            foreach (ICoordinate c in q0.GetCoordinatesIn(250, 250, 600, 600))
            {
                t0.Find(c).Color = Color.Blue;
            }

            string signature1 = q0.GetImage(1000, 1000, (z, s) =>
                z.ToBitmap(100, 50, z.X + "\n" + z.Y)).Item.GetSignature();

            foreach (ICoordinate c in q0.GetCoordinatesIn(52, 52, 62, 62))
            {
                t0.Find(c).Color = Color.White;
            }

            string signature2 = q0.GetImage(1000, 1000, (z, s) =>
                z.ToBitmap(100, 50, z.X + "\n" + z.Y)).Item.GetSignature();

            foreach (ICoordinate c in q0.GetCoordinatesIn(12, 12, 13, 13))
            {
                t0.Find(c).Color = Color.Black;
            }

            string signature3 = q0.GetImage(1000, 1000, (z, s) =>
                z.ToBitmap(100, 50, z.X + "\n" + z.Y)).Item.GetSignature();
        }

        [Test, Category("Image hash")]
        public void Rulers()
        {
            var tile = MainTile.GetTile(1);

            var t1 = tile
               .Flatten<SubTile, Item>();

            t1.Reference.Context.Color = Color.Lavender;

            var item = t1.ElementAt(500, 1000);
            //item.Context.Color = Color.Red;

            RectangleD<Bitmap> i1 = t1.GetImage(2000, 2000, (z, s) => z.Context.ToBitmap(100, 100, z.X + "\n" + z.Y));

            CircularProfile p = new CircularProfile(1000);

            RectangleD<Bitmap> i2 = p.GetImage(i1, ScaleMode.ALL, null, null, Pens.DarkOrange);

            string signature = t1.GetRulers(i2, new float[] { 100f, 500f }).Item.GetSignature();
            Assert.AreEqual("9272D2C42A039C2122B649DAD516B390A3A2A3C51BA861B6E615F27BA0F1BDA3", signature, "Image hash");
        }


    }
}


