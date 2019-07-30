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
using PA.TileList.Tile;

namespace PA.TileList.Selection
{
	public static class SelectionExtensions
	{
		#region Select 

		/// <summary>
		/// Selects the coordinates.
		/// </summary>
		/// <returns>The coordinates.</returns>
		/// <param name="tile">List.</param>
		/// <param name="profile">Profile.</param>
		/// <param name="config">Config.</param>
		/// <param name="fullsize">If set to <c>true</c> fullsize.</param>
		public static IEnumerable<Coordinate> SelectCoordinates(this IQuantifiedTile tile,
				ISelectionProfile profile, SelectionConfiguration config, bool fullSize = true)
		{
			Contract.Requires(tile != null, nameof(tile));
			Contract.Requires(profile != null, nameof(profile));
			Contract.Requires(config != null, nameof(config));

			return tile.Zone.Where(c => c.Selected(tile, profile, config, fullSize));
		}

		#endregion

		#region IQuantifiedTile

		/// <summary>
		/// Filter from tile according to profile, config and fullSize.
		/// </summary>
		/// <returns>The filter.</returns>
		/// <param name="tile">Tile.</param>
		/// <param name="profile">Profile.</param>
		/// <param name="config">Config.</param>
		/// <param name="fullSize">If set to <c>true</c> full size.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static QuantifiedTile<T> Filter<T>(this IQuantifiedTile<T> tile, ISelectionProfile profile,
			SelectionConfiguration config, bool fullSize = false)
			where T : class, ICoordinate
		{
			Contract.Requires(tile != null, nameof(tile));
			Contract.Requires(profile != null, nameof(profile));
			Contract.Requires(config != null, nameof(config));

			return tile.Except(profile, config, fullSize).ToTile().ToQuantified(tile.ElementSizeX, tile.ElementSizeY, tile.ElementStepX, tile.ElementStepY, tile.RefOffsetX, tile.RefOffsetY);
		}

