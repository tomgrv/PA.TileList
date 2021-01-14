using System.Drawing;
using PA.TileList.Cropping;
using PA.TileList.Tests.Utils;

namespace PA.TileList.Tests.Tiles
{
    internal class BigTile
    {
        public static MainTile GetTile()
        {
            // 3322

            IZone first = new Zone(0, 0, 0, 0);
            IZone second = new Zone(0, 0, 253, 485);

            var i = new Item(95, 465, Color.Yellow);

            var t1 = new SubTile(second, i);
            t1.FillWithItem(Color.Yellow);
            t1.ElementStepX = 600;
            t1.ElementStepY = 300;
            t1.ElementSizeX = 600;
            t1.ElementSizeY = 300;


            var t0 = new MainTile(first, t1);

            t0.RefOffsetX = 0;
            t0.RefOffsetY = 2850;

            return t0;
        }
    }
}