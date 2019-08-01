using System;

namespace PA.TileList.Selection
{
    /// <summary>
    ///     Parameters for circular operations
    /// </summary>
    public class SelectionConfiguration
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:PA.TileList.Selection.SelectionConfiguration" /> class.
        /// </summary>
        /// <param name="selectionType">Selection type.</param>
        /// <param name="tolerance">Surface of "on profile items", in %, to be inside to be considered "inside profile"  </param>
		/// <param name="useFullSurface">Surface considered is full available surface between stepX / stepY</param>
        public SelectionConfiguration(SelectionPosition selectionType, float tolerance, bool useFullSurface = true)
        {
            if ((tolerance <= 0f) || (tolerance > 1f))
                throw new ArgumentOutOfRangeException(nameof(tolerance), tolerance, "Must be a percentage");
			this.Init(selectionType, tolerance, useFullSurface);
        }


        /// <summary>
        ///     Define SelectionConfiguration with automatic resolution based on tolerance
        /// </summary>
        /// <param name="selectionType"></param>
        public SelectionConfiguration(SelectionPosition selectionType, bool useFullSurface = true)
        {
            this.Init(selectionType, 1f, useFullSurface);
        }


        [Obsolete("Please use SelectionConfiguration( SelectionFlag selectionType,float tolerance) as constructor")]
        public SelectionConfiguration(float tolerance, float resolution, SelectionPosition selectionType)
        {
            if ((tolerance <= 0f) || (tolerance > 1f))
                throw new ArgumentOutOfRangeException(nameof(tolerance), tolerance, "Should be a percentage");

            if ((resolution <= 0f) || (resolution > 1f))
                throw new ArgumentOutOfRangeException(nameof(resolution), resolution, "Should be a percentage");

            this.Tolerance = tolerance;
            this.SelectionType = selectionType;

            this.ResolutionX = (int) Math.Round(1f/resolution + 1f, 0);
            this.ResolutionY = (int) Math.Round(1f/resolution + 1f, 0);
            this.MaxSurface = this.ResolutionX*this.ResolutionY;
            this.MinSurface = this.Tolerance*this.MaxSurface;
        }

        /// <summary>
        ///     Percentage of surface considered (1f = 100% = all surface)
        /// </summary>
        public float Tolerance { get; private set; }

        /// <summary>
        ///     Nb of calc steps (dots per T on X)
        /// </summary>
        public int ResolutionX { get; private set; }

        /// <summary>
        ///     Nb of calc steps (dots per T on Y)
        /// </summary>
        public int ResolutionY { get; private set; }

        /// <summary>
        ///     Number of Point required for under
        /// </summary>
        public float MinSurface { get; private set; }

        /// <summary>
        ///     Number Of Points required for inside
        /// </summary>
        public int MaxSurface { get; private set; }

        /// <summary>
        ///     Gets the type of the selection.
        /// </summary>
        /// <value>The type of the selection.</value>
        public SelectionPosition SelectionType { get; private set; }

		/// <summary>
		///     Surface considered is full available surface between stepX / stepY
		/// </summary>
		/// <value>The type of the selection.</value>
		public bool UseFullSurface { get; private set; }


        private void Init(SelectionPosition selectionType, float tolerance, bool useFullSurface)
        {
            this.Tolerance = tolerance;
            this.SelectionType = selectionType;

			var power = BitConverter.GetBytes(decimal.GetBits((decimal)tolerance)[3])[2];
			var resolution = Math.Pow(10, power);

            // Members
            this.ResolutionX = (int) Math.Round( resolution + 1f, 0);
            this.ResolutionY = (int) Math.Round( resolution + 1f, 0);
            this.MaxSurface = this.ResolutionX*this.ResolutionY;
            this.MinSurface = this.Tolerance*this.MaxSurface;
			this.UseFullSurface = useFullSurface;
        }

        public float GetSurfacePercent(int points)
        {
            return points/this.MaxSurface;
        }
    }
}