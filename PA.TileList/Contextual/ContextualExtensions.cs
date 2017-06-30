using System;
using System.Collections.Generic;
using System.Linq;
using PA.TileList.Cropping;
using PA.TileList.Linear;
using PA.TileList.Quantified;
using PA.TileList.Tile;

namespace PA.TileList.Contextual
{
	public static class ContextualExtensions
	{
		#region Contextualize

		public static IContextual<T> Contextualize<T>(this ITile t, T item)
			where T : ICoordinate
		{
			return t.Contextualize(item, t.Zone.SizeX, t.Zone.SizeY);
		}

		public static IContextual<T> Contextualize<T>(this ITile t, T item, IZone a)
			where T : ICoordinate
		{
			return t.Contextualize(item, a.SizeX, a.SizeY);
		}

		public static IContextual<T> Contextualize<T>(this ITile t, T item, ushort sizeX, ushort sizeY)
			where T : ICoordinate
		{
			return new Contextual<T>(item.X + t.X * sizeX, item.Y + t.Y * sizeY, item);
		}

		public static void DeContextualize<T>(this IContextual<T> t)
			where T : ICoordinate
		{
			t.Context.X = t.X;
			t.Context.Y = t.Y;
		}

		#endregion

		#region Flatten

		public static QuantifiedTile<IContextual<T>> Flatten<U, T>(this IQuantifiedTile<U> t,
			Func<U, bool> predicateU = null, Func<T, bool> predicateT = null)
			where U : ITile<T>
			where T : ICoordinate
		{
			var reference = t.Reference.Contextualize(t.Reference.Reference);

			// Sizes in flattened output Tile
			var sizeX = t.ElementSizeX / t.Reference.Zone.SizeX;
			var sizeY = t.ElementSizeY / t.Reference.Zone.SizeY;
			var stepX = t.ElementStepX / t.Reference.Zone.SizeX;
			var stepY = t.ElementStepY / t.Reference.Zone.SizeY;

			// Convert offset<U> (relative to <U> center) to Offset<T> (relative to  <T> center), expressed in {number of <T>} 
			double distX = t.Reference.Reference.X - t.Reference.Zone.Min.X - (t.Reference.Zone.SizeX - 1) / 2f;
			double distY = t.Reference.Reference.Y - t.Reference.Zone.Min.Y - (t.Reference.Zone.SizeY - 1) / 2f;

			// Flatten Tile
			var flatten =  (t as ITile<U>).Flatten<U, T>(predicateU, predicateT);

			// Reference correction if needed
			distX += flatten.Reference.X - reference.X;
			distY += flatten.Reference.Y - reference.Y;

			// Converted to iquantified
			return new QuantifiedTile<IContextual<T>>(flatten, sizeX, sizeY, stepX, stepY, 
												      distX * stepX + t.RefOffsetX,
			                                          distY * stepY + t.RefOffsetY);
		}


		public static Tile<IContextual<T>> Flatten<U, T>(this ITile<U> t,
			Func<U, bool> predicateU = null, Func<T, bool> predicateT = null)
			where U : ITile<T>
			where T : ICoordinate
		{
			var reference = t.Reference.Contextualize(t.Reference.Reference);

			var tile = new Tile<IContextual<T>>(t.GetZone(), t
																 .Filter(predicateU)
																 .SelectMany(subtile => subtile
																			 .Filter(predicateT)
																			 .Select(c => subtile.Contextualize(c))));

			tile.UpdateZone();
			tile.SetReference(tile.Find(reference.X, reference.Y));
			return tile;
		}

		private static IEnumerable<T> Filter<T>(this IEnumerable<T> t, Func<T, bool> predicate)
			where T : ICoordinate
		{
			return predicate is Func<T, bool> ? t.Where(predicate) : t;
		}

		#endregion
	}
}