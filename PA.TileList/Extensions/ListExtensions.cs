using System;
using System.Collections.Generic;
using System.Linq;
using PA.TileList.Linear;
using PA.TileList.Cropping;
using PA.TileList.Tile;
using PA.TileList.Quantified;

namespace PA.TileList.Extensions
{
    public static class ListExtensions
    {
        public static IEnumerable<T> WhereOrDefault<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            return predicate != null ? list.Where(predicate) : list.AsEnumerable();
        }

        #region Crop

        #region IEnumerable

        public static IEnumerable<T> Crop<T>(this IEnumerable<T> list, IZone a)
         where T : ICoordinate
        {
            return list.Where(e => a.Contains(e));
        }

        public static IEnumerable<T> Crop<T>(this IEnumerable<T> list, Func<T, bool> predicate)
            where T : ICoordinate
        {
            return list.Crop(list.GetCropZone(predicate));
        }

        #endregion

        #region ITile



        #endregion





        #endregion

        public static string GetChecksum<T>(this IEnumerable<T> list, Func<T, IEnumerable<char>> signature)
         where T : ICoordinate
        {
            int checksum = 0;

            foreach (char c in list.SelectMany(signature))
            {
                checksum = ((checksum + c) * 16) % 251;
            }

            checksum = ((checksum + 'A') * 16) % 251;
            checksum = ((checksum + 'A')) % 251;

            if (checksum != 0)
            {
                checksum = 251 - checksum;
            }

            return (65 + (checksum / 16)).ToString() + (65 + (checksum % 16)).ToString();
        }
    }
}
