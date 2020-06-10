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
		/// <param name="forceMinResolution">Force minimum 2x2 resolution, whatever the tolerance</param>
        public SelectionConfiguration(SelectionPosition selectionType, float tolerance, bool useFullSurface = true, bool forceMinResolution = false)
        {
            if ((tolerance <= 0f) || (tolerance > 1f))
                throw new ArgumentOutOfRangeException(nameof(tolerance), tolerance, "Must be a percentage");
			
			this.Init(selectionType, tolerance, useFullSurface, forceMinResolution);
        }


        /// <summary>
        ///     Define SelectionConfiguration with automatic resolution based on tolerance
        /// </summary>
        /// <param name="selectionType"></param>
        public SelectionConfiguration(SelectionPosition selectionType, bool useFullSurface = true, bool forceMinResolution = false)
        {
            this.Init(selectionType, 1f, useFullSurface, forceMinResolution );
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

		 
		private void Init(SelectionPosition selectionType, float tolerance, bool useFullSurface, bool forceMinResolution = false)
        {
			// Set Variables		
            this.Tolerance = tolerance;
            this.SelectionType = selectionType;

			// Resolution
			var r = forceMinResolution ? 1 : (int) Math.Pow(10, BitConverter.GetBytes(decimal.GetBits((decimal)tolerance)[3])[2]);

			// Members
			this.ResolutionX = r + 1;
            this.ResolutionY = r + 1;
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