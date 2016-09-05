//
// OrientationExtensions.cs
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
using PA.TileList.Linear;


namespace PA.TileList.Linear
{
    public static class OrientationExtensions
    {
        /// <summary>
        /// Dot Product OA.OB
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static double DistanceTo<T>(this T p, ICoordinate q)
            where T : ICoordinate
        {
            return Math.Sqrt(Math.Pow(q.X - p.X, 2) + Math.Pow(q.Y - p.Y, 2));
        }

        /// <summary>
        /// Dot Product OA.OB
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int DotProduct<T>(this T o, ICoordinate a, ICoordinate b)
        where T : ICoordinate
        {
            return (a.X - o.X) * (b.X - o.X) + (a.Y - o.Y) * (b.Y - o.Y);
        }

        /// <summary>
        /// Gets the orientation.
        /// </summary>
        /// <returns>The orientation.</returns>
        /// <param name="o">O.</param>
        /// <param name="p1">P1.</param>
        /// <param name="p2">P2.</param>
        /// <typeparam name="P">The 1st type parameter.</typeparam>
        public static Orientation GetOrientation<P>(this P o, P p1, P p2)
            where P : ICoordinate
        {
            var a = (p1.X - o.X) * (p2.Y - o.Y) - (p2.X - o.X) * (p1.Y - o.Y);

            if (a < 0)
            {
                return Orientation.ClockWise;
            }

            if (a > 0)
            {
                return Orientation.CounterClockWise;
            }

            return Orientation.Collinear;
        }

        /// <summary>
        /// Determine whether or not OA and OB are Collinear
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool AreCollinear<T>(this T o, ICoordinate a, ICoordinate b)
            where T : ICoordinate
        {
            return o.GetOrientation(a, b) == Orientation.Collinear;
        }
    }
}