using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PA.TileList.Linear;
using PA.TileList.Zone;

namespace PA.TileList.Tile
{
    public interface ITile : IList, ICoordinate
    {
        IZone Zone { get; }
        void UpdateZone();
    }

    public interface ITile<T> : IList<T>, ITile
        where T : ICoordinate
    {
        T Reference { get; }
        void SetReference(T reference);
        void SetReference(int reference);
    }
}
