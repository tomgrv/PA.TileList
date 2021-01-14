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
                            Math.Abs(xc - x) < list.ElementStepX && Math.Abs(yc - y) < list.ElementStepY ? 1 : 0, true);
                return points == 4;
            });
        }


        /// <summary>
        ///     Groups the elements by comptuting points satisfying predicate
        ///     Each element is divided into pointsInX*pointsInY points, each of them submitted to predicate
        /// </summary>
        public static IEnumerable<IGrouping<float, P>> GroupByPercent<P>(this IQuantifiedTile<P> tile,
            ISelectionProfile profile,
            SelectionConfiguration config)
            where P : ICoordinate
        {
            return tile.GroupBy(c =>
            {
                var p = c.Surface(tile, profile, config);
                if (p == 0 || p == config.MaxSurface) return p;
                return (float) p / config.MaxSurface;
            });
        }

        /// <summary>
        ///     Groups the elements by comptuting points satisfying predicate
        ///     Each element is divided into pointsInX*pointsInY points, each of them submitted to predicate
        /// </summary>
        /// <returns>Elements grouped by comptuting points satisfying predicate</returns>
        /// <param name="tile">Tile.</param>
        public static IEnumerable<IGrouping<uint, P>> GroupByPoints<P>(this IQuantifiedTile<P> tile,
            ISelectionProfile profile,
            SelectionConfiguration config, bool fullSize = false)
            where P : ICoordinate
        {
            Contract.Requires(profile != null);
            Contract.Requires(config != null);

            return tile.GroupBy(c => c.Surface(tile, profile, config));
        }


        /// <summary>
        ///     Gets the center coordinate
        /// </summary>
        /// <returns>The center coordinate</returns>
        /// <param name="tile">IQuantifiedTile</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static Coordinate GetCenter<T>(this IQuantifiedTile<T> tile)
            where T : ICoordinate
        {
            var center = tile.Reference.ToCoordinate();

            center.X -= (int) Math.Round(tile.RefOffsetX / tile.ElementStepX, 0);
            center.Y -= (int) Math.Round(tile.RefOffsetY / tile.ElementStepY, 0);

            return center;
        }

        #region AsQuantified

        public static IQuantifiedTile<T> AsQuantified<T>(this ITile<T> l)
            where T : class, ICoordinate
        {
            Contract.Requires(l is IQuantifiedTile<T>);

            return l as IQuantifiedTile<T> ?? l.ToQuantified();
        }

        #endregion

        #region ToQuantified

        public static QuantifiedTile<T> ToQuantified<T>(this IEnumerable<T> list, int referenceIndex = 0)
            where T : class, ICoordinate
        {
            Contract.Requires(list != null, nameof(list));
            Contract.Requires(referenceIndex >= 0, nameof(referenceIndex));

            return new QuantifiedTile<T>(list, referenceIndex);
        }

        public static QuantifiedTile<T> ToQuantified<T>(this ITile<T> list)
            where T : class, ICoordinate
        {
            Contract.Requires(list != null, nameof(list));
            return new QuantifiedTile<T>(list);
        }

        public static QuantifiedTile<T> ToQuantified<T>(this ITile<T> list, double sizeX, double sizeY)
            where T : class, ICoordinate
        {
            Contract.Requires(list != null, nameof(list));
            Contract.Requires(sizeX > 0, nameof(sizeX));
            Contract.Requires(sizeY > 0, nameof(sizeY));


            return new QuantifiedTile<T>(list, sizeX, sizeY);
        }

        public static QuantifiedTile<T> ToQuantified<T>(this ITile<T> list, double sizeX, double sizeY, double stepX,
            double stepY)
            where T : class, ICoordinate
        {
            Contract.Requires(list != null, nameof(list));
            Contract.Requires(sizeX > 0, nameof(sizeX));
            Contract.Requires(sizeY > 0, nameof(sizeY));
            Contract.Requires(stepX > 0, nameof(stepX));
            Contract.Requires(stepY > 0, nameof(stepY));
            Contract.Requires(stepX < sizeX, nameof(stepX) + " must be greater than " + nameof(sizeX));
            Contract.Requires(stepY < sizeY, nameof(stepY) + " must be greater than " + nameof(sizeY));

            return new QuantifiedTile<T>(list, sizeX, sizeY, stepX, stepY);
        }

        public static QuantifiedTile<T> ToQuantified<T>(this ITile<T> list, double sizeX, double sizeY, double stepX,
            double stepY, double refOffsetX, double refOffsetY)
            where T : class, ICoordinate
        {
            Contract.Requires(list != null, nameof(list));
            Contract.Requires(sizeX > 0, nameof(sizeX));
            Contract.Requires(sizeY > 0, nameof(sizeY));
            Contract.Requires(stepX > 0, nameof(stepX));
            Contract.Requires(stepY > 0, nameof(stepY));
            Contract.Requires(stepX < sizeX, nameof(stepX) + " must be greater than " + nameof(sizeX));
            Contract.Requires(stepY < sizeY, nameof(stepY) + " must be greater than " + nameof(sizeY));


            return new QuantifiedTile<T>(list, sizeX, sizeY, stepX, stepY, refOffsetX, refOffsetY);
        }

        #endregion

        #region Crop

        #endregion
    }
}