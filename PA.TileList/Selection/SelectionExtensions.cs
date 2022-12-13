//
// SelectionExtensions.cs
//
// Author:
//       Thomas <tomgrv@users.perspikapps.fr>
//
// Copyright (c) 2016 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using PA.TileList.Linear;
using PA.TileList.Quantified;
using PA.TileList.Tile;

namespace PA.TileList.Selection
{
    public static class SelectionExtensions
    {
        #region Select

        /// <summary>
        ///     Selects the coordinates.
        /// </summary>
        /// <returns>The coordinates.</returns>
        /// <param name="tile">List.</param>
        /// <param name="profile">Profile.</param>
        /// <param name="config">Config.</param>
        /// <param name="fullsize">If set to <c>true</c> fullsize.</param>
        public static IEnumerable<Coordinate> SelectCoordinates(this IQuantifiedTile tile,
            ISelectionProfile profile, SelectionConfiguration config)
        {
            Contract.Requires(tile != null, nameof(tile));
            Contract.Requires(profile != null, nameof(profile));
            Contract.Requires(config != null, nameof(config));

            return tile.Zone.Where(c => c.Selected(tile, profile, config));
        }

        #endregion

        #region 

        /// <summary>
        ///     Filter from tile according to profile, config and fullSize.
        /// </summary>
        /// <returns>The filter.</returns>
        /// <param name="tile">Tile.</param>
        /// <param name="profile">Profile.</param>
        /// <param name="config">Config.</param>
        /// <param name="fullSize">If set to <c>true</c> full size.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static QuantifiedTile<T> Keep<T>(this IQuantifiedTile<T> tile, ISelectionProfile profile,
            SelectionConfiguration config)
            where T : class, ICoordinate
        {
            Contract.Requires(tile != null, nameof(tile));
            Contract.Requires(profile != null, nameof(profile));
            Contract.Requires(config != null, nameof(config));

            return tile.Take(profile, config).ToTile().ToQuantified(tile.ElementSizeX, tile.ElementSizeY,
                tile.ElementStepX, tile.ElementStepY, tile.RefOffsetX, tile.RefOffsetY);
        }

