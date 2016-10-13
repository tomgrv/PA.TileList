using System.Collections.Generic;

namespace PA.TileList.Linear
{
    public class CoordinateComparer : IEqualityComparer<ICoordinate>
    {
        public bool Equals(ICoordinate a, ICoordinate b)
        {
            return (a.X == b.X) && (a.Y == b.Y);
        }

        public int GetHashCode(ICoordinate c)
        {
            return c.X ^ c.Y;
        }
    }
}