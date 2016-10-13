using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using PA.TileList.Linear;

namespace PA.TileList.Tests.Utils
{
    public static class TestsExtensions
    {
        public static string GetMD5Hash<U>(this U value, Func<U, string> signature = null)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var inputBytes =
                    Encoding.UTF8.GetBytes(signature == null ? value.ToString() : signature(value));
                var hash = md5.ComputeHash(inputBytes);
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