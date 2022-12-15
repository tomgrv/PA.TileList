using System.Drawing;
using System.Linq;
using NUnit.Framework;
using PA.TileList.Contextual;
using PA.TileList.Drawing.Graphics2D;
using PA.TileList.Drawing.Linear;
using PA.TileList.Drawing.Quantified;
using PA.TileList.Linear;
using PA.TileList.Quantified;
using PA.TileList.Selection;
using PA.TileList.Tests.Tiles;
using PA.TileList.Tests.Utils;

namespace PA.TileList.Tests
{
    [TestFixture]
    public class BigTileTests
    {
        [Test(Author = "TG", Description = "Big Tile Selection")]
        [Category("Trustable")]
        public void Selection()
        {
            var bt = BigTile.GetTile();

            var btflat = bt.Flatten<SubTile, Item>()
                .ToQuantified(bt.ElementSizeX / 1000f, bt.ElementSizeY / 1000f, bt.ElementStepX / 1000f,
                    bt.ElementStepY / 1000f, -18.9, 69.600)
                .Translate(TranslateExtensions.TranslateSource.Min);

            // OK 7 juin 2020
            Assert.AreEqual(123444, btflat.Count(), "BigTile Count");


            var crop = new RectangularProfile(-9, 69.45, 9, 72.25);
            var conf = new SelectionConfiguration(SelectionPosition.Inside, 0.25f, false);

            crop.OptimizeProfile();

            foreach (ICoordinate o in btflat.Take(crop, conf))
            {
                var d = btflat.First(c => c.X == o.X && c.Y == o.Y);

                d.Context.Color = Color.Red;
            }

            btflat.GetDebugGraphic(crop, new RectangularRenderer(Color.Black), conf).SaveDebugImage();

            // OK 7 juin 2020
            Assert.AreEqual(300, btflat.Count(c => c.Context.Color == Color.Red), "BigTile Selection Count");
        }
    }
}