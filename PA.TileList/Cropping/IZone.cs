using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using PA.TileList.Linear;
using PA.TileList.Tile;

namespace PA.TileList.Cropping
{
    public interface IZone : IEnumerable<ICoordinate>
    {
        Coordinate Min { get; }
        Coordinate Max { get; }
        ushort SizeX { get; }
        ushort SizeY { get; }

        bool Contains(ICoordinate c);
        bool Contains(IZone b);
        void Offset(ICoordinate c);
        ICoordinate Center();

    }
}
