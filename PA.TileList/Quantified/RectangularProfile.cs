using System;
using System.Diagnostics.Contracts;
using PA.TileList.Selection;

namespace PA.TileList.Quantified
{
    public class RectangularProfile : ISelectionProfile
    {
        public RectangularProfile(double xMin, double yMin, double xMax, double yMax, string name = null)
        {
            Contract.Assert(xMin <= xMax && yMin <= yMax, "Values are not coherent");

            this.xMin = xMin;
            this.yMin = yMin;
            this.xMax = xMax;
            this.yMax = yMax;
            Name = name;
            GranularityX = Math.Abs(this.xMax - this.xMin) / 2;
            GranularityY = Math.Abs(this.yMax - this.yMin) / 2;
        }

        public double xMin { get; }
        public double yMin { get; }
        public double xMax { get; }
        public double yMax { get; }
        public string Name { get; }
        public double GranularityX { get; }
        public double GranularityY { get; }

        public void OptimizeProfile()
        {
        }

        public SelectionPosition Position(double[] x, double[] y, bool IsQuickMode = false)
        {
            return Position(x[0], y[0]);
        }

        public double[] GetValuesX(double x)
        {
            return new[] { x };
        }

        public double[] GetValuesY(double y)
        {
            return new[] { y };
        }

        public SelectionPosition Position(double x, double y)
        {
            var xr = Math.Round(x, 14);
            var yr = Math.Round(y, 14);

            if (xr > xMin && xr < xMax && yr > yMin && yr < yMax)
                return SelectionPosition.Inside;

            if (xr < xMin || xr > xMax || yr < yMin || yr > yMax)
                return SelectionPosition.Outside;

            return SelectionPosition.Under;
        }

        public SelectionPosition Position(double x, double y, double x2, double y2)
        {
            return Position(x, y);
        }
    }
}