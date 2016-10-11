using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PA.Utilities;
using PA.TileList.Quantified;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace PA.TileList.Selection
{
    /// <summary>
    /// Parameters for circular operations
    /// </summary>
    public class SelectionConfiguration
    {


        /// <summary>
        /// Percentage of surface considered (1f = 100% = all surface)
        /// </summary>
        public float Tolerance { get; private set; }

        /// <summary>
        /// Nb of calc steps (dots per T on X)
        /// </summary>
        public int ResolutionX { get; private set; }

        /// <summary>
        /// Nb of calc steps (dots per T on Y)
        /// </summary>
        public int ResolutionY { get; private set; }

        /// <summary>
        /// Number of Point required for under
        /// </summary>
        public float MinSurface { get; private set; }

        /// <summary>
        /// Number Of Points required for inside
        /// </summary>
        public int MaxSurface { get; private set; }

        /// <summary>
        /// Gets the type of the selection.
        /// </summary>
        /// <value>The type of the selection.</value>
        public SelectionPosition SelectionType { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:PA.TileList.Selection.SelectionConfiguration"/> class.
        /// </summary>
        /// <param name="selectionType">Selection type.</param>
        /// <param name="tolerance">Surface of "on profile items", in %, to be inside to be considered "inside profile"  </param>
        public SelectionConfiguration(SelectionPosition selectionType, float tolerance)
        {
            if (tolerance < 0f || tolerance > 1f)
                throw new ArgumentOutOfRangeException(nameof(tolerance), tolerance, "Must be a percentage");
            this.Init(selectionType, tolerance);
        }


        /// <summary>
        /// Define CircularConfiguration with automatic resolution based on tolerance
        /// </summary>
        /// <param name="selectionType"></param>
        public SelectionConfiguration(SelectionPosition selectionType)
        {
            this.Init(selectionType, 1f);
        }


        private void Init(SelectionPosition selectionType, float tolerance)
        {
            this.Tolerance = tolerance;
            this.SelectionType = selectionType;

            // Automatic resolution            
            var factor = this.Tolerance / 10;
            while (!Math.Floor(factor).NearlyEquals(factor))
            {
                factor = factor * 10f;
            }

            // Members
            this.ResolutionX = (int)Math.Round(factor * 10 + 1f, 0);
            this.ResolutionY = (int)Math.Round(factor * 10 + 1f, 0);
            this.MaxSurface = this.ResolutionX * this.ResolutionY;
            this.MinSurface = this.Tolerance * this.MaxSurface;
        }


        [Obsolete("Please use CircularConfiguration(float tolerance, SelectionFlag selectionType) as constructor")]
        public SelectionConfiguration(float tolerance, float resolution, SelectionPosition selectionType)
        {
            if (tolerance < 0f || tolerance > 1f)
                throw new ArgumentOutOfRangeException(nameof(tolerance), tolerance, "Should be a percentage");

            if (resolution < 0f || resolution > 1f)
                throw new ArgumentOutOfRangeException(nameof(resolution), resolution, "Should be a percentage");

            this.Tolerance = tolerance;
            this.SelectionType = selectionType;

            this.ResolutionX = (int)Math.Round(1f / resolution + 1f, 0);
            this.ResolutionY = (int)Math.Round(1f / resolution + 1f, 0);
            this.MaxSurface = this.ResolutionX * this.ResolutionY;
            this.MinSurface = this.Tolerance * this.MaxSurface;
        }

        public float GetSurfacePercent(int points)
        {
            return points / this.MaxSurface;
        }

    }
}