        /// <summary>
        ///     Filter from tile according to profile, config and fullSize.
        /// </summary>
        /// <returns>The filter.</returns>
        /// <param name="tile">Tile.</param>
        /// <param name="profile">Profile.</param>
        /// <param name="config">Config.</param>
        /// <param name="fullSize">If set to <c>true</c> full size.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static QuantifiedTile<T> Remove<T>(this IQuantifiedTile<T> tile, ISelectionProfile profile,
            SelectionConfiguration config)
            where T : class, ICoordinate
        {
            Contract.Requires(tile != null, nameof(tile));
            Contract.Requires(profile != null, nameof(profile));
            Contract.Requires(config != null, nameof(config));

            return tile.Except(profile, config).ToTile().ToQuantified(tile.ElementSizeX, tile.ElementSizeY,
                tile.ElementStepX, tile.ElementStepY, tile.RefOffsetX, tile.RefOffsetY);
        }

        /// <summary>
        ///     Filter from tile according to profile, config and fullSize.
        /// </summary>
        /// <returns>The filter.</returns>
        /// <param name="tile">Tile.</param>
        /// <param name="profile">Profile.</param>
        /// <param name="config">Config.</param>
        /// <param name="fullSize">If set to <c>true</c> full size.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        [Obsolete("Consider using .Remove()")]
        public static QuantifiedTile<T> Filter<T>(this IQuantifiedTile<T> tile, ISelectionProfile profile,
            SelectionConfiguration config)
            where T : class, ICoordinate
        {
            Contract.Requires(tile != null, nameof(tile));
            Contract.Requires(profile != null, nameof(profile));
            Contract.Requires(config != null, nameof(config));

            return tile.Except(profile, config).ToTile().ToQuantified(tile.ElementSizeX, tile.ElementSizeY,
                tile.ElementStepX, tile.ElementStepY, tile.RefOffsetX, tile.RefOffsetY);
        }

        /// <summary>
        ///     Take from tile according to profile, config and fullSize.
        /// </summary>
        /// <returns>The take.</returns>
        /// <param name="tile">Tile.</param>
        /// <param name="profile">Profile.</param>
        /// <param name="config">Config.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static IEnumerable<T> Take<T>(this IQuantifiedTile<T> tile, ISelectionProfile profile,
            SelectionConfiguration config)
            where T : class, ICoordinate
        {
            Contract.Requires(tile != null, nameof(tile));
            Contract.Requires(profile != null, nameof(profile));
            Contract.Requires(config != null, nameof(config));

            return tile.Where(c => c.Selected(tile, profile, config));
            ;
        }

        public static IEnumerable<T> Except<T>(this IQuantifiedTile<T> tile, ISelectionProfile profile,
            SelectionConfiguration config)
            where T : class, ICoordinate
        {
            Contract.Requires(tile != null, nameof(tile));
            Contract.Requires(profile != null, nameof(profile));
            Contract.Requires(config != null, nameof(config));

            return tile.Where(c => !c.Selected(tile, profile, config));
        }

        #endregion

        #region Helpers

        /// <summary>
        ///     Position the specified c within tile according to profile, config and fullSize.
        /// </summary>
        /// <returns>The position.</returns>
        /// <param name="c">C.</param>
        /// <param name="tile">Tile.</param>
        /// <param name="profile">Profile.</param>
        /// <param name="config">Config.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool Selected<T>(this T c, IQuantifiedTile tile, ISelectionProfile profile,
            SelectionConfiguration config)
            where T : ICoordinate
        {
            Contract.Requires(tile != null, nameof(tile));
            Contract.Requires(profile != null, nameof(profile));
            Contract.Requires(config != null, nameof(config));

            return c.OptimizedCount(tile, profile, config).IsSelected(config);
        }


        /// <summary>
        ///     Returns the surface covered by profile within tile according config and fullSize.
        /// </summary>
        /// <returns>The position.</returns>
        /// <param name="c">C.</param>
        /// <param name="tile">Tile.</param>
        /// <param name="profile">Profile.</param>
        /// <param name="config">Config.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static uint Surface<T>(this T c, IQuantifiedTile tile, ISelectionProfile profile,
            SelectionConfiguration config)
            where T : ICoordinate
        {
            Contract.Requires(tile != null, nameof(tile));
            Contract.Requires(profile != null, nameof(profile));
            Contract.Requires(config != null, nameof(config));

            return c.OptimizedCount(tile, profile, config).GetSurface(config);
        }

        public static SelectionPoints OptimizedCount<T>(this T c, IQuantifiedTile tile, ISelectionProfile profile,
            SelectionConfiguration config, Action<double[], double[], SelectionPosition> predicate = null)
            where T : ICoordinate
        {
            var quickPoints = c.CountPoints(tile, profile, config.GetQuickSelectionVariant(), predicate);

            if (!quickPoints.IsAmbiguous())
                return quickPoints;

            return c.CountPoints(tile, profile, config, predicate);
        }

        /// <summary>
        ///     Counts the points.
        /// </summary>
        /// <returns>The points.</returns>
        /// <param name="c">C.</param>
        /// <param name="tile">Tile.</param>
        /// <param name="profile">Profile.</param>
        /// <param name="config">Config.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static SelectionPoints CountPoints<T>(this T c, IQuantifiedTile tile, ISelectionProfile profile,
            SelectionConfiguration config, Action<double[], double[], SelectionPosition> predicate = null)
            where T : ICoordinate
        {
            Contract.Requires(tile != null, nameof(tile));
            Contract.Requires(profile != null, nameof(profile));
            Contract.Requires(config != null, nameof(config));

            var p = new SelectionPoints();

            c.GetPoints(tile, config,
                (xc, yc) =>
                {
                    var position = profile.Position(xc, yc, config.IsQuick);

                    switch (position)
                    {
                        case SelectionPosition.Inside:
                            p.Inside += 1;
                            break;
                        case SelectionPosition.Outside:
                            p.Outside += 1;
                            break;
                        case SelectionPosition.Under:
                            p.Under += 1;
                            break;
                    }

                    predicate?.Invoke(xc, yc, position);

                }, profile.GetValuesX, profile.GetValuesY);

            return p;
        }

        /// <summary>
        ///     Gets the bounds.
        /// </summary>
        /// <param name="c">C.</param>
        /// <param name="tile">Tile.</param>
        /// <param name="predicate">Predicate.</param>
        /// <param name="fullSize">If set to <c>true</c> full size.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void GetBounds<T>(this T c, IQuantifiedTile tile,
            Action<double, double, double, double> predicate, bool fullSize)
            where T : ICoordinate
        {
            Contract.Requires(tile != null, nameof(tile));
            Contract.Requires(predicate != null, nameof(predicate));

            var xMin = double.MaxValue;
            var yMin = double.MaxValue;
            var xMax = double.MinValue;
            var yMax = double.MinValue;

            c.GetPoints(tile, 2, 2, (xc, yc, xc2, yc2) =>
            {
                if (xc < xMin)
                    xMin = xc;
                if (xc > xMax)
                    xMax = xc;
                if (yc < yMin)
                    yMin = yc;
                if (yc > yMax)
                    yMax = yc;
            }, fullSize);

            predicate(xMin, yMin, xMax, yMax);
        }


        /// <summary>
        ///     Gets the points.
        /// </summary>
        /// <param name="c">C.</param>
        /// <param name="tile">Tile.</param>
        /// <param name="config">SelectionConfiguration.</param>
        /// <param name="predicate">Predicate.</param>
        /// <typeparam name="T">ICoordinate</typeparam>
        public static void GetPoints<T>(this T c, IQuantifiedTile tile, SelectionConfiguration config,
            Action<double[], double[]> predicate, Func<double, double[]> getValuesX, Func<double, double[]> getValuesY)
            where T : ICoordinate
        {
            Contract.Requires(tile != null, nameof(tile));
            Contract.Requires(predicate != null, nameof(predicate));
            Contract.Requires(config != null, nameof(config));

            var ratioX = config.UseFullSurface ? 1f : tile.ElementSizeX / tile.ElementStepX;
            var ratioY = config.UseFullSurface ? 1f : tile.ElementSizeY / tile.ElementStepY;

            var stepSizeX = config.ResolutionX > 1 ? ratioX / (config.ResolutionX - 1) : 0f;
            var stepSizeY = config.ResolutionY > 1 ? ratioY / (config.ResolutionY - 1) : 0f;

            var offsetX = ratioX / 2f;
            var offsetY = ratioY / 2f;

            var testY = new double[config.ResolutionY][];

            var reference = tile.GetReference();

            for (var i = 0; i < config.ResolutionX; i++)
            {
                var testX = getValuesX((c.X - reference.X - offsetX + i * stepSizeX) * tile.ElementStepX +
                                       tile.RefOffsetX);

                for (var j = 0; j < config.ResolutionY; j++)
                {
                    if (i == 0)
                        testY[j] = getValuesY((c.Y - reference.Y - offsetY + j * stepSizeY) * tile.ElementStepY +
                                              tile.RefOffsetY);

                    predicate(testX, testY[j]);
                }
            }
        }


        /// <summary>
        ///     Gets the points.
        /// </summary>
        /// <param name="c">C.</param>
        /// <param name="tile">Tile.</param>
        /// <param name="pointsInX">Points in x.</param>
        /// <param name="pointsInY">Points in y.</param>
        /// <param name="predicate">Predicate.</param>
        /// <param name="fullSize">If set to <c>true</c> full size.</param>
        /// <typeparam name="T">ICoordinate</typeparam>
        public static void GetPoints<T>(this T c, IQuantifiedTile tile, int pointsInX, int pointsInY,
            Action<double, double, double, double> predicate, bool fullSize)
            where T : ICoordinate
        {
            Contract.Requires(tile != null, nameof(tile));
            Contract.Requires(predicate != null, nameof(predicate));
            Contract.Requires(pointsInX >= 1, nameof(pointsInX));
            Contract.Requires(pointsInY >= 1, nameof(pointsInY));

            var ratioX = fullSize ? 1f : tile.ElementSizeX / tile.ElementStepX;
            var ratioY = fullSize ? 1f : tile.ElementSizeY / tile.ElementStepY;

            var stepSizeX = pointsInX > 1 ? ratioX / (pointsInX - 1) : 0f;
            var stepSizeY = pointsInY > 1 ? ratioY / (pointsInY - 1) : 0f;

            var offsetX = ratioX / 2f;
            var offsetY = ratioY / 2f;

            var testY = new double[pointsInY];
            var testY2 = new double[pointsInY];

            var reference = tile.GetReference();

            for (var i = 0; i < pointsInX; i++)
            {
                var testX = (c.X - reference.X - offsetX + i * stepSizeX) * tile.ElementStepX + tile.RefOffsetX;
                var testX2 = testX * testX;

                for (var j = 0; j < pointsInY; j++)
                {
                    if (i == 0)
                    {
                        testY[j] = (c.Y - reference.Y - offsetY + j * stepSizeY) * tile.ElementStepY + tile.RefOffsetY;
                        testY2[j] = testY[j] * testY[j];
                    }

                    predicate(testX, testY[j], testX2, testY2[j]);
                }
            }
        }
    }

    #endregion
}