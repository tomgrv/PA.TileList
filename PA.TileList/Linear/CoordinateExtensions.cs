using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace PA.TileList.Linear
{
    public static class CoordinateExtensions
    {


        public static string ToString<T>(this T c)
            where T : ICoordinate
        {
            return c.X + "," + c.Y;
        }

        public static Coordinate GetCoordinate<T>(this T c)
           where T : ICoordinate
        {
            return new Coordinate(c.X, c.Y);
        }

        #region Orientation

        public enum Orientation
        {
            ClockWise = 1,
            Collinear = 0,
            CounterClockWise = -1
        }

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

        #endregion
    }


}
