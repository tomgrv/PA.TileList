using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PA.TileList.Tests.Utils
{
    static class Signature
    {
        private static readonly Object locker = new Object();

        public static string GetMD5Hash<T>(this T value)
        {
            return ComputeHash(value.ObjectToByteArray() ?? value.Hash());
        }

        #region Serialisable

        private static string ComputeHash(byte[] objectAsBytes)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            try
            {
                byte[] result = md5.ComputeHash(objectAsBytes);

                // Build the final string by converting each byte
                // into hex and appending it to a StringBuilder
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    sb.Append(result[i].ToString("X2"));
                }

                // And return it
                return sb.ToString();
            }
            catch (ArgumentNullException ane)
            {
                //If something occurred during serialization, 
                //this method is called with a null argument. 
                Console.WriteLine("Hash has not been generated.");
                return null;
            }
        }

            private static byte[] ObjectToByteArray<T>(this T objectToSerialize)
        {
            MemoryStream fs = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                //Here's the core functionality! One Line!
                //To be thread-safe we lock the object
                lock (locker)
                {
                    formatter.Serialize(fs, objectToSerialize);
                }
                return fs.ToArray();
            }
            catch (SerializationException se)
            {
                Console.WriteLine("Error occurred during serialization. Message: " +
                se.Message);
                return null;
            }
            finally
            {
                fs.Close();
            }
        }

        #endregion

        #region Parse

        public static byte[] Hash<T>(this T entity)
        {
            var seen = new HashSet<object>();
            var properties = GetAllSimpleProperties(entity, seen).ToList();

            var prop_array = properties.Select(p => ObjectToByteArray(p).AsEnumerable()).Aggregate((ag, next) => ag.Concat(next)).ToArray();
            var item_array = entity is IEnumerable ? (entity as IEnumerable).Cast<object>().Select(p => p.Hash().AsEnumerable()).Aggregate((ag, next) => ag.Concat(next)).ToArray() : new byte[] {};

            return prop_array.Concat(item_array).ToArray();
        }


        private static IEnumerable<object> GetAllSimpleProperties<T>(this T entity, HashSet<object> seen)
        {
            foreach (var property in PropertiesOf<T>.All(entity))
            {
                if (property is int || property is long || property is string ) yield return property;
                else if (seen.Add(property)) // Handle cyclic references
                {
                    foreach (var simple in GetAllSimpleProperties(property, seen)) yield return simple;
                }
            }
        }

            private static class ItemsOf<T>
        {
            private static readonly List<Func<T, dynamic>> Properties = new List<Func<T, dynamic>>();

            static ItemsOf()
            {
                foreach (var property in typeof(T).GetProperties())
                {
                    var getMethod = property.GetGetMethod();
                    var function = (Func<T, dynamic>)Delegate.CreateDelegate(typeof(Func<T, dynamic>), getMethod);
                    Properties.Add(function);
                }
            }

            public static IEnumerable<dynamic> All(T entity)
            {
                return Properties.Select(p => p(entity)).Where(v => v != null);
            }
        }

        private static class PropertiesOf<T>
        {
            private static readonly List<Func<T, dynamic>> Properties = new List<Func<T, dynamic>>();

            static PropertiesOf()
            {
                foreach (var property in typeof(T).GetProperties())
                {
                    var getMethod = property.GetGetMethod();
                    var function = (Func<T, dynamic>)Delegate.CreateDelegate(typeof(Func<T, dynamic>), getMethod);
                    Properties.Add(function);
                }
            }

            public static IEnumerable<dynamic> All(T entity)
            {
                return Properties.Select(p => p(entity)).Where(v => v != null);
            }
        }

        #endregion
    }
}
