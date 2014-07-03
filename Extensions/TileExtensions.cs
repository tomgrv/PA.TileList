using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList.Extensions
{
    public static class TileExtensions
    {
        public static ITile<T> ToTile<T>(this IEnumerable<T> c, int referenceIndex = 0)
          where T : ICoordinate
        {
            return new Tile<T>(c.GetArea(), c, referenceIndex);
        }

        public static IEnumerable<T> Crop<T>(this ITile<T> list, Func<T, bool> predicate)
          where T : ICoordinate
        {
            // Crop area
            Area crop = list.GetArea();

            // Reduce on x increasing
            while (list.Where(c => c.X == crop.Min.X).All(predicate))
            {
                crop.Min.X++;
            }

            // Reduce on x decreasing
            while (list.Where(c => c.X == crop.Max.X).All(predicate))
            {
                crop.Max.X--;
            }

            // Reduce on y increasing
            while (list.Where(c => c.Y == crop.Min.Y).All(predicate))
            {
                crop.Min.Y++;
            }

            // Reduce on y decreasing
            while (list.Where(c => c.Y == crop.Max.Y).All(predicate))
            {
                crop.Max.Y--;
            }

            return list.Crop(crop);
        }

        public static IEnumerable<T> Crop<T>(this ITile<T> list, IArea a)
           where T : ICoordinate
        {
            return list.Where(c => (c.X >= a.Min.X && c.X <= a.Max.X && c.Y >= a.Min.Y && c.Y <= a.Max.Y));
        }
    }
}
