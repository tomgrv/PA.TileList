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

        public void OptimizeProfile()
        {

        }

        public SelectionPosition Position(double[] x, double[] y)
        {
            return this.Position(x[0], y[0]);
        }

        public double[] GetValuesX(double x)
        {
            return new double[] { x };
        }

        public double[] GetValuesY(double y)
        {
            return new double[] { y };
        }

        public SelectionPosition Position(double x, double y)
        {
            var xr = System.Math.Round(x, 14);
            var yr = System.Math.Round(y, 14);

            if ((xr > this.xMin) && (xr < this.xMax) && (yr > this.yMin) && (yr < this.yMax))
                return SelectionPosition.Inside;

            if ((xr < this.xMin) || (xr > this.xMax) || (yr < this.yMin) || (yr > this.yMax))
                return SelectionPosition.Outside;

            return SelectionPosition.Under;
        }

        public SelectionPosition Position(double x, double y, double x2, double y2)
        {
            return this.Position(x, y);
        }
    }
}