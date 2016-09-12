using PA.TileList.Quantified;
using PA.TileList.Extensions;
using PA.TileList.Linear;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PA.TileList.Circular
{
    public static class CircularExtensions
    {
        public static IEnumerable<KeyValuePair<T, double>> Distance<T>(this IQuantifiedTile<T> tile, Func<T, bool> predicate = null)
            where T : ICoordinate
        {
            return tile.WhereOrDefault(predicate).Select(c => new KeyValuePair<T, double>(c, Math.Sqrt(tile.Distance2(c))));
        }

        internal static double Distance2<T>(this IQuantifiedTile<T> tile, T c)
            where T : ICoordinate
        {
            double testX = (c.X - tile.Reference.X) * tile.ElementStepX + tile.RefOffsetX;
            double testY = (c.Y - tile.Reference.Y) * tile.ElementStepY + tile.RefOffsetY;
            return Math.Pow(testX, 2d) + Math.Pow(testY, 2d);
        }

        public static IEnumerable<KeyValuePair<T, int>> Points<T>(this IQuantifiedTile<T> tile, CircularProfile p, SelectionConfiguration config, Func<T, bool> predicate = null)
            where T : ICoordinate
        {
            double minRadius2 = Math.Pow(p.GetMinRadius(), 2d);
            double maxRadius2 = Math.Pow(p.GetMaxRadius(), 2d);
            CircularProfile.ProfileStep first = p.GetFirst();
            CircularProfile.ProfileStep[] profile = p.Profile.ToArray();

            foreach (T c in tile.WhereOrDefault(predicate))
            {
                //				CircularProfile.ProfileStep step = first;
                //				bool quickMode = true;
                //
                //				// Quick check with 4 corners
                //				int quick = tile.Points (c, 2, 1, (angle, r2) => {
                //					if (r2 > maxRadius2) {
                //						return false;
                //					}
                //
                //					if (r2 < minRadius2) {
                //						return true;
                //					}
                //
                //					CircularProfile.ProfileStep last = profile.LastOrDefault (ps => ps.Angle < angle) ?? first;
                //
                //					if (step != last) {
                //						quickMode = step.Equals (first);
                //						step = last;
                //					}
                //
                //					return config.SelectionType.HasFlag (CircularConfiguration.SelectionFlag.Under);
                //				});
                //
                //				if (quickMode) {
                //					// Certainly All Outside
                //					if (quick == 0) {
                //						yield return new KeyValuePair<T, int> (c, 0);
                //						continue;
                //					}
                //
                //					// Certainly All Inside
                //					if (quick == 4) {
                //						yield return new KeyValuePair<T, int> (c, (int)config.MaxSurface);
                //						continue;
                //					}
                //				}

                // Full check on all surface
                int full = tile.Points(c, config, (angle, r2) =>
                    {
                        if (r2 > maxRadius2)
                        {
                            //return false;
                        }

                        if (r2 < minRadius2)
                        {
                            //return true;
                        }

                        CircularProfile.ProfileStep last = profile.LastOrDefault(ps => ps.Angle < angle) ?? first;
                        return r2 < Math.Pow(last.Radius, 2d);

                    });

                yield return new KeyValuePair<T, int>(c, full);
            }
        }

        /// <summary>
        /// Get number of points  
        /// </summary>
        /// <param name="tile">Tile.</param>
        /// <param name="c">Element of Tile</param>
        /// <param name="config">Config.</param>
        /// <param name="predicate">Select point by angle and squared radius</param>
        /// <typeparam name="T">ICoordinate</typeparam>
        internal static int Points<T>(this IQuantifiedTile<T> tile, T c, SelectionConfiguration config, Func<double, double, bool> predicate)
            where T : ICoordinate
        {
            return tile.Points(c, config, (testX, testY, r2) => predicate(Math.Atan2(testY, testX), r2));
        }

        /// <summary>
        /// Get number of points  
        /// </summary>
        /// <param name="tile">Tile.</param>
        /// <param name="c">Element of Tile</param>
        /// <param name = "config">Config.</param>
        /// <param name="predicate">Select point at x, y and squared distance from tile center</param>
        /// <typeparam name="T">ICoordinate</typeparam>
        internal static int Points<T>(this IQuantifiedTile<T> tile, T c, SelectionConfiguration config, Func<double, double, double, bool> predicate)
            where T : ICoordinate
        {
            int points = 0;

            double[] testY = new double[config.Resolution];
            double[] testY2 = new double[config.Resolution];

            for (int i = 0; i < config.Resolution; i++)
            {

                double testX = ((c.X - tile.Reference.X) - 0.5d + i * config.StepSize) * tile.ElementStepX + tile.RefOffsetX;
                double testX2 = Math.Pow(testX, 2d);

                for (int j = 0; j < config.Resolution; j++)
                {
                    // Work in topleft quadrant by default
                    if (i == 0)
                    {
                        testY[j] = -(((c.Y - tile.Reference.Y) - 0.5d + j * config.StepSize) * tile.ElementStepY + tile.RefOffsetY);
                        testY2[j] = Math.Pow(testY[j], 2);
                    }

                    // X, Y, X2+Y2 (=radius2)
                    points += predicate(testX, testY[j], testX2 + testY2[j]) ? 1 : 0;
                }
            }

            return points;
        }

        public static IEnumerable<KeyValuePair<T, float>> Percent<T>(this IQuantifiedTile<T> tile, CircularProfile p, SelectionConfiguration config, Func<T, bool> predicate = null)
            where T : ICoordinate
        {
            foreach (KeyValuePair<T, int> c in tile.Points(p, config, predicate))
            {
                yield return new KeyValuePair<T, float>(c.Key, (float)c.Value / config.MaxSurface);
            }
        }

        public static IEnumerable<T> Take<T>(this IQuantifiedTile<T> tile, CircularProfile p, SelectionConfiguration config, Func<T, bool> predicate = null)
            where T : class, ICoordinate
        {
            foreach (KeyValuePair<T, int> c in tile.Points(p, config, predicate))
            {
                if (config.SelectionType.HasFlag(SelectionConfiguration.SelectionFlag.Inside) && config.MinSurface <= c.Value)
                {
                    yield return c.Key;
                }

                if (config.SelectionType.HasFlag(SelectionConfiguration.SelectionFlag.Under) && 0 < c.Value && c.Value < config.MinSurface)
                {
                    yield return c.Key;
                }

                if (config.SelectionType.HasFlag(SelectionConfiguration.SelectionFlag.Outside) && c.Value == 0)
                {
                    yield return c.Key;
                }
            }
        }

        public static IQuantifiedTile<T> Take<T>(this IQuantifiedTile<T> tile, CircularProfile p, SelectionConfiguration config, ref bool referenceChange, Func<T, bool> predicate = null)
            where T : class, ICoordinate
        {
            IQuantifiedTile<T> qtile = new QuantifiedTile<T>(tile);

            IEnumerable<T> list = tile.Take(p, config, predicate);

            if (!list.Any())
            {
                referenceChange = false;
                (qtile as IList<T>).Clear();
            }
            else
            {
                referenceChange = referenceChange && !list.Contains(tile.Reference);

                if (referenceChange)
                {
                    qtile.SetReference(list.First());
                }

                foreach (T e in qtile.Except(list).ToArray())
                {
                    qtile.Remove(e);
                }
            }

            return qtile;
        }
    }
}
