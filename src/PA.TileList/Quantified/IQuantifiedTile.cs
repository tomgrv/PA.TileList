using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList
{
    /// <summary>
    /// Tile quantification elements
    /// </summary>
    public interface IQuantifiedTile : ITile
    {
        /// <summary>
        /// Size on an element, on X 
        /// </summary>
        double ElementSizeX { get; }

        /// <summary>
        /// Size on an element, on Y 
        /// </summary>
        double ElementSizeY { get; }

        /// <summary>
        /// Distance between 2 elements center, on X 
        /// </summary>
        double ElementStepX { get; }

        /// <summary>
        /// Distance between 2 elements center, on Y 
        /// </summary>
        double ElementStepY { get; }

        /// <summary>
        /// Distance from tile center to tile reference center, on X
        /// </summary>
        double RefOffsetX { get; }

        /// <summary>
        /// Distance from tile center to tile reference center, on Y
        /// </summary>
        double RefOffsetY { get; }
    }

    public interface IQuantifiedTile<T> : ITile<T>, IQuantifiedTile
        where T : ICoordinate
    {

    }
}
