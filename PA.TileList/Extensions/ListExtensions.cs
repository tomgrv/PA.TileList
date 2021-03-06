﻿using System;
using System.Collections.Generic;
using System.Linq;
using PA.TileList.Linear;

namespace PA.TileList.Extensions
{
    public static class ListExtensions
    {
        public static IEnumerable<T> WhereOrDefault<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            return predicate != null ? list.Where(predicate) : list.AsEnumerable();
        }


        public static string GetChecksum<T>(this IEnumerable<T> list, Func<T, IEnumerable<char>> signature)
            where T : ICoordinate
        {
            var checksum = 0;

            foreach (var c in list.SelectMany(signature))
                checksum = (checksum + c)*16%251;

            checksum = (checksum + 'A')*16%251;
            checksum = (checksum + 'A')%251;

            if (checksum != 0)
                checksum = 251 - checksum;

            return 65 + checksum/16 + (65 + checksum%16).ToString();
        }
    }
}