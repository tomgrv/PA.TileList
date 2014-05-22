using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList
{
    public class QuantifiedTile<T> : Tile<T>, IQuantifiedTile<T>, ITile<T>
        where T : ICoordinate
    {

        public double ElementSizeX { get; private set; }
        public double ElementSizeY { get; private set; }
        public double ElementStepX { get; private set; }
        public double ElementStepY { get; private set; }
        public double RefOffsetX { get; private set; }
        public double RefOffsetY { get; private set; }

        public QuantifiedTile(IQuantifiedTile<T> t)
            : this(t, t.ElementSizeX, t.ElementSizeY, t.ElementStepX, t.ElementStepY, t.RefOffsetX, t.RefOffsetY) { }

        public QuantifiedTile(ITile<T> t)
            : this(t, 1, 1, 1, 1, 0, 0) { }

        public QuantifiedTile(ITile<T> t, double sizeX, double sizeY)
            : this(t, sizeX, sizeY, sizeX, sizeY, 0, 0) { }

        public QuantifiedTile(ITile<T> t, double sizeX, double sizeY, double stepX, double stepY)
            : this(t, sizeX, sizeY, stepX, stepY, 0, 0) { }

        public QuantifiedTile(ITile<T> t, double sizeX, double sizeY, double stepX, double stepY, double offsetX, double offsetY)
            : base(t)
        {
            if (stepX < sizeX || stepY < sizeY)
            {
                throw new ArgumentOutOfRangeException("stepX/stepY", "step must be greater than size for QuantifiedTile");
            }

            this.ElementSizeX = sizeX;
            this.ElementSizeY = sizeY;
            this.ElementStepX = stepX;
            this.ElementStepY = stepY;
            this.RefOffsetX = offsetX;
            this.RefOffsetY = offsetY;
        }

        public new void SetReference(T reference)
        {
            if (this.Contains(reference))
            {
                Coordinate n = new Coordinate(reference.X - this.Reference.X, reference.Y - this.Reference.Y);

                this.RefOffsetX += n.X * this.ElementStepX;
                this.RefOffsetY += n.Y * this.ElementStepY;

                base.SetReference(reference);
            }
        }

        public new void SetReference(int reference)
        {
            this.SetReference(this.ElementAt(reference));
        }
    }
}
