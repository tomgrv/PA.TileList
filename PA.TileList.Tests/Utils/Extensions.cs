using PA.TileList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;


namespace PA.TileList
{
    public static class TestsExtensions
    {
        public static string GetMD5Hash<U>(this U value, Func<U, string> signature = null)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(signature == null ? value.ToString() : signature(value));
                byte[] hash = md5.ComputeHash(inputBytes);
                return Convert.ToBase64String(hash);
            }
        }


        public static string GetSignature<T>(this IEnumerable<T> list, Func<T, string> signature)
            where T : ICoordinate
        {
            return list
                .Select(c => c.X.ToString() + signature(c) + c.Y.ToString())
                .Aggregate((a, b) => (a + b).GetMD5Hash());

        }
    }
}
