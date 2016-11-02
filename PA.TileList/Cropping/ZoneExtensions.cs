using System.Collections.Generic;
using PA.TileList.Linear;
using PA.TileList.Tile;

namespace PA.TileList.Cropping
{
    public static class ZoneExtension
    {
        public static T RefreshZone<T>(this T list)
            where T : ITile
        {
            list.UpdateZone();
            return list;
        }

        public static Zone GetZone<T>(this IEnumerable<T> list)
            where T : ICoordinate
        {
            var zone = new Zone(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);

            foreach (var item in list)
            {
                if (item.X < zone.Min.X)
                    zone.Min.X = item.X;

                if (item.X > zone.Max.X)
                    zone.Max.X = item.X;

                if (item.Y < zone.Min.Y)
                    zone.Min.Y = item.Y;

                if (item.Y > zone.Max.Y)
                    zone.Max.Y = item.Y;
            }

            return zone;
        }
    }
}