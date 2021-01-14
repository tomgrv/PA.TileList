using System.Collections.Generic;
using System.Diagnostics.Contracts;
using PA.TileList.Linear;
using PA.TileList.Tile;

namespace PA.TileList.Quantified
{
    public class QuantifiedTile<T> : Tile<T>, IQuantifiedTile<T>, ITile<T>
        where T : class, ICoordinate
    {
        public QuantifiedTile(IEnumerable<T> t, int referenceIndex = 0)
            : base(t, referenceIndex)
        {
            ElementSizeX = 1;
            ElementSizeY = 1;
            ElementStepX = 1;
            ElementStepY = 1;
            RefOffsetX = 0;
            RefOffsetY = 0;
        }

        public QuantifiedTile(ITile<T> t)
            : this(t, 1, 1, 1, 1, 0, 0)
        {
        }

        public QuantifiedTile(ITile<T> t, double sizeX, double sizeY)
            : this(t, sizeX, sizeY, sizeX, sizeY, 0, 0)
        {
        }

        public QuantifiedTile(ITile<T> t, double sizeX, double sizeY, double stepX, double stepY)
            : this(t, sizeX, sizeY, stepX, stepY, 0, 0)
        {
        }

        public QuantifiedTile(ITile<T> t, double sizeX, double sizeY, double stepX, double stepY, double offsetX,
            double offsetY)
            : base(t)
        {
            Contract.Requires(stepX > sizeX, nameof(stepX) + " must be greater than " + nameof(sizeX));
            Contract.Requires(stepY > sizeY, nameof(stepY) + " must be greater than " + nameof(sizeY));
            Contract.Requires(sizeX > 0, nameof(sizeX) + " must be positive");
            Contract.Requires(sizeY > 0, nameof(sizeY) + " must be positive");
            Contract.Requires(stepX > 0, nameof(stepX) + " must be positive");
            Contract.Requires(stepY > 0, nameof(stepY) + " must be positive");

            ElementSizeX = sizeX;
            ElementSizeY = sizeY;
            ElementStepX = stepX;
            ElementStepY = stepY;
            RefOffsetX = offsetX;
            RefOffsetY = offsetY;
        }

        public QuantifiedTile(IQuantifiedTile<T> t)
            : this(t, t.ElementSizeX, t.ElementSizeY, t.ElementStepX, t.ElementStepY, t.RefOffsetX, t.RefOffsetY)
        {
        }

        public double ElementSizeX { get; }
        public double ElementSizeY { get; }
        public double ElementStepX { get; }
        public double ElementStepY { get; }
        public double RefOffsetX { get; private set; }
        public double RefOffsetY { get; private set; }

        public override void SetReference(T reference)
        {
            if (Contains(reference))
            {
                var n = new Coordinate(reference.X - Reference.X, reference.Y - Reference.Y);

                RefOffsetX += n.X * ElementStepX;
                RefOffsetY += n.Y * ElementStepY;

                base.SetReference(reference);
            }
        }

        public override object Clone()
        {
            return new QuantifiedTile<T>((Tile<T>) base.Clone(), ElementSizeX, ElementSizeY, ElementStepX, ElementStepY,
                RefOffsetX, RefOffsetY);
        }

        public override object Clone(int x, int y)
        {
            return new QuantifiedTile<T>((Tile<T>) base.Clone(x, y), ElementSizeX, ElementSizeY, ElementStepX,
                ElementStepY, RefOffsetX, RefOffsetY);
        }
    }
}