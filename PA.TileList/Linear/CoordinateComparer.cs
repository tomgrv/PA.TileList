using System.Collections.Generic;

namespace PA.TileList.Linear
{
    public class CoordinateComparer<T> : IEqualityComparer<T>
        where T : ICoordinate
    {
        public bool Equals(T a, T b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public int GetHashCode(T c)
        {
            return c.X ^ c.Y;
        }
    }
}