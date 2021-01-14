using System;
using PA.TileList.Quantified;

namespace PA.TileList.Selection
{
    /// <summary>
    ///     Parameters for circular operations
    /// </summary>
    public class SelectionConfiguration
    {
        private SelectionConfiguration _quick;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:PA.TileList.Selection.SelectionConfiguration" /> class.
        /// </summary>
        /// <param name="selectionType">Selection type.</param>
        /// <param name="tolerance">Surface of "on profile items", in %, to be inside to be considered "inside profile"  </param>
        /// <param name="useFullSurface">Surface considered is full available surface between stepX / stepY</param>
        /// <param name="forceMinResolution">Force minimum 2x2 resolution, whatever the tolerance</param>
        public SelectionConfiguration(SelectionPosition selectionType, float tolerance, bool useFullSurface = true)
        {
            if (tolerance < 0f || tolerance > 1f)
                throw new ArgumentOutOfRangeException(nameof(tolerance), tolerance, "Must be a percentage");

            // Set Variables	
            UseFullSurface = useFullSurface;
            SelectionType = selectionType;

            // Init
            SetResolution(tolerance);
        }


        /// <summary>
        ///     Define SelectionConfiguration with automatic resolution based on tolerance
        /// </summary>
        /// <param name="selectionType"></param>
        public SelectionConfiguration(SelectionPosition selectionType, bool useFullSurface = true)
        {
            // Set Variables		
            UseFullSurface = useFullSurface;
            SelectionType = selectionType;

            // Init
            SetResolution(1f);
        }


        /// <summary>
        ///     Define SelectionConfiguration with automatic resolution based on tolerance
        /// </summary>
        /// <param name="selectionType"></param>
        private SelectionConfiguration(SelectionConfiguration config)
            : this(config.SelectionType, 1f, config.UseFullSurface)
        {
            // Set Variables		
            IsQuick = true;
        }

        /// <summary>
        ///     Define SelectionConfiguration with automatic resolution based on tolerance
        /// </summary>
        /// <param name="selectionType"></param>
        private SelectionConfiguration(SelectionConfiguration config, uint resolutionX, uint resolutionY)
            : this(config.SelectionType, 1f, config.UseFullSurface)
        {
            // Set Variables		
            IsQuick = true;
            ResolutionX = resolutionX;
            ResolutionY = resolutionY;
        }

        /// <summary>
        ///     Percentage of surface considered (1f = 100% = all surface)
        /// </summary>
        public float Tolerance { get; private set; }


        /// <summary>
        ///     Is Quick selection configuration ?
        /// </summary>
        public bool IsQuick { get; }


        /// <summary>
        ///     Number of Point required for under
        /// </summary>
        public float MinSurface { get; private set; }

        /// <summary>
        ///     Number Of Points required for inside
        /// </summary>
        public uint MaxSurface { get; private set; }

        /// <summary>
        ///     Nb of calc steps (dots per T on X)
        /// </summary>
        public uint ResolutionX { get; protected set; }

        /// <summary>
        ///     Nb of calc steps (dots per T on Y)
        /// </summary>
        public uint ResolutionY { get; protected set; }

        /// <summary>
        ///     Gets the type of the selection.
        /// </summary>
        /// <value>The type of the selection.</value>
        public SelectionPosition SelectionType { get; }

        /// <summary>
        ///     Surface considered is full available surface between stepX / stepY
        /// </summary>
        /// <value>The type of the selection.</value>
        public bool UseFullSurface { get; }

        public SelectionConfiguration GetQuickSelectionVariant()
        {
            if (_quick == null)
                _quick = new SelectionConfiguration(this);

            return _quick;
        }

        public bool IsMatching(SelectionPosition selection)
        {
            return (selection & SelectionType) != 0;
        }

        public void SetResolution(float tolerance)
        {
            if (tolerance < 0f || tolerance > 1f)
                throw new ArgumentOutOfRangeException(nameof(tolerance), tolerance, "Must be a percentage");

            // Save
            Tolerance = tolerance;

            // Resolution
            var r = (uint) Math.Pow(10, BitConverter.GetBytes(decimal.GetBits((decimal) tolerance)[3])[2]);

            // Save X Y
            ResolutionX = ResolutionY = Math.Max(2, r);

            // Surface
            OptimizeSurface();
        }

        public float GetSurfacePercent(int points)
        {
            return points / MaxSurface;
        }

        public void OptimizeResolution(IQuantifiedTile tile, ISelectionProfile profile, bool isoXY = false)
        {
            profile.OptimizeProfile();

            // Ratio vs profile
            var rX = tile.ElementStepX / profile.GranularityX;
            var rY = tile.ElementStepY / profile.GranularityY;

            if (rX > 2 || rY > 2)
                _quick = new SelectionConfiguration(this, Math.Max((uint) rX, 2), Math.Max((uint) rY, 2));

            // Save X Y
            ResolutionX = Math.Max(ResolutionX, (uint) rX);
            ResolutionY = Math.Max(ResolutionY, (uint) rY);

            if (isoXY) ResolutionX = ResolutionY = Math.Max(ResolutionX, ResolutionY);

            // Surface
            OptimizeSurface();
        }

        public void OptimizeSurface()
        {
            MaxSurface = ResolutionX * ResolutionY;
            MinSurface = Tolerance * MaxSurface;
        }
    }
}