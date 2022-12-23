using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
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

            this.OptimizeProfile();
        }

        public double xMin { get; }
        public double yMin { get; }
        public double xMax { get; }
        public double yMax { get; }
        public string Name { get; }

        private double _maxRadius2;

        private double _minRadius2;

        public void OptimizeProfile()
        {
            double[] r2 = {
                Math.Pow(xMax, 2)+Math.Pow(yMax, 2),
                Math.Pow(xMax, 2)+Math.Pow(yMin, 2),
                Math.Pow(xMin, 2)+Math.Pow(yMax, 2),
                Math.Pow(xMin, 2)+Math.Pow(yMin, 2)
            };

            double[] angles =
            {
                Math.Atan2(yMin, xMin),
                Math.Atan2(yMin, xMax),
                Math.Atan2(yMax, xMin),
                Math.Atan2(yMax, xMax)
            };

            r2 = r2.OrderBy(r => r).ToArray();
            _maxRadius2 = r2.Last();
            _minRadius2 = r2.First();
        }

        public SelectionPosition Position(double[] x, double[] y, bool IsQuickMode = false)
        {
            return Position(x[0], y[0], IsQuickMode);
        }

        public double[] GetValuesX(double x)
        {
            return new[] { x };
        }

        public double[] GetValuesY(double y)
        {
            return new[] { y };
        }

        public SelectionPosition Position(double x, double y, bool IsQuickMode = false)
        {
            return Position(x, y, x * x, y * y, IsQuickMode);
        }

        public SelectionPosition Position(double x, double y, double x2, double y2, bool IsQuickMode = false)
        {
            var xr = Math.Round(x, 14);
            var yr = Math.Round(y, 14);

            var r2 = x2 + y2;

            if (IsQuickMode && r2 > _minRadius2 && r2 < _maxRadius2)
                return SelectionPosition.Under;

            if (xr > xMin && xr < xMax && yr > yMin && yr < yMax)
                return SelectionPosition.Inside;

            if ((xr == xMin || xr == xMax) && yr >= yMin && yr <= yMax)
                return SelectionPosition.Under;

            if (xr >= xMin && xr <= xMax  && ( yr ==yMin || yr == yMax))
                return SelectionPosition.Under;

            return SelectionPosition.Outside;
        }
    }
}