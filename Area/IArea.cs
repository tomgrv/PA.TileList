﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace PA.TileList
{
    public interface IArea : IEnumerable<ICoordinate>
    {
        Coordinate Min { get; }
        Coordinate Max { get; }
        ushort SizeX { get; }
        ushort SizeY { get; }

        bool Contains(ICoordinate c);
        bool Contains(IArea b);
        void Offset(ICoordinate c);
        ICoordinate Center();

    }
}
