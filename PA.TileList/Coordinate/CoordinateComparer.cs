using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA.TileList
{
    public class CoordinateComparer : IEqualityComparer<ICoordinate>
    {
        public bool Equals(ICoordinate a, ICoordinate b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public int GetHashCode(ICoordinate c)
        {
            return c.X ^ c.Y;
        }
    }
}

