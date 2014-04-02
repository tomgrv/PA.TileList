using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList
{
    public interface IQuantifiedTile : ITile
    {
        double ElementSizeX { get; }
        double ElementSizeY { get; }
        double ElementStepX { get; }
        double ElementStepY { get; }
        double RefOffsetX { get; }
        double RefOffsetY { get; }
    }

    public interface IQuantifiedTile<T> : ITile<T>, IQuantifiedTile
        where T : ICoordinate
    {

    }
}
