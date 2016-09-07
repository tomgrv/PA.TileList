//
// CropExtensions.cs
//
// Author:
//       Thomas GERVAIS <thomas.gervais@gmail.com>
//
// Copyright (c) 2016 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PA.TileList.Tile;
using PA.TileList.Linear;

namespace PA.TileList.Cropping
{
    public static class CroppingExtensions
    {

        internal static IZone GetCroppingZone<T>(this IEnumerable<T> list, Func<T, bool> predicate)
     where T : ICoordinate
        {
            // Crop area
            var crop = list.GetZone();

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

        public static ITile<T> Cropping<T>(this ITile<T> list, IZone a)
      where T : ICoordinate
        {
            foreach (var e in list.Where(e => !a.Contains(e)).ToArray())
            {
                list.Remove(e);
            }

            list.UpdateZone();
            return list;
        }

        public static ITile<T> Cropping<T>(this ITile<T> list, Func<T, bool> predicate)
            where T : ICoordinate
        {
            return list.Cropping(list.GetCroppingZone(predicate));
        }

        public static IEnumerable<T> Cropping<T>(this IEnumerable<T> list, IZone a)
        where T : ICoordinate
        {
            return list.Where(e => a.Contains(e));
        }

        public static IEnumerable<T> Cropping<T>(this IEnumerable<T> list, Func<T, bool> predicate)
            where T : ICoordinate
        {
            return list.Cropping(list.GetCroppingZone(predicate));
        }
    }
}
