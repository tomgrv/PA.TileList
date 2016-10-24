﻿using System;
using System.Drawing;
using NUnit.Framework;
using PA.TileList.Circular;
using PA.TileList.Contextual;
using PA.TileList.Cropping;
using PA.TileList.Drawing.Circular;
using PA.TileList.Drawing.Graphics2D;
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

            var item = t1.ElementAt(t1.GetCoordinateAt(1000, 500));
            item.Context.Color = Color.Red;

            var coord = t1.GetCoordinateAt(1000, 500);

            Assert.AreEqual(item.X, coord.X);
            Assert.AreEqual(item.Y, coord.Y);
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

            var q0 = t0.AsQuantified(10, 10);

            var signature0 = q0.RenderImage(1000, 1000, ScaleMode.NONE, new QuantifiedRenderer<Item>((z, s) =>
                  z.ToBitmap(100, 50, z.X + "\n" + z.Y))).Item.GetSignature();


            foreach (
                var c in
                q0.SelectCoordinates(new RectangularProfile(250, 250, 600, 600),
                    new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under)))
                t0.Find(c).Color = Color.Chocolate;

            var signature1 = q0.RenderImage(1000, 1000, ScaleMode.NONE, new QuantifiedRenderer<Item>((z, s) =>
                   z.ToBitmap(100, 50, z.X + "\n" + z.Y))).Item.GetSignature();

            foreach (
                var c in
                q0.SelectCoordinates(new RectangularProfile(52, 52, 62, 62),
                    new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under)))
                t0.Find(c).Color = Color.White;

            var signature2 = q0.RenderImage(1000, 1000, ScaleMode.NONE, new QuantifiedRenderer<Item>((z, s) =>
                  z.ToBitmap(100, 50, z.X + "\n" + z.Y))).Item.GetSignature();

            foreach (
                var c in
                q0.SelectCoordinates(new RectangularProfile(12, 12, 13, 13),
                    new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under)))
                t0.Find(c).Color = Color.Black;

            var signature3 = q0.RenderImage(1000, 1000, ScaleMode.NONE, new QuantifiedRenderer<Item>((z, s) =>
                  z.ToBitmap(100, 50, z.X + "\n" + z.Y))).Item.GetSignature();
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

            var change = true;
            var q = tile.Take(p, new SelectionConfiguration(SelectionPosition.Inside), ref change, true);

            var i = q.RenderImage(5000, 5000, ScaleMode.ALL, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(50, 50, z.X + "\n" + z.Y),
                Pens.Crimson));

            this.TestCoordinates(tile, r1, i, z => z.Context.Color = Color.Violet, Color.Aqua);
            this.TestCoordinates(tile, r2, i, z => z.Context.Color = Color.Violet, Color.OrangeRed);
            this.TestCoordinates(tile, r3, i, z => z.Context.Color = Color.Violet, Color.Blue);

            var ii = q.RenderImage(i, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(50, 50, z.X + "\n" + z.Y)));

            var pi = p.RenderImage(ii, new CircularProfileRenderer());

            var pj = q.RenderImage(pi, new RulersRenderer<IContextual<Item>>(new[] { 100f, 500f }));

            var signature = pj.Item.GetSignature();
        }

        [Test]
        [Category("Image hash")]
        public void FirstOrDefault()
        {
            var tile = MainTile.GetTile(1);

            var t1 = tile
                .Flatten<SubTile, Item>();

            var item1 = t1.ElementAt(t1.GetCoordinateAt(27.4, 38));
            item1.Context.Color = Color.Red;

            var item2 = t1.ElementAt(t1.GetCoordinateAt(0, 0));
            item2.Context.Color = Color.Blue;

            var i1 = t1.RenderImage(2000, 2000, ScaleMode.NONE, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(100, 50, z.X + "\n" + z.Y)));

            var signature = t1.RenderImage(i1, new RulersRenderer<IContextual<Item>>(new[] { 100f, 500f })).Item.GetSignature();
            Assert.AreEqual("A56EBC8E87772EA73D38342AF45FF00B5489A22DB73E7ED5996C6AF7EEE3DE0A", signature, "Image hash");
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

            var i1 = t1.RenderImage(2000, 2000, ScaleMode.STRETCH, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(100, 100, z.X + "\n" + z.Y)));

            var p = new CircularProfile(1000);

            var i2 = p.RenderImage(i1, new CircularProfileRenderer(null, null, Pens.DarkViolet));

            var signature = t1.RenderImage(i2, new RulersRenderer<IContextual<Item>>(new[] { 100f, 500f })).Item.GetSignature();
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

            var r1 = t1.RenderImage(2000, 2000, ScaleMode.STRETCH, new RulersRenderer<IContextual<Item>>(new[] { 100f, 500f }));

            //            var i1 = t1.RenderImage(r1, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(100, 100, z.X + "\n" + z.Y)));

            var p = new CircularProfile(1000);

            var i2 = p.RenderImage(r1, new CircularProfileRenderer(null, null, Pens.DarkViolet));

            var signature = i2.Item.GetSignature();

            //    var signature = t1.RenderImage(i2, new RulersRenderer<IContextual<Item>>(new[] { 100f, 500f })).Item.GetSignature();
            Assert.AreEqual("9272D2C42A039C2122B649DAD516B390A3A2A3C51BA861B6E615F27BA0F1BDA3", signature, "Image hash");
        }
    }
}