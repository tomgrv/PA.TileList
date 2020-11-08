using PA.TileList.Quantified;
using System;
using System.Xml.Serialization;

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
        public SelectionConfiguration(SelectionPosition selectionType, float tolerance, bool useFullSurface = true)
        {
            if ((tolerance < 0f) || (tolerance > 1f))
                throw new ArgumentOutOfRangeException(nameof(tolerance), tolerance, "Must be a percentage");

            // Set Variables	
            this.UseFullSurface = useFullSurface;
            this.SelectionType = selectionType;

            // Init
            this.SetResolution(tolerance);
        }


        /// <summary>
        ///     Define SelectionConfiguration with automatic resolution based on tolerance
        /// </summary>
        /// <param name="selectionType"></param>
        public SelectionConfiguration(SelectionPosition selectionType, bool useFullSurface = true)
        {
            // Set Variables		
            this.UseFullSurface = useFullSurface;
            this.SelectionType = selectionType;

            // Init
            this.SetResolution(1f);
        }


        /// <summary>
        ///     Define SelectionConfiguration with automatic resolution based on tolerance
        /// </summary>
        /// <param name="selectionType"></param>
        private SelectionConfiguration(SelectionPosition selectionType, int resolutionX, int resolutionY , bool useFullSurface = true)
        {
            // Set Variables		
            this.UseFullSurface = useFullSurface;
            this.SelectionType = selectionType;
            this.ResolutionX = resolutionX;
            this.ResolutionY = resolutionY;
        }

        /// <summary>
        ///     Percentage of surface considered (1f = 100% = all surface)
        /// </summary>
        public float Tolerance { get; private set; }


        /// <summary>
        ///     Number of Point required for under
        /// </summary>
        public float MinSurface { get; private set; }

        /// <summary>
        ///     Number Of Points required for inside
        /// </summary>
        public int MaxSurface { get; private set; }

        /// <summary>
        ///     Nb of calc steps (dots per T on X)
        /// </summary>
        public int ResolutionX { get; protected set; }

        /// <summary>
        ///     Nb of calc steps (dots per T on Y)
        /// </summary>
        public int ResolutionY { get; protected set; }

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


        private SelectionConfiguration _quick;

        public SelectionConfiguration GetQuickSelectionVariant()
        {
            if (this._quick == null)
                this._quick = new SelectionConfiguration(this.SelectionType,1f, this.UseFullSurface);

            return this._quick;
        }




        public void SetResolution(float tolerance)
        {
            if ((tolerance < 0f) || (tolerance > 1f))
                throw new ArgumentOutOfRangeException(nameof(tolerance), tolerance, "Must be a percentage");

            // Save
            this.Tolerance = tolerance;

            // Resolution
            var r = (int)Math.Pow(10, BitConverter.GetBytes(decimal.GetBits((decimal)tolerance)[3])[2]);

            // Save X Y
            this.ResolutionX = this.ResolutionY = Math.Max(2, r);

            // Surface
            this.OptimizeSurface();
        }

        public float GetSurfacePercent(int points)
        {
            return points / this.MaxSurface;
        }

        public void OptimizeResolution(IQuantifiedTile tile, ISelectionProfile profile, bool isoXY = false)
        {
            profile.OptimizeProfile();

            // Ratio vs profile
            var rX = tile.ElementStepX / profile.GranularityX;
            var rY = tile.ElementStepY / profile.GranularityY;

            if (rX > 2 || rY > 2)
                this._quick = new SelectionConfiguration(this.SelectionType, Math.Max((int) rX, 2),  Math.Max((int) rY, 2), this.UseFullSurface);

            // Save X Y
            this.ResolutionX = Math.Max(this.ResolutionX, (int)rX);
            this.ResolutionY = Math.Max(this.ResolutionY, (int)rY);

            if (isoXY)
            {
                this.ResolutionX = this.ResolutionY = Math.Max(this.ResolutionX, this.ResolutionY);
            }

            // Surface
            this.OptimizeSurface();
        }

        public void OptimizeSurface()
        {
            this.MaxSurface = this.ResolutionX * this.ResolutionY;
            this.MinSurface = this.Tolerance * this.MaxSurface;
        }



    }
}