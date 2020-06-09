using System;
using System.Drawing;
using System.Linq;
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
					new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under, false)))
			{
				//this.DrawPoints(q, r, i, cl);


				var z = q.ElementAt(c);

				if (z != null)
					a(z);
			}


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
			Assert.AreEqual(23, coord2.X);
			Assert.AreEqual(11, coord2.Y);
		}

		[Test]
		public void CoordinatesIn()
		{

			var rr = new RulersRenderer<IContextual<Item>>(new[] { 100f, 500f });
			var rrr = new RectangularRenderer(Color.Black, 1);

			var pro = new RectangularProfile[2]{
				new RectangularProfile(-2000, -1990, -1000, -1000),
				new RectangularProfile(-25, 0, 2000, 112.5)
			};

			for (int k = 0; k < pro.Length; k++)
			{

				var scs = new SelectionConfiguration[9]{
						new SelectionConfiguration(SelectionPosition.Inside , false),
						new SelectionConfiguration(SelectionPosition.Inside |SelectionPosition.Under, false),
						new SelectionConfiguration(SelectionPosition.Under, false),
						new SelectionConfiguration(SelectionPosition.Inside , true),
						new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under, true),
						new SelectionConfiguration(SelectionPosition.Inside , 0.25f, true, true),
						new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under , 0.25f,true, true),
						new SelectionConfiguration(SelectionPosition.Inside , 0.25f, false, true),
						new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under , 0.25f,false, true)

					};

				for (int i = 0; i < scs.Length; i++)
				{
					var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();

					foreach (var c in tile.SelectCoordinates(pro[k], scs[i]))
					{
						tile.Find(c).Context.Color = Color.Chocolate;
					}

					var img = tile.RenderImage(5000, 5000, ScaleMode.STRETCH, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(s, z)))
			 				.Render(tile, rr)
			 				.Render(pro[k], rrr);

					tile.DrawSelectionPoints<IContextual<Item>, Bitmap>(pro[k], scs[i], img, Color.Green, Color.Red, true);

					img.Item.Debug(k.ToString() + "-" + i.ToString());
				}
			}
		}


		[Test]
		[Category("Image hash")]
		public void DrawSelectionPoints()
		{
			const float factor = 1f;

			var tile = MainTile.GetTile(factor).Flatten<SubTile, Item>();

			var p = new CircularProfile(1400);

			var sc = new SelectionConfiguration(SelectionPosition.Inside, 0.70f);

			var i = tile.RenderImage(5000, 5000, ScaleMode.CENTER, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap((int)s.Width, (int)s.Height, z.X + "\n" + z.Y),
																										Pens.Blue, new Pen(Color.Violet, 5)));

			tile.DrawSelectionPoints<IContextual<Item>, Bitmap>(p, sc, i, Color.Green, Color.Red, false);

			p.DrawImage(i, new CircularProfileRenderer(Color.BlueViolet));

			var signature = i.Item.GetSignature();
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

			var i1 = t1.RenderImage(2000, 2000, ScaleMode.STRETCH, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(100, 50, z.X + "\n" + z.Y)));
			t1.DrawImage(i1, new RulersRenderer<IContextual<Item>>(new[] { 100f, 500f }));
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

			var i1 = t1.RenderImage(2000, 2000, ScaleMode.STRETCH, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(100, 100, z.X + "\n" + z.Y)));

			var p = new CircularProfile(1000);

			p.DrawImage(i1, new CircularProfileRenderer(null, null, Pens.DarkViolet));

			t1.DrawImage(i1, new RulersRenderer<IContextual<Item>>(new[] { 100f, 500f }));

			i1.Item.GetSignature();
			//Assert.AreEqual("9272D2C42A039C2122B649DAD516B390A3A2A3C51BA861B6E615F27BA0F1BDA3", i1, "Image hash");
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

			var i1 = t1.RenderImage(2000, 2000, ScaleMode.STRETCH, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(100, 100, z.X + "\n" + z.Y)));

			var p = new CircularProfile(1000);

			p.DrawImage(i1, new CircularProfileRenderer(null, null, Pens.DarkViolet));

			var r = new RectangularProfile(-500, 1000, 500, 1100);



			r.DrawImage(i1, new RectangularRenderer());

			//Assert.AreEqual("9272D2C42A039C2122B649DAD516B390A3A2A3C51BA861B6E615F27BA0F1BDA3", signature, "Image hash");

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

			p.DrawImage(r1, new CircularProfileRenderer(null, null, Pens.DarkViolet));

			var signature = r1.Item.GetSignature();

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

	
						tile.ToBitmap(5000, 5000, new RectangleF(-2000, -2000, 4000, 4000)).Debug("imbricated");

	
					   flat.RenderImage(5000, 5000, new RectangleF(-2000, -2000, 4000, 4000), ScaleMode.STRETCH, new QuantifiedRenderer<IContextual<Item>>(
								(z2, s2) =>
								{
									return z2.Context.ToBitmap((int)s2.Width, (int)s2.Height, z2.X + "\n" + z2.Y);
								})
						).Item.Debug("flattened");

		}
	}
}