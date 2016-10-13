using System.Collections.Generic;
using PA.TileList.Linear;
using PA.TileList.Tile;

namespace PA.TileList.Quadrant
{
    public interface IQuadrant<T> : IList<T>, ITile where T : ICoordinate
    {
        Quadrant Quadrant { get; }
        void SetQuadrant(Quadrant q);
    }
}