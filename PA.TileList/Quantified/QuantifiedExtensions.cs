using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using PA.TileList.Tile;
using PA.TileList.Linear;
using PA.TileList.Cropping;

namespace PA.TileList.Quantified
{
    public static class QuantifiedExtensions
    {
        public static double GetScaleFactor(this IQuantifiedTile list, double sizeX, double sizeY)
        {
            double ratioX = sizeX / (list.Zone.SizeX * list.ElementStepX);
            double ratioY = sizeY / (list.Zone.SizeY * list.ElementStepY);

            return Math.Round(Math.Min(ratioX, ratioY), 4);
        }

        public static IQuantifiedTile<T> Scale<T>(this IQuantifiedTile<T> list, double scaleFactor)
            where T : class, ICoordinate
        {
            return new QuantifiedTile<T>(list, list.ElementSizeX * scaleFactor, list.ElementSizeY * scaleFactor, list.ElementStepX * scaleFactor, list.ElementStepY * scaleFactor, list.RefOffsetX * scaleFactor, list.RefOffsetY * scaleFactor);
        }




        /// <summary>
        /// Get coordinate at specified location.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static ICoordinate GetCoordinateAt<T>(this IQuantifiedTile<T> list, double x, double y)
             where T : ICoordinate
        {
            return list.Zone.FirstOrDefault(c => c.CountPoints(list, 2, 2, 1d, 1d,
                                                        (xc, yc) => Math.Abs(xc - x) < list.ElementStepX && Math.Abs(yc - y) < list.ElementStepY) == 4);
        }

        /// <summary>
        /// Gets the coordinates within specified points
        /// </summary>
        /// <returns>The coordinates in.</returns>
        /// <param name="list">List.</param>
        /// <param name="x1">The first x value.</param>
        /// <param name="y1">The first y value.</param>
        /// <param name="x2">The second x value.</param>
        /// <param name="y2">The second y value.</param>
        /// <param name="strict">If set to <c>true</c> strict.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        [Obsolete]
        public static IEnumerable<ICoordinate> GetCoordinatesIn<T>(this IQuantifiedTile<T> list, double x1, double y1, double x2, double y2, bool strict = false)
            where T : ICoordinate
        {
            double minX = Math.Min(x1, x2);
            double minY = Math.Min(y1, y2);
            double maxX = Math.Max(x1, x2);
            double maxY = Math.Max(y1, y2);

            int pointsInX = Math.Max(1, (int)Math.Ceiling(list.ElementStepX / (maxX - minX))) + 1;
            int pointsInY = Math.Max(1, (int)Math.Ceiling(list.ElementStepY / (maxY - minY))) + 1;



            return list.Zone.Where(c => c.CountPoints(list, pointsInX, pointsInY,
                                                                    (xc, yc) => xc >= minX && xc <= maxX && yc >= minY && yc <= maxY) >= (strict ? 4 : 1));
        }

        [Obsolete]
        public static IEnumerable<T> Crop<T>(this IQuantifiedTile<T> list, double x1, double y1, double x2, double y2, bool strict = false)
           where T : ICoordinate
        {
            double minX = Math.Min(x1, x2);
            double minY = Math.Min(y1, y2);
            double maxX = Math.Max(x1, x2);
            double maxY = Math.Max(y1, y2);

            return list.Where(c => c.CountPoints(list, 2, 2, 1d, 1d,
                                                        (xc, yc) => xc >= minX && xc <= maxX && yc >= minY && yc <= maxY) >= (strict ? 4 : 1));
        }



