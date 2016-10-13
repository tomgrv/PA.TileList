using System.Diagnostics.Contracts;
using PA.TileList.Selection;

namespace PA.TileList.Quantified
{
    public class RectangularProfile : ISelectionProfile
    {
        public RectangularProfile(double xMin, double yMin, double xMax, double yMax)
        {
            Contract.Assert((xMin <= xMax) && (yMin <= yMax), "Values are not coherent");

            this.xMin = xMin;
            this.yMin = yMin;
            this.xMax = xMax;
            this.yMax = yMax;
        }

        public double xMin { get; }
        public double yMin { get; }
        public double xMax { get; }
        public double yMax { get; }

        public SelectionPosition Position(double x, double y)
        {
            if ((x > this.xMin) && (x < this.xMax) && (y > this.yMin) && (y < this.yMax))
                return SelectionPosition.Inside;

            if ((x < this.xMin) || (x > this.xMax) || (y < this.yMin) || (y > this.yMax))
                return SelectionPosition.Outside;

            return SelectionPosition.Under;
        }

        public SelectionPosition Position(double x, double y, double x2, double y2)
        {
            return this.Position(x, y);
        }
    }
}