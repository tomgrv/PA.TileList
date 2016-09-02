using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PA.Utilities;

namespace PA.TileList.Geometrics.Circular
{
    /// <summary>
    /// Parameters for circular operations
    /// </summary>
    public class CircularConfiguration
    {
        [Flags]
        public enum SelectionFlag
        {
            Inside = 0x04,
            Outside = 0x02,
            Under = 0x01
        }

        /// <summary>
        /// Calc resolution
        /// </summary>
        public float StepSize { get; private set; }

        /// <summary>
        /// Percentage of surface considered (1f = 100% = all surface)
        /// </summary>
        public float Tolerance { get; private set; }

        /// <summary>
        /// Nb of calc steps (dots per T)
        /// </summary>
        public int Resolution { get; private set; }

        /// <summary>
        /// Number of Point required for under
        /// </summary>
        public float MinSurface { get; private set; }

        /// <summary>
        /// Number Of Points required for inside
        /// </summary>
        public float MaxSurface { get; private set; }

        public SelectionFlag SelectionType { get; private set; }

        /// <summary>
        /// Define CircularConfiguration with automatic resolution based on tolerance
        /// </summary>
        /// <param name="tolerance">Tolerance percentage for selectionType</param>
        /// <param name="selectionType"></param>
        public CircularConfiguration(float tolerance, SelectionFlag selectionType)
        {
            if (tolerance < 0f || tolerance > 1f)
                throw new ArgumentOutOfRangeException("tolerance", "Should be a percentage");

            this.Tolerance = tolerance;
            this.StepSize = 1f;
            this.SelectionType = selectionType;

            // Automatic resolution
            float factor = tolerance;
            while (!Math.Floor(factor).NearlyEquals(factor))
            {
                this.StepSize = this.StepSize / 10f;
                factor = factor * 10f;
            }

            // Members
            this.Resolution = (int)Math.Round(1f / this.StepSize + 1f, 0);
            this.MaxSurface = this.Resolution * this.Resolution;
            this.MinSurface = this.Tolerance * this.MaxSurface;
        }

        [Obsolete("Please use CircularConfiguration(float tolerance, SelectionFlag selectionType) as constructor")]
        public CircularConfiguration(float tolerance, float resolution, SelectionFlag selectionType)
        {
            if (tolerance < 0f || tolerance > 1f)
                throw new ArgumentOutOfRangeException("tolerance", "Should be a percentage");

            if (resolution < 0f || resolution > 1f)
                throw new ArgumentOutOfRangeException("resolution", "Should be a percentage");

            this.Tolerance = tolerance;
            this.StepSize = resolution;
            this.SelectionType = selectionType;

            this.Resolution = (int)Math.Round(1f / this.StepSize + 1f, 0);
            this.MaxSurface = this.Resolution * this.Resolution;
            this.MinSurface = this.Tolerance * this.MaxSurface;
        }

        public float GetSurfacePercent(int points)
        {
            return points / this.MaxSurface;
        }

    }
}
