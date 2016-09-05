
using System.Collections.Generic;
using PA.TileList.Tile;
using PA.TileList.Linear;

namespace PA.TileList.Quadrant
{
    public interface IQuadrant<T> : IList<T>, ITile where T : ICoordinate
    {
        Quadrant Quadrant { get; }
        void SetQuadrant(Quadrant q);
    }
}
