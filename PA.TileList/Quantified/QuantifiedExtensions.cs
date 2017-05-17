using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using PA.TileList.Linear;
using PA.TileList.Selection;
using PA.TileList.Tile;

namespace PA.TileList.Quantified
{
    public static class QuantifiedExtensions
    {
        public static double GetScaleFactor(this IQuantifiedTile list, double sizeX, double sizeY)
        {
            var ratioX = sizeX / (list.Zone.SizeX * list.ElementStepX);
            var ratioY = sizeY / (list.Zone.SizeY * list.ElementStepY);

            return Math.Round(Math.Min(ratioX, ratioY), 4);
        }

        public static IQuantifiedTile<T> Scale<T>(this IQuantifiedTile<T> list, double scaleFactor)
            where T : class, ICoordinate
        {
            return new QuantifiedTile<T>(list, list.ElementSizeX * scaleFactor, list.ElementSizeY * scaleFactor,
                list.ElementStepX * scaleFactor, list.ElementStepY * scaleFactor, list.RefOffsetX * scaleFactor,
                list.RefOffsetY * scaleFactor);
        }


        /// <summary>
        ///     Get coordinate at specified location.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static ICoordinate GetCoordinateAt<T>(this IQuantifiedTile<T> list, double x, double y)
            where T : ICoordinate
        {
            return list.Zone.FirstOrDefault(c =>
            {
                var points = 0;
                c.GetPoints(list, 2, 2,
                    (xc, yc, xc2, yc2) =>
                        points +=
                            (Math.Abs(xc - x) < list.ElementStepX) && (Math.Abs(yc - y) < list.ElementStepY) ? 1 : 0, true);
                return points == 4;
            });
        }


        /// <summary>
        ///     Groups the elements by comptuting points satisfying predicate
        ///     Each element is divided into pointsInX*pointsInY points, each of them submitted to predicate
        /// </summary>
        public static IEnumerable<IGrouping<float, P>> GroupByPercent<P>(this IQuantifiedTile<P> tile, ISelectionProfile profile,
                    SelectionConfiguration config, bool fullSize = false)
            where P : ICoordinate
        {

            return tile.GroupBy(c =>
            {
                var p = c.CountPoints(tile, profile, config, fullSize);
                if ((p == 0) || (p == config.MaxSurface)) return p;
                return (float)p / config.MaxSurface;
            });
        }

        /// <summary>
        ///     Groups the elements by comptuting points satisfying predicate
        ///     Each element is divided into pointsInX*pointsInY points, each of them submitted to predicate
        /// </summary>
        /// <returns>Elements grouped by comptuting points satisfying predicate</returns>
        /// <param name="tile">Tile.</param>
        public static IEnumerable<IGrouping<int, P>> GroupByPoints<P>(this IQuantifiedTile<P> tile, ISelectionProfile profile,
                    SelectionConfiguration config, bool fullSize = false)
            where P : ICoordinate
        {
            Contract.Requires(profile != null);
            Contract.Requires(config != null);

            return tile.GroupBy(c => c.CountPoints(tile, profile, config, fullSize));
        }


		/// <summary>
		/// Gets the center coordinate
		/// </summary>
		/// <returns>The center coordinate</returns>
		/// <param name="tile">IQuantifiedTile</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static Coordinate GetCenter<T>(this IQuantifiedTile<T> tile)
			where T : ICoordinate
		{
			Coordinate center = tile.Reference.ToCoordinate();

			center.X -= (int)Math.Round(tile.RefOffsetX / tile.ElementStepX, 0);
			center.Y -= (int)Math.Round(tile.RefOffsetY / tile.ElementStepY, 0);

			return center;
		}


        /// <summary>
        ///     Count points of element c that specifies predicate within tile
        /// </summary>
        /// <returns>The points.</returns>
        /// <param name="c">C.</param>
        /// <param name="tile">Tile.</param>
        /// <param name="pointsInX">Points in x.</param>
        /// <param name="pointsInY">Points in y.</param>
        /// <param name="stepSizeX">Step size x.</param>
        /// <param name="stepSizeY">Step size y.</param>
        /// <param name="predicate">Predicate in cartesian coordinates (x,y)</param>
        /// <param name="polarCoordinates">Predicate is expressed in polar coordinates (angle, radius²)</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        [Obsolete]
        private static int CountPoints<T>(this ICoordinate c, IQuantifiedTile<T> tile, int pointsInX, int pointsInY,
            double stepSizeX, double stepSizeY, Func<double, double, bool> predicate, bool polarCoordinates = false)
            where T : ICoordinate
        {
            Contract.Requires(pointsInX > 1);
            Contract.Requires(pointsInY > 1);
            Contract.Requires((0d < stepSizeX) && (stepSizeX <= 1d));
            Contract.Requires((0d < stepSizeY) && (stepSizeY <= 1d));
            Contract.Requires(predicate != null);

            var points = 0;

            var testY = new double[pointsInY];
            var testY2 = new double[pointsInY];

            for (var i = 0; i < pointsInX; i++)
            {
                var testX = (c.X - tile.Reference.X - 0.5f + i * stepSizeX) * tile.ElementStepX + tile.RefOffsetX;
                var testX2 = Math.Pow(testX, 2d);

                for (var j = 0; j < pointsInY; j++)
                {
                    if (i == 0)
                    {
                        testY[j] = (c.Y - tile.Reference.Y - 0.5f + j * stepSizeY) * tile.ElementStepY + tile.RefOffsetY;
                        testY2[j] = Math.Pow(testY[j], 2);
                    }

                    if (polarCoordinates)
                        points += predicate(Math.Atan2(testY[j], testX), testX2 + testY2[j]) ? 1 : 0;
                    else
                        points += predicate(testX, testY[j]) ? 1 : 0;
                }
            }

            return points;
        }

        #region ToQuantified

        public static QuantifiedTile<T> ToQuantified<T>(this ITile<T> l)
            where T : class, ICoordinate
        {
            return new QuantifiedTile<T>(l);
        }

        public static QuantifiedTile<T> ToQuantified<T>(this ITile<T> l, double sizeX, double sizeY)
            where T : class, ICoordinate
        {
            return new QuantifiedTile<T>(l, sizeX, sizeY);
        }

        public static QuantifiedTile<T> ToQuantified<T>(this ITile<T> l, double sizeX, double sizeY, double stepX,
            double stepY)
            where T : class, ICoordinate
        {
            return new QuantifiedTile<T>(l, sizeX, sizeY, stepX, stepY);
        }

        public static QuantifiedTile<T> ToQuantified<T>(this ITile<T> l, double sizeX, double sizeY, double stepX,
            double stepY, double refOffsetX, double refOffsetY)
            where T : class, ICoordinate
        {
            return new QuantifiedTile<T>(l, sizeX, sizeY, stepX, stepY, refOffsetX, refOffsetY);
        }

        #endregion

        #region AsQuantified

        public static IQuantifiedTile<T> AsQuantified<T>(this ITile<T> l)
            where T : class, ICoordinate
        {
            Contract.Requires(l is IQuantifiedTile<T>);

            return l as IQuantifiedTile<T> ?? l.ToQuantified();
        }

        #endregion

        #region Crop

        #endregion
    }
}