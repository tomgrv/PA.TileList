
using System.Collections.Generic;

namespace PA.TileList.Quadrant
{
    public interface IQuadrant<T> : IList<T>, ITile where T : ICoordinate
    {
        Quadrant Quadrant { get; }
        void SetQuadrant(Quadrant q);
    }
}
