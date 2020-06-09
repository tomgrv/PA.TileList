using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using PA.TileList.Linear;
using PA.TileList.Tile;

namespace PA.TileList.Tests.Utils
{
    public static class TestsExtensions
    {
        public static string GetSignature<T>(this IEnumerable<T> list, Func<T, string> signature = null)
            where T : Item
        {
            return list.GetMD5Hash();     
        }

        public static void FillWithItem(this Tile<Item> list, Color color)
        {
            list.Fill(c =>
            {
                return new Item(c.X, c.Y, color);
            });
        }
    }
}