using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList.Extensions
{
    public static class TileExtensions
    {
        public static Tile<T> ToTile<T>(this IEnumerable<T> c, int referenceIndex = 0)
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
            IEnumerable<T> l1 = list.Where(c => c.X == crop.Min.X);
            while (l1.All(predicate))
            {
                crop.Min.X++;
                l1 = list.Where(c => c.X == crop.Min.X);
            }

            // Reduce on x decreasing
            IEnumerable<T> l2 = list.Where(c => c.X == crop.Max.X);
            while (l2.All(predicate))
            {
                crop.Max.X--;
                l2 = list.Where(c => c.X == crop.Max.X);
            }

            // Reduce on y increasing, limit to x-cropping
            IEnumerable<T> l3 = list.Where(c => c.Y == crop.Min.Y && c.X >= crop.Min.X && c.X <= crop.Max.X);
            while (l3.All(predicate))
            {
                crop.Min.Y++;
                l3 = list.Where(c => c.Y == crop.Min.Y && c.X >= crop.Min.X && c.X <= crop.Max.X);
            }

            // Reduce on y decreasing, limit to x-cropping
            IEnumerable<T> l4 = list.Where(c => c.Y == crop.Max.Y && c.X >= crop.Min.X && c.X <= crop.Max.X);
            while (l4.All(predicate))
            {
                crop.Max.Y--;
                l4 = list.Where(c => c.Y == crop.Max.Y && c.X >= crop.Min.X && c.X <= crop.Max.X);
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
