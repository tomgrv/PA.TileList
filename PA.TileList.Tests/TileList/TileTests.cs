using System;
using PA.TileList;
using PA.TileList.Extensions;
using PA.TileList.Drawing.Graphics2D;
using PA.TileList.Quadrant;
using System.Drawing;
using PA.TileList.Quantified;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using PA.TileList.Cropping;
using PA.TileList.Tile;

namespace PA.TileList.Tests
{
    [TestFixtureAttribute]
    public class TileTests
    {
        #region Definitions

        internal class MainTile : Tile<SubTile>, IQuadrant<SubTile>, IQuantifiedTile<SubTile>, ITile<SubTile>
        {
            public MainTile(IZone a, SubTile t)
                : base(a, t)
            {
            }

            public Quadrant.Quadrant Quadrant { get; private set; }

            public void SetQuadrant(Quadrant.Quadrant q)
            {
                throw new NotImplementedException();
            }

            public double ElementSizeX { get; internal set; }

            public double ElementSizeY { get; internal set; }

            public double ElementStepX { get; internal set; }

            public double ElementStepY { get; internal set; }

            public double RefOffsetX { get; internal set; }

            public double RefOffsetY { get; internal set; }


        }

        internal class SubTile : Tile<Item>, IQuadrant<Item>
        {

            public SubTile(IZone a, Item t)
                : base(a, t)
            {
            }

            public SubTile(SubTile t)
                : base(t)
            {
            }

            public SubTile(IEnumerable<Item> t, int referenceIndex = 0)
                : base(t, referenceIndex)
            {
            }

            public Quadrant.Quadrant Quadrant { get; private set; }

            public void SetQuadrant(Quadrant.Quadrant q)
            {
                throw new NotImplementedException();
            }

        }

        internal class Item : Coordinate
        {
            public Color Color { get; set; }

            public Item(int x, int y, Color c)
                : base(x, y)
            {
                this.Color = c;
            }

            public Bitmap ToBitmap(int w, int h, string s)
            {
                Bitmap b = new Bitmap(w, h);

                using (Graphics g = Graphics.FromImage(b))
                {
                    g.DrawRectangle(Pens.Pink, 0, 0, w - 1, h - 1);
                    g.FillRectangle(new SolidBrush(this.Color), 1, 1, w - 2, h - 2);
                    g.DrawString(s, new Font(FontFamily.GenericSansSerif, (float)w / 3f), Brushes.Gray, 0, 0);
                }

                return b;
            }

        }

        internal static MainTile GetTile(float factor)
        {
            IZone first = new Zone((int)(-5 * factor), (int)(-5 * factor), (int)(5 * factor), (int)(5 * factor));
            IZone second = new Zone(1, 1, 5, 5);

            SubTile t1 = new SubTile(second, new Item(3, 3, Color.Green));
            t1.Fill(c => new Item(c.X, c.Y, c.X + c.Y == 6 ? Color.Green : Color.Yellow));

            MainTile t0 = new MainTile(first, t1);
            t0.Fill(c => t1.Clone(c.X, c.Y) as SubTile);

            t0.ElementSizeX = 50f / factor * second.SizeX;
            t0.ElementSizeY = 50f / factor * second.SizeY;
            t0.ElementStepX = 55f / factor * second.SizeX;
            t0.ElementStepY = 60f / factor * second.SizeY;
            t0.RefOffsetX = 50;
            t0.RefOffsetY = 0;

            return t0;
        }

        internal static MainTile GetTileFullSpace(float factor)
        {
            IZone first = new Zone((int)(-5 * factor), (int)(-5 * factor), (int)(5 * factor), (int)(5 * factor));
            IZone second = new Zone(1, 1, 5, 5);

            SubTile t1 = new SubTile(second, new Item(3, 3, Color.Green));
            t1.Fill(c => new Item(c.X, c.Y, c.X + c.Y == 6 ? Color.Green : Color.Yellow));

            MainTile t0 = new MainTile(first, t1);
            t0.Fill(c => t1.Clone(c.X, c.Y) as SubTile);

            t0.ElementSizeX = 50f / factor * second.SizeX;
            t0.ElementSizeY = 50f / factor * second.SizeY;
            t0.ElementStepX = 50f / factor * second.SizeX;
            t0.ElementStepY = 50f / factor * second.SizeY;
            t0.RefOffsetX = 50;
            t0.RefOffsetY = 0;

            return t0;
        }

        #endregion

        [Test, Category("Image hash")]
        public void Crop()
        {
            Tile<Item> t0 = new Tile<Item>(new Zone(0, 0, 100, 100), new Item(0, 0, Color.Red));

            t0.Fill(c => c.X > 25 && c.X < 75 && c.Y > 30 && c.Y < 60 ? new Item(c.X, c.Y, c.X == c.Y ? Color.Yellow : Color.Green) : new Item(c.X, c.Y, Color.Red));



            var q0 = t0.AsQuantified();

            var c0 = t0.GetChecksum(i => i.Color.Name);
            var s0 = q0.GetImage(1000, 1000, (z, s) => z.ToBitmap(100, 50, z.X + "\n" + z.Y)).Item.GetSignature();


            Assert.AreEqual("BFE39DA3858C0A979B54F99442B397DA", s0, "Image hash");
            Assert.AreEqual("0,0;100,100", t0.GetZone().ToString(), "Area");

            IEnumerable<Item> crop1 = t0.Crop(new Zone(25, 30, 75, 60));
            Tile<Item> t1 = new Tile<Item>(crop1);

            string signature1 = t1.AsQuantified().GetImage(1000, 1000, (z, s) =>
                z.ToBitmap(100, 50, z.X + "\n" + z.Y)).Item.GetSignature();

            Assert.AreEqual("742D809F5440028ED7F86072C4FC2FA9", signature1, "Image hash");
            Assert.AreEqual("25,30;75,60", t1.GetZone().ToString(), "Area");


            IEnumerable<Item> crop2 = t0.Crop(t => t.Color != Color.Yellow);
            Tile<Item> t2 = new Tile<Item>(crop2);

            string signature2 = t2.AsQuantified().GetImage(1000, 1000, (z, s) =>
                z.ToBitmap(100, 50, z.X + "\n" + z.Y)).Item.GetSignature();

            Assert.AreEqual("6A226FC4E4EC36837BA5042FBFE8D923", signature2, "Image hash");
            Assert.AreEqual("31,31;59,59", t2.GetZone().ToString(), "Area");

        }
    }
}
