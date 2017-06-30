using System;
using System.Collections.Generic;
using System.Linq;
using PA.TileList.Linear;
using PA.TileList.Quantified;

namespace PA.TileList.Geometrics.Extensions
{
	public static class PolygonExtensions
	{
		public static bool IsInside<P>(this P p, P[] polygon)
			where P : ICoordinate
		{
			// There must be at least 3 vertices in polygon[]
			if (polygon.Length < 3)
				throw new InvalidOperationException("At least 3 Coordinates needed in polygon");

			// Create a point for line segment from p to infinite
			var extreme = new Coordinate(int.MaxValue, p.Y);
			var ray = new Segment<P>(p, extreme);

			// Count intersections of the above line with sides of polygon
			int count = 0, i = 0;

			do
			{
				var next = (i + 1) % polygon.Length;

				var segment = new Segment<P>(polygon[i], polygon[next]);

				// Check if the line segment from 'p' to 'extreme' intersects
				// with the line segment from 'polygon[i]' to 'polygon[next]'
				if (!segment.AreCollinear(ray))
					count++;
				else if (p.AreCollinear(polygon[i], polygon[next]))
					return segment.Contains(p);

				i = next;
			} while (i != 0);

			// Return true if count is odd, false otherwise
			return count % 2 == 1;
		}


		public static double GetArea<P>(this IEnumerable<P> polygon)
			where P : ICoordinate
		{
			int i, j, k; // indices

			var p = new List<P>(polygon);

			if (p.Last().ToCoordinate() != p.First().ToCoordinate())
				p.Add(p.First());

			var n = p.Count() - 1;

			if (n < 3)
				return 0d;

			double area = 0;

			for (i = 1, j = 2, k = 0; i < n; i++, j++, k++)
				area += p[i].X * (p[j].Y - p[k].Y);

			area += p[n].X * (p[1].Y - p[n - 1].Y);

			return area / 2d;
		}


		public static IEnumerable<P> GetHull<P>(this IEnumerable<P> list)
			where P : ICoordinate
		{
			var p = list.OrderBy(t => t.X).GroupBy(t => t.X, (k, g) => g.OrderBy(t => t.Y)).SelectMany(t => t).ToArray();
			var n = p.Length;
			var h = new P[n];

			// the output array h[] will be used as the stack
			int bot = 0, top = -1; // indices for bottom and top of the stack
			int i; // array scan index

			// Get the indices of points with min x-coord and min|max y-coord
			int minmin = 0, minmax;

			var xMin = p[0].X;

			for (i = 1; i < n; i++)
			{
				if (p[i].X != xMin)
				{
					break;
				}
			}

			minmax = i - 1;

			if (minmax == n - 1)
			{
				// degenerate case: all x-coords == xmin
				h[++top] = p[minmin];
				if (p[minmax].Y != p[minmin].Y) // a  nontrivial segment
				{
					h[++top] = p[minmax];
				}
				h[++top] = p[minmin]; // add polygon endpoint

				return h.OfType<P>();
			}

			// Get the indices of points with max x-coord and min|max y-coord
			int maxmin, maxmax = n - 1;
			var xmax = p[n - 1].X;
			for (i = n - 2; i >= 0; i--)
			{
				if (p[i].X != xmax)
				{
					break;
				}
			}
			maxmin = i + 1;

			// Compute the lower hull on the stack H
			h[++top] = p[minmin]; // push  minmin point onto stack
			i = minmax;
			while (++i <= maxmin)
			{
				// the lower line joins p[minmin]  with p[maxmin]
				var o = p[minmin].GetOrientation(p[maxmin], p[i]);
				if (((o == Orientation.CounterClockWise) || (o == Orientation.Collinear)) && (i < maxmin))
					continue; // ignore p[i] above or on the lower line

				while (top > 0) // there are at least 2 points on the stack
				{
					// test if  p[i] is left of the line at the stack top
					var ot = h[top - 1].GetOrientation(h[top], p[i]);
					if (ot == Orientation.CounterClockWise)
					{
						break; // p[i] is a new hull  vertex
					}
					top--; // pop top point off  stack
				}
				h[++top] = p[i]; // push p[i] onto stack
			}

			// Next, compute the upper hull on the stack H above  the bottom hull
			if (maxmax != maxmin) // if  distinct xmax points
			{
				h[++top] = p[maxmax]; // push maxmax point onto stack
			}

			bot = top; // the bottom point of the upper hull stack
			i = maxmin;
			while (--i >= minmax)
			{
				// the upper line joins p[maxmax]  with p[minmax]
				var o = p[maxmax].GetOrientation(p[minmax], p[i]);
				if (((o == Orientation.CounterClockWise) || (o == Orientation.Collinear)) && (i > minmax))
					continue; // ignore p[i] below or on the upper line

				while (top > bot) // at least 2 points on the upper stack
				{
					// test if  p[i] is left of the line at the stack top
					var ot = h[top - 1].GetOrientation(h[top], p[i]);
					if (ot == Orientation.CounterClockWise)
						break; // p[i] is a new hull  vertex
					top--; // pop top point off  stack
				}
				h[++top] = p[i]; // push p[i] onto stack
			}

			if (minmax != minmin)
				h[++top] = p[minmin]; // push  joining endpoint onto stack

			return h.OfType<P>();
		}

		public static IEnumerable<P> GetBiggestZone<P>(this IEnumerable<P> list)
			where P : ICoordinate
		{
			double area = 0;

			P[] square = null;

			foreach (var t0 in GetHull(list))
			{
				var t1 = list.Where(t => t.X == t0.X).OrderBy(t => Math.Abs(t.DistanceTo(t0))).LastOrDefault();
				if (t1.Equals(default(P))) continue;

				var t2 = list.Where(t => t.Y == t1.Y).OrderBy(t => Math.Abs(t.DistanceTo(t1))).LastOrDefault();
				if (t2.Equals(default(P))) continue;

				var t3 = list.Where(t => (t.X == t2.X) && (t.Y == t0.Y)).FirstOrDefault();
				if (t3.Equals(default(P))) continue;

				var tmp_square = new[] { t0, t1, t2, t3 };

				var tmp_area = Math.Abs(tmp_square.GetArea());

				if (tmp_area > area)
				{
					area = tmp_area;
					square = tmp_square;
				}
			}

			return square;
		}

		public static Coordinate GetCenter<T>(this IEnumerable<T> list, Func<T, bool> predicate)
			where T : ICoordinate
		{
			var cX = 0f;
			var cY = 0f;
			var count = list.Count();

			foreach (var p in list)
			{
				cX += p.X ;
				cY += p.Y ;
			}

			return new Coordinate((int)Math.Round(cX / count, 0), (int)Math.Round(cY / count, 0));
		}


		/// <summary>
		/// Gets the center (barycenter) of ĉoordinates list
		/// </summary>
		/// <returns>The center.</returns>
		/// <param name="list">List.</param>
		/// <param name="predicate">Predicate.</param>
		/// <typeparam name="T">ICoordinate</typeparam>
		public static Coordinate GetCenter<T>(this IEnumerable<KeyValuePair<T, float>> list, Func<T, bool> predicate)
			where T : ICoordinate
		{
			var cX = 0f;
			var cY = 0f;
			float count = list.Count();

			foreach (var p in list)
			{
				cX += p.Key.X * p.Value;
				cY += p.Key.Y * p.Value;
			}

			return new Coordinate((int)Math.Round(cX / count, 0), (int)Math.Round(cY / count, 0));
		}
	}
}