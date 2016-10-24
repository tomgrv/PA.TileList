//
// SelectionExtensions.cs
//
// Author:
//       Thomas GERVAIS <thomas.gervais@gmail.com>
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

namespace PA.TileList.Selection
{
    public static class SelectionExtensions
    {
        public static IQuantifiedTile<T> Take<T>(this IQuantifiedTile<T> tile, ISelectionProfile profile,
            SelectionConfiguration config, ref bool referenceChange, bool fullSize = false)
            where T : class, ICoordinate
        {
            var l = tile.Take(profile, config, fullSize);

            var q = new QuantifiedTile<T>(tile);

            referenceChange = referenceChange && !l.Contains(tile.Reference);

            if (referenceChange)
                q.SetReference(l.First());

            foreach (var e in q.Except(l).ToArray())
                q.Remove(e);

            q.UpdateZone();

            return q;
        }

        public static IEnumerable<T> Take<T>(this IQuantifiedTile<T> tile, ISelectionProfile profile,
            SelectionConfiguration config, bool fullSize = false)
            where T : class, ICoordinate
        {
            return tile.Where(c => config.SelectionType.HasFlag(c.Position(tile, profile, config, fullSize)));
        }

        public static IEnumerable<ICoordinate> SelectCoordinates<T>(this IQuantifiedTile<T> list,
            ISelectionProfile profile, SelectionConfiguration config, bool fullsize = true)
            where T : ICoordinate
        {
            return list.Zone.Where(c => config.SelectionType.HasFlag(c.Position(list, profile, config, fullsize)));
        }


        public static SelectionPosition Position<T>(this T c, IQuantifiedTile tile, ISelectionProfile profile,
            SelectionConfiguration config, bool fullSize = false)
            where T : ICoordinate
        {
            var points = c.CountPoints(tile, profile, config, fullSize);

            if (points >= config.MinSurface)
                return SelectionPosition.Inside;

            if (points <= 0)
                return SelectionPosition.Outside;

            return SelectionPosition.Under;
        }

        public static int CountPoints<T>(this T c, IQuantifiedTile tile, ISelectionProfile profile,
            SelectionConfiguration config, bool fullSize = false)
            where T : ICoordinate
        {
            return c.CountPoints(tile, config.ResolutionX, config.ResolutionY,
                (xc, yc, xc2, yc2) => profile.Position(xc, yc, xc2, yc2) == SelectionPosition.Inside, fullSize);
        }

        public static int CountPoints<T>(this T c, IQuantifiedTile tile, int pointsInX, int pointsInY,
            Func<double, double, double, double, bool> predicate, bool fullSize = false)
            where T : ICoordinate
        {
            var points = 0;

            c.GetPoints(tile, pointsInX, pointsInX, (xc, yc, xc2, yc2) => points += predicate(xc, yc, xc2, yc2) ? 1 : 0,
                fullSize);

            return points;
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
            Action<double, double, double, double> predicate, bool fullSize = false)
            where T : ICoordinate
        {
            Contract.Requires(tile != null);
            Contract.Requires(predicate != null);

            if (pointsInX < 2)
                throw new ArgumentOutOfRangeException(nameof(pointsInX), pointsInX, "must be >= 2");

            if (pointsInY < 2)
                throw new ArgumentOutOfRangeException(nameof(pointsInY), pointsInY, "must be >= 2");

            var ratioX = fullSize ? 1f : tile.ElementSizeX / tile.ElementStepX;
            var ratioY = fullSize ? 1f : tile.ElementSizeY / tile.ElementStepY;

            var stepSizeX = ratioX / (pointsInX - 1);
            var stepSizeY = ratioY / (pointsInY - 1);

            var offsetX = ratioX / 2f;
            var offsetY = ratioY / 2f;

            var testY = new double[pointsInY];
            var testY2 = new double[pointsInY];

            var reference = tile.GetReference();

            for (var i = 0; i < pointsInX; i++)
            {
                var testX = (c.X - reference.X - offsetX + i * stepSizeX) * tile.ElementStepX + tile.RefOffsetX;
                var testX2 = Math.Pow(testX, 2d);

                for (var j = 0; j < pointsInY; j++)
                {
                    if (i == 0)
                    {
                        testY[j] = (c.Y - reference.Y - offsetY + j * stepSizeY) * tile.ElementStepY + tile.RefOffsetY;
                        testY2[j] = Math.Pow(testY[j], 2);
                    }

                    predicate(testX, testY[j], testX2, testY2[j]);
                }
            }
        }
    }
}