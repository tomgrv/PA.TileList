using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList
{
    public interface ITile : IList, ICoordinate
    {
        IArea Area { get; }
        void UpdateArea();
    }

    public interface ITile<T> : IList<T>, ITile 
        where T : ICoordinate
    {
        T Reference { get; }
        void SetReference(T reference);
        void SetReference(int reference);
    }
}
