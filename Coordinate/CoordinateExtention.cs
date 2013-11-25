using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace PA.TileList
{
    public static class CoordinateExtention
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
    }

   
}