		/// <summary>
		/// Take from tile according to profile, config and fullSize.
		/// </summary>
		/// <returns>The take.</returns>
		/// <param name="tile">Tile.</param>
		/// <param name="profile">Profile.</param>
		/// <param name="config">Config.</param>
		/// <param name="fullsize">If set to <c>true</c> full size.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static IEnumerable<T> Take<T>(this IQuantifiedTile<T> tile, ISelectionProfile profile,
			SelectionConfiguration config, bool fullSize = false)
			where T : class, ICoordinate
		{
			Contract.Requires(tile != null, nameof(tile));
			Contract.Requires(profile != null, nameof(profile));
			Contract.Requires(config != null, nameof(config));

			return tile.Where(c =>  c.Selected(tile, profile, config, fullSize)); ;
		}

		public static IEnumerable<T> Except<T>(this IQuantifiedTile<T> tile, ISelectionProfile profile,
			SelectionConfiguration config, bool fullSize = false)
			where T : class, ICoordinate
		{
			Contract.Requires(tile != null, nameof(tile));
			Contract.Requires(profile != null, nameof(profile));
			Contract.Requires(config != null, nameof(config));

			return tile.Where(c => ! c.Selected(tile, profile, config, fullSize));
		}

		#endregion


		#region Helpers

		/// <summary>
		/// Position the specified c within tile according to profile, config and fullSize.
		/// </summary>
		/// <returns>The position.</returns>
		/// <param name="c">C.</param>
		/// <param name="tile">Tile.</param>
		/// <param name="profile">Profile.</param>
		/// <param name="config">Config.</param>
		/// <param name="fullSize">If set to <c>true</c> full size.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		[Obsolete("Should use Selected() instead")]
		public static SelectionPosition Position<T>(this T c, IQuantifiedTile tile, ISelectionProfile profile,
			SelectionConfiguration config, bool fullSize = false)
			where T : ICoordinate
		{
			Contract.Requires(tile != null, nameof(tile));
			Contract.Requires(profile != null, nameof(profile));
			Contract.Requires(config != null, nameof(config));

			// full mode / follows SelectionConfiguration
			var points = c.CountPoints(tile, profile, config, fullSize);

			if (points >= config.MinSurface)
				return config.SelectionType;

			if (points <= 0)
				return config.SelectionType ^ SelectionPosition.Under;

			return SelectionPosition.Under;
		}

		/// <summary>
		/// Position the specified c within tile according to profile, config and fullSize.
		/// </summary>
		/// <returns>The position.</returns>
		/// <param name="c">C.</param>
		/// <param name="tile">Tile.</param>
		/// <param name="profile">Profile.</param>
		/// <param name="config">Config.</param>
		/// <param name="fullSize">If set to <c>true</c> full size.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static bool Selected<T>(this T c, IQuantifiedTile tile, ISelectionProfile profile,
			SelectionConfiguration config, bool fullSize = false)
			where T : ICoordinate
		{
			Contract.Requires(tile != null, nameof(tile));
			Contract.Requires(profile != null, nameof(profile));
			Contract.Requires(config != null, nameof(config));

			// full mode / follows SelectionConfiguration
			var points = c.CountPoints(tile, profile, config, fullSize);

			if (SelectionPosition.Under.HasFlag(config.SelectionType))
				return (points > 0);

			return (points > 0 && points >= config.MinSurface);
		}

		/// <summary>
		/// Counts the points.
		/// </summary>
		/// <returns>The points.</returns>
		/// <param name="c">C.</param>
		/// <param name="tile">Tile.</param>
		/// <param name="profile">Profile.</param>
		/// <param name="config">Config.</param>
		/// <param name="fullSize">If set to <c>true</c> full size.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static uint CountPoints<T>(this T c, IQuantifiedTile tile, ISelectionProfile profile,
			SelectionConfiguration config, bool fullSize = false)
			where T : ICoordinate
		{
			Contract.Requires(tile != null, nameof(tile));
			Contract.Requires(profile != null, nameof(profile));
			Contract.Requires(config != null, nameof(config));

			profile.OptimizeProfile();

			SelectionPosition position = 0x00;

			return c.CountPoints(tile, config.ResolutionX, config.ResolutionY,
			                     (xc, yc, xc2, yc2) => (position |= profile.Position(xc, yc, xc2, yc2)).HasFlag(config.SelectionType), fullSize);
		}

		/// <summary>
		/// Counts the points.
		/// </summary>
		/// <returns>The points.</returns>
		/// <param name="c">C.</param>
		/// <param name="tile">Tile.</param>
		/// <param name="pointsInX">Points in x.</param>
		/// <param name="pointsInY">Points in y.</param>
		/// <param name="predicate">Predicate.</param>
		/// <param name="fullSize">If set to <c>true</c> full size.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static uint CountPoints<T>(this T c, IQuantifiedTile tile, int pointsInX, int pointsInY,
			Func<double, double, double, double, bool> predicate, bool fullSize = false)
			where T : ICoordinate
		{
			Contract.Requires(tile != null);
			Contract.Requires(predicate != null);
			Contract.Requires(pointsInX >= 2, nameof(pointsInX));
			Contract.Requires(pointsInY >= 2, nameof(pointsInY));

			var points = 0u;

			c.GetPoints(tile, pointsInX, pointsInY, (xc, yc, xc2, yc2) => points += predicate(xc, yc, xc2, yc2) ? 1u : 0u,
				fullSize);

			return points;
		}

		/// <summary>
		/// Gets the bounds.
		/// </summary>
		/// <param name="c">C.</param>
		/// <param name="tile">Tile.</param>
		/// <param name="predicate">Predicate.</param>
		/// <param name="fullSize">If set to <c>true</c> full size.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void GetBounds<T>(this T c, IQuantifiedTile tile,
		 Action<double, double, double, double> predicate, bool fullSize = false)
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
		/// <param name="pointsInX">Points in x.</param>
		/// <param name="pointsInY">Points in y.</param>
		/// <param name="predicate">Predicate.</param>
		/// <param name="fullSize">If set to <c>true</c> full size.</param>
		/// <typeparam name="T">ICoordinate</typeparam>
		public static void GetPoints<T>(this T c, IQuantifiedTile tile, int pointsInX, int pointsInY,
			Action<double, double, double, double> predicate, bool fullSize = false)
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

	#endregion
}