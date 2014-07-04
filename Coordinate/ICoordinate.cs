using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList
{
    public interface ICoordinate
    {
        int X { get; set; }
        int Y { get; set; }

        ICoordinate Clone();
    }

    
}
