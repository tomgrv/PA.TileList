using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PA.TileList.Contextual;
using PA.TileList.Linear;
using PA.TileList.Extensions;
using PA.TileList.Quantified;

namespace PA.TileList.Geometrics.Extensions
{
    public static class SegmentExtensions
    {
        /// <summary>
        /// Determine whether or not OA and OB are Collinear
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name = "s1"></param>
        /// <param name = "s2"></param>
        /// <returns></returns>
        //public static bool Intersect<T>(this Segment<T> s1, Segment<T> s2)
        //    where T : ICoordinate
        //{
        //    int t1 = (int)s1.Origin.OrientationTriangle(s1.Point, s2.Point) * (int)s1.Origin.OrientationTriangle(s1.Point, s2.Origin);
        //    int t2 = (int)s2.Origin.OrientationTriangle(s2.Point, s1.Point) * (int)s2.Origin.OrientationTriangle(s2.Point, s1.Origin);

        //    return t1 <= 0 && t2 <= 0;
        //}

        /// <summary>
        /// Determine whether or not OA and OB are Collinear
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name = "s1"></param>
        /// <param name = "s2"></param>
        /// <returns></returns>
        public static bool AreCollinear<T>(this Segment<T> s1, Segment<T> s2)
           where T : ICoordinate
        {
            return s1.Origin.AreCollinear(s1.Point, s2.Vector());
        }

    }
}
