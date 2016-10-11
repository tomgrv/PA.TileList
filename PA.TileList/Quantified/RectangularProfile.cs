using System;
using System.Diagnostics.Contracts;
using PA.TileList.Selection;

namespace PA.TileList.Quantified
{
    public class RectangularProfile : ISelectionProfile
    {
        public double xMin { get; private set; }
        public double yMin { get; private set; }
        public double xMax { get; private set; }
        public double yMax { get; private set; }

        public RectangularProfile(double xMin, double yMin, double xMax, double yMax)
        {
            Contract.Assert(xMin <= xMax && yMin <= yMax, "Values are not coherent");

            this.xMin = xMin;
            this.yMin = yMin;
            this.xMax = xMax;
            this.yMax = yMax;
        }

        public SelectionPosition Position(double x, double y)
        {
            if (x > xMin && x < xMax && y > yMin && y < yMax)
                return SelectionPosition.Inside;

            if (x < xMin || x > xMax || y < yMin || y > yMax)
                return SelectionPosition.Outside;

            return SelectionPosition.Under;
        }

        public SelectionPosition Position(double x, double y, double x2, double y2)
        {
            return this.Position(x, y);
        }
    }
}