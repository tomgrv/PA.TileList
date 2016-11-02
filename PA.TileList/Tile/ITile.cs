using System.Collections;
using System.Collections.Generic;
using PA.TileList.Cropping;
using PA.TileList.Linear;

namespace PA.TileList.Tile
{
    public interface ITile : IList, ICoordinate
    {
        Zone Zone { get; }
        void UpdateZone();

        ICoordinate GetReference();
        void SetReference(ICoordinate reference);
    }

    public interface ITile<T> : IList<T>, ITile
        where T : ICoordinate
    {
        T Reference { get; }
        void SetReference(T reference);
    }
}