        /// <summary>
        /// Groups the elements by comptuting points satisfying predicate
        /// Each element is divided into pointsInX*pointsInY points, each of them submitted to predicate
        /// </summary>
        /// <returns>Elements grouped by comptuting points satisfying preicate</returns>
        /// <param name="tile">Tile.</param>
        /// <param name="resolution">Resolution per element <P> on X. Must be >1</param>
        /// <param name="predicate">Predicate.</param>
        /// <typeparam name="P">The 1st type parameter.</typeparam>
        public static IEnumerable<IGrouping<float, P>> GroupByPercent<P>(this IQuantifiedTile<P> tile, float resolution, Func<double, double, bool> predicate, bool polarCoordinates = false)
            where P : ICoordinate
        {
            Contract.Requires(0 < resolution && resolution < 1);
            Contract.Requires(predicate != null);

            int points = (int)Math.Ceiling(1d / resolution) + 1;
            int area = points * points;

            return tile.GroupBy(c =>
            {
                var p = c.CountPoints(tile, points, points, resolution, resolution, predicate, polarCoordinates);
                if (p == 0 || p == area) return p;
                else return (float)p / area;
            });
        }

        /// <summary>
        /// Groups the elements by comptuting points satisfying predicate
        /// Each element is divided into pointsInX*pointsInY points, each of them submitted to predicate
        /// </summary>
        /// <returns>Elements grouped by comptuting points satisfying predicate</returns>
        /// <param name="tile">Tile.</param>
        /// <param name="pointsInY">Number of computing points per element <P> on X. Must be >1</param>
        /// <param name="pointsInY">Number of computing points per element <P> in Y. Must be >1</param>
        /// <param name="predicate">Predicate.</param>
        /// <typeparam name="P">The 1st type parameter.</typeparam>
        public static IEnumerable<IGrouping<int, P>> GroupByPoints<P>(this IQuantifiedTile<P> tile, int pointsInX, int pointsInY, Func<double, double, bool> predicate, bool polarCoordinates = false)
            where P : ICoordinate
        {
            Contract.Requires(pointsInX > 1);
            Contract.Requires(pointsInY > 1);
            Contract.Requires(predicate != null);

            double stepSizeX = 1d / (pointsInX - 1);
            double stepSizeY = 1d / (pointsInY - 1);

            return tile.GroupBy(c => c.CountPoints(tile, pointsInX, pointsInY, stepSizeX, stepSizeY, predicate, polarCoordinates));
        }


        /// <summary>
        ///  Count points of element c that specifies predicate within tile
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
        private static int CountPoints<T>(this ICoordinate c, IQuantifiedTile<T> tile, int pointsInX, int pointsInY, double stepSizeX, double stepSizeY, Func<double, double, bool> predicate, bool polarCoordinates = false)
            where T : ICoordinate
        {
            Contract.Requires(pointsInX > 1);
            Contract.Requires(pointsInY > 1);
            Contract.Requires(0d < stepSizeX && stepSizeX <= 1d);
            Contract.Requires(0d < stepSizeY && stepSizeY <= 1d);
            Contract.Requires(predicate != null);

            int points = 0;

            double[] testY = new double[pointsInY];
            double[] testY2 = new double[pointsInY];

            for (int i = 0; i < pointsInX; i++)
            {
                double testX = ((c.X - tile.Reference.X) - 0.5f + i * stepSizeX) * tile.ElementStepX + tile.RefOffsetX;
                double testX2 = Math.Pow(testX, 2d);

                for (int j = 0; j < pointsInY; j++)
                {
                    if (i == 0)
                    {
                        testY[j] = ((c.Y - tile.Reference.Y) - 0.5f + j * stepSizeY) * tile.ElementStepY + tile.RefOffsetY;
                        testY2[j] = Math.Pow(testY[j], 2);
                    }

                    if (polarCoordinates)
                    {
                        points += predicate(Math.Atan2(testY[j], testX), testX2 + testY2[j]) ? 1 : 0;
                    }
                    else
                    {
                        points += predicate(testX, testY[j]) ? 1 : 0;
                    }
                }

            }

            return points;
        }

        private static int CountPoints<T>(this ICoordinate c, IQuantifiedTile<T> tile, int pointsInX, int pointsInY, Func<double, double, bool> predicate, bool polarCoordinates = false)
         where T : ICoordinate
        {
            Contract.Requires(pointsInX > 1);
            Contract.Requires(pointsInY > 1);
            Contract.Requires(predicate != null);

            var points = 0;

            c.GetPoints(tile, pointsInX, pointsInY, (xc, yc) => points += predicate(xc,yc) ? 1 : 0, polarCoordinates);

            return points;
        }

        public static void GetPoints<T>(this ICoordinate c, IQuantifiedTile<T> tile, int pointsInX, int pointsInY, Action<double, double> predicate, bool polarCoordinates = false)
        where T : ICoordinate
        {
            Contract.Requires(pointsInX > 1);
            Contract.Requires(pointsInY > 1);
            Contract.Requires(predicate != null);

            var ratioX = tile.ElementSizeX/tile.ElementStepX;
            var ratioY = tile.ElementSizeY/tile.ElementStepY;

            var stepSizeX = ratioX / (pointsInX - 1) ;
            var stepSizeY = ratioY/ (pointsInY - 1);

            var offsetX = ratioX / 2f;
            var offsetY = ratioY / 2f;

            var testY = new double[pointsInY];
            var testY2 = new double[pointsInY];

            for (var i = 0; i < pointsInX; i++)
            {
                var testX = ((c.X - tile.Reference.X) - offsetX + i * stepSizeX) * tile.ElementStepX + tile.RefOffsetX;
                var testX2 = Math.Pow(testX, 2d);

                for (var j = 0; j < pointsInY; j++)
                {
                    if (i == 0)
                    {
                        testY[j] = ((c.Y - tile.Reference.Y) - offsetY + j * stepSizeY) * tile.ElementStepY + tile.RefOffsetY;
                        testY2[j] = Math.Pow(testY[j], 2);
                    }

                    if (polarCoordinates)
                    {
                        predicate(Math.Atan2(testY[j], testX), testX2 + testY2[j]);
                    }
                    else
                    {
                        predicate(testX, testY[j]);
                    }
                }

            }
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

        public static QuantifiedTile<T> ToQuantified<T>(this ITile<T> l, double sizeX, double sizeY, double stepX, double stepY)
            where T : class, ICoordinate
        {
            return new QuantifiedTile<T>(l, sizeX, sizeY, stepX, stepY);
        }

        public static QuantifiedTile<T> ToQuantified<T>(this ITile<T> l, double sizeX, double sizeY, double stepX, double stepY, double refOffsetX, double refOffsetY)
            where T : class, ICoordinate
        {
            return new QuantifiedTile<T>(l, sizeX, sizeY, stepX, stepY, refOffsetX, refOffsetY);
        }

        #endregion

        #region AsQuantified

        public static IQuantifiedTile<T> AsQuantified<T>(this ITile<T> l)
            where T : class, ICoordinate
        {
            return l as IQuantifiedTile<T> ?? l.ToQuantified();
        }

        public static IQuantifiedTile<T> AsQuantified<T>(this ITile<T> l, double sizeX, double sizeY)
            where T : class, ICoordinate
        {
            return l as IQuantifiedTile<T> ?? l.ToQuantified(sizeX, sizeY);
        }

        public static IQuantifiedTile<T> AsQuantified<T>(this ITile<T> l, double sizeX, double sizeY, double stepX, double stepY)
            where T : class, ICoordinate
        {
            return l as IQuantifiedTile<T> ?? l.ToQuantified(sizeX, sizeY, stepX, stepY);
        }

        public static IQuantifiedTile<T> AsQuantified<T>(this ITile<T> l, double sizeX, double sizeY, double stepX, double stepY, double refOffsetX, double refOffsetY)
            where T : class, ICoordinate
        {
            return l as IQuantifiedTile<T> ?? l.ToQuantified(sizeX, sizeY, stepX, stepY, refOffsetX, refOffsetY);
        }

        #endregion

        #region Crop




        #endregion
    }
}
