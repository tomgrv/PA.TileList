using System.Drawing;
using NUnit.Framework;
using PA.TileList.Contextual;
using PA.TileList.Drawing.Graphics2D;
using PA.TileList.Drawing.Quantified;
using PA.TileList.Quadrant;
using PA.TileList.Quantified;
using PA.TileList.Selection;
using PA.TileList.Tests.Utils;

namespace PA.TileList.Drawing.Tests.TileList.Extensions
{
    [TestFixture]
    public class QuadrantTest
    {

        [Test]
        [Category("Image hash")]
        public void ChangeQuadrantMainTile()
        {
            float factor = 1;

            var tile = MainTile.GetTile(factor);

            tile.Reference.Reference.Color = Color.Blue;

            tile[15].Reference.Color = Color.Pink;

            tile.ToBitmap(5000, 5000, new RectangleF(-2000, -2000, 4000, 4000)).Debug("Init");

            ChangeAndRender(tile, Quadrant.Quadrant.Array).Debug(Quadrant.Quadrant.Array.ToString());

            ChangeAndRender(tile, Quadrant.Quadrant.BottomLeft).Debug(Quadrant.Quadrant.BottomLeft.ToString());

            ChangeAndRender(tile, Quadrant.Quadrant.BottomRight).Debug(Quadrant.Quadrant.BottomRight.ToString());

            ChangeAndRender(tile, Quadrant.Quadrant.TopLeft).Debug(Quadrant.Quadrant.TopLeft.ToString());

            ChangeAndRender(tile, Quadrant.Quadrant.TopRight).Debug(Quadrant.Quadrant.TopRight.ToString());

        }

        private Bitmap ChangeAndRender(MainTile tile, Quadrant.Quadrant q)
        {
            var ntile = tile.ChangeQuadrant(tile.Quadrant, q);

            return ntile.ToQuantified(ntile.ElementStepX, ntile.ElementStepY, ntile.ElementStepX, ntile.ElementStepY, ntile.RefOffsetX, ntile.RefOffsetY)
                               .RenderImage(5000, 5000, new RectangleF(-2000, -2000, 4000, 4000), ScaleMode.STRETCH, new QuantifiedRenderer<IContextual<SubTile>>(
                                   (z, s) => z.Context.ToBitmap((int)s.Width, (int)s.Height, ntile), Pens.Blue, Pens.Red)
                        ).Item;
        }

        [Test]
        [Category("Image hash")]
        public void ChangeQuadrant()
        {
            float factor = 1;

            var tile = MainTile.GetTile(factor);

            var t = tile
                .Flatten<SubTile, Item>();

            var p = CircularTests.GetTestProfile(1400);


            var t1 = t.Filter(p, new SelectionConfiguration(SelectionPosition.Inside, true));


    
                t1.RenderImage(5000, 5000, ScaleMode.STRETCH, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(100, 100, z.X + "\n" + z.Y)))
                    .Item.Debug("TopLeft");
            //Assert.AreEqual("055FBADECFE4D727D083968D6D2AEA62A2312E303FF48635474E11E5525CEEC3", signature1, "TopLeft");

            var t2 = t
                .ChangeQuadrant(Quadrant.Quadrant.TopLeft, Quadrant.Quadrant.BottomLeft)
                .Filter(p, new SelectionConfiguration(SelectionPosition.Inside, true));

     
                t2.RenderImage(5000, 5000, ScaleMode.STRETCH, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(100, 100, z.Context.X + "\n" + z.Context.Y)))
                    .Item.Debug("BottomLeft");
            //Assert.AreEqual("4B02E3B3619367AB0CCE9AB8648B508FE5611B1D1B46BD225AB62A90F014BA0D", signature2, "BottomLeft");

            var t3 = t
                .ChangeQuadrant(Quadrant.Quadrant.TopLeft, Quadrant.Quadrant.BottomRight)
                .Filter(p, new SelectionConfiguration(SelectionPosition.Inside, true));

 
                t3.RenderImage(5000, 5000, ScaleMode.STRETCH, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(100, 100, z.Context.X + "\n" + z.Context.Y)))
                    .Item.Debug("BottomRight");

            // Assert.AreEqual("0ED609DCF12112DCFDDAEC61C32DBEB9874B347C3E5305CA545A5D6795F8DA31", signature3, "BottomRight");

            var t4 = t
                .ChangeQuadrant(Quadrant.Quadrant.TopLeft, Quadrant.Quadrant.TopRight)
                .Filter(p, new SelectionConfiguration(SelectionPosition.Inside, true));

                t4.RenderImage(5000, 5000, ScaleMode.STRETCH, new QuantifiedRenderer<IContextual<Item>>((z, s) => z.Context.ToBitmap(100, 100, z.Context.X + "\n" + z.Context.Y)))
                    .Item.Debug("TopRight");

            // Assert.AreEqual("70CEDF7E06EE71F13DC5844E3ECC5F897501BD8356B3FF6EE60430B23781ECA6", signature4, "TopRight");
        }
    }
}