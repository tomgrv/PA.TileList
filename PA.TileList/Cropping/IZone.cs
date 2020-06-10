using System.Collections.Generic;
using PA.TileList.Linear;

namespace PA.TileList.Cropping
{
    public interface IZone : IEnumerable<Coordinate>
    {
        Coordinate Min { get; }
        Coordinate Max { get; }
        ushort SizeX { get; }
        ushort SizeY { get; }

        bool Contains(ICoordinate c);
        bool Contains(IZone b);
        void Offset(ICoordinate c);
        Coordinate Center();
    }
}