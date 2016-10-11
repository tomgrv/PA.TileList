
using PA.TileList.Contextual;
using PA.TileList.Extensions;
using PA.TileList.Quantified;
using PA.TileList.Geometrics;
using PA.TileList.Linear;
using PA.TileList.Tile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PA.TileList.Cropping;
using PA.TileList.Linear.TranslateExtensions;

namespace PA.TileList.Quantified
{
    public static class TranslateExtensions
    {
        

        

        public static IQuantifiedTile<IContextual<T>> Translate<T>(this IQuantifiedTile<T> t, TranslateSource source)
            where T : class, ICoordinate
        {
            return t.AsTile()
                .Translate<T>(source)
                        .AsQuantified(t.ElementSizeX, t.ElementSizeY, t.ElementStepX, t.ElementStepY, t.RefOffsetX, t.RefOffsetY);
        }

        public static IQuantifiedTile<IContextual<T>> Translate<T>(this IQuantifiedTile<IContextual<T>> t, TranslateSource source)
           where T : class, ICoordinate
        {
            return t.AsTile()
                .Translate(source)
                        .AsQuantified(t.ElementSizeX, t.ElementSizeY, t.ElementStepX, t.ElementStepY, t.RefOffsetX, t.RefOffsetY);
        }


        /// <summary>
        /// Translate all list so that source match destination
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="refOrigin"></param>
        /// <param name="refDest"></param>
        /// <returns></returns>
        public static IEnumerable<IContextual<T>> Translate<T>(this IEnumerable<T> c, ICoordinate refOrigin, ICoordinate refDest)
            where T : ICoordinate
        {
            int offsetX = refDest.X - refOrigin.X;
            int offsetY = refDest.Y - refOrigin.Y;

            foreach (T e in c)
            {
                yield return new Contextual<T>(e.X + offsetX, e.Y + offsetY, e);
            }
        }


        /// <summary>
        /// Translate all list so that source match destination
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="refOrigin"></param>
        /// <param name="refDest"></param>
        /// <returns></returns>
        public static IEnumerable<IContextual<T>> Translate<T>(this IEnumerable<IContextual<T>> c, ICoordinate refOrigin, ICoordinate refDest)
            where T : ICoordinate
        {
            int offsetX = refDest.X - refOrigin.X;
            int offsetY = refDest.Y - refOrigin.Y;

            foreach (IContextual<T> e in c)
            {
                e.X += offsetX;
                e.Y += offsetY;
            }

            return c;
        }
    }
}
