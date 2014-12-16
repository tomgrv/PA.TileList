using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList.Extensions
{
    public static class ListExtensions
    {
        public static IEnumerable<T> WhereOrDefault<T>(this IEnumerable<T> c, Func<T, bool> predicate)
        {
            return predicate is Func<T, bool> ? c.Where(predicate) : c.AsEnumerable();
        }

        #region Crop

        #region IEnumerable

        public static IEnumerable<T> Crop<T>(this IEnumerable<T> list, IArea a)
         where T : ICoordinate
        {
            return list.Where(c => a.Contains(c));
        }

        public static IEnumerable<T> Crop<T>(this IEnumerable<T> list, Func<T, bool> predicate)
            where T : ICoordinate
        {
            return list.Crop(list.GetCropArea(predicate));
        }

        #endregion

        #region ITile

        public static ITile<T> Crop<T>(this ITile<T> list, IArea a)
         where T : ICoordinate
        {
            foreach (var e in list.Where(e => !a.Contains(e)).ToArray())
            {
                list.Remove(e);
            }

            list.UpdateArea();
            return list;
        }

        public static ITile<T> Crop<T>(this ITile<T> list, Func<T, bool> predicate)
            where T : ICoordinate
        {
            return list.Crop(list.GetCropArea(predicate));
        }

        #endregion

        #region IQuantifiedTile

        public static IQuantifiedTile<T> Crop<T>(this IQuantifiedTile<T> list, IArea a)
         where T : ICoordinate
        {
            foreach (var e in list.Where(e => !a.Contains(e)).ToArray())
            {
                list.Remove(e);
            }
            list.UpdateArea();
            return list;
        }

        public static IQuantifiedTile<T> Crop<T>(this IQuantifiedTile<T> list, Func<T, bool> predicate)
            where T : ICoordinate
        {
            return list.Crop(list.GetCropArea(predicate));
        }

        #endregion

        internal static Area GetCropArea<T>(this IEnumerable<T> list, Func<T, bool> predicate)
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

            return crop;
        }

        #endregion
    }
}
