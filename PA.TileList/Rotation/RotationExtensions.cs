using System;
using System.Collections.Generic;
using PA.TileList.Contextual;
using PA.TileList.Linear;
using PA.TileList.Tile;

namespace PA.TileList.Rotation
{
    public static class RotationExtensions
    {
        #region Element

        public static IContextual<T> Rotate<T>(this IContextual<T> c, ICoordinate center, RotationTable.Angle angle)
            where T : ICoordinate
        {
            float cX = c.X - center.X;
            float cY = c.Y - center.Y;

            c.X = Convert.ToInt32(center.X + RotationTable.Cos(angle) * cX - RotationTable.Sin(angle) * cY);
            c.Y = Convert.ToInt32(center.Y + RotationTable.Sin(angle) * cX + RotationTable.Cos(angle) * cY);

            return c;
        }

        public static IContextual<T> Rotate<T>(this T c, ICoordinate center, RotationTable.Angle angle)
            where T : ICoordinate
        {
            IContextual<T> e = new Contextual<T>(c.X, c.Y, c);
            return e.Rotate(center, angle);
        }

        #endregion

        #region IEnumerable

        public static IEnumerable<IContextual<T>> Rotate<T>(this IEnumerable<T> list, ICoordinate center,
            RotationTable.Angle angle)
            where T : ICoordinate
        {
            foreach (var e in list)
                yield return e.Rotate(center, angle);
        }

        public static IEnumerable<IContextual<T>> Rotate<T>(this IEnumerable<IContextual<T>> list, ICoordinate center,
            RotationTable.Angle angle)
            where T : ICoordinate
        {
            foreach (var e in list)
                yield return e.Rotate(center, angle);
        }

        #endregion

        #region ITile

        public static Tile<IContextual<T>> Rotate<T>(this ITile<T> tile, RotationTable.Angle angle)
            where T : ICoordinate
        {
            var c = tile.Zone.Center();

            var reference = tile.Reference.Rotate(c, angle);

            var list = new Tile<IContextual<T>>(tile.Rotate<T>(c, angle));

            list.SetReference(list.Find(reference.X, reference.Y));

            return list;
        }

        public static Tile<IContextual<T>> Rotate<T>(this ITile<IContextual<T>> tile, RotationTable.Angle angle)
            where T : ICoordinate
        {
            var c = tile.Zone.Center();

            var reference = tile.Reference.Rotate(c, angle);

            var list = new Tile<IContextual<T>>(tile.Rotate<T>(c, angle));

            list.SetReference(list.Find(reference.X, reference.Y));

            return list;
        }

        #endregion
    }
}