﻿using System.Collections.Generic;
using System.Linq;
using PA.TileList.Contextual;
using PA.TileList.Cropping;
using PA.TileList.Quantified;
using PA.TileList.Tile;

namespace PA.TileList.Linear
{
    public static class TranslateExtensions
    {
        public enum TranslateSource
        {
            Center,
            Min,
            Max
        }

        /// <summary>
        ///     Translate all list so that specified source match coordinate [0,0]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<IContextual<T>> Translate<T>(this IEnumerable<T> c, TranslateSource source)
            where T : ICoordinate
        {
            switch (source)
            {
                case TranslateSource.Min:
                    return c.Translate(c.GetZone().Min, Coordinate.Zero);
                case TranslateSource.Max:
                    return c.Translate(c.GetZone().Max, Coordinate.Zero);
                default:
                    return c.Translate(c.GetZone().Center(), Coordinate.Zero);
            }
        }

        /// <summary>
        ///     Translate all list so that specified source match coordinate [0,0]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<IContextual<T>> Translate<T>(this IEnumerable<IContextual<T>> c,
            TranslateSource source)
            where T : class, ICoordinate
        {
            switch (source)
            {
                case TranslateSource.Min:
                    return c.Translate(c.GetZone().Min, Coordinate.Zero);
                case TranslateSource.Max:
                    return c.Translate(c.GetZone().Max, Coordinate.Zero);
                default:
                    return c.Translate(c.GetZone().Center(), Coordinate.Zero);
            }
        }

        public static Tile<IContextual<T>> Translate<T>(this ITile<T> t, TranslateSource source)
            where T : class, ICoordinate
        {
            return t.AsEnumerable().Translate(source)
                .ToTile(t.IndexOf(t.Reference))
                .RefreshZone();
        }

        public static Tile<IContextual<T>> Translate<T>(this ITile<IContextual<T>> t, TranslateSource source)
            where T : class, ICoordinate
        {
            return t.AsEnumerable().Translate(source)
                .ToTile(t.IndexOf(t.Reference))
                .RefreshZone();
        }

        public static QuantifiedTile<IContextual<T>> Translate<T>(this IQuantifiedTile<T> t, TranslateSource source)
            where T : class, ICoordinate
        {
            return t.AsTile()
                .Translate(source)
                .ToQuantified(t.ElementSizeX, t.ElementSizeY, t.ElementStepX, t.ElementStepY, t.RefOffsetX, t.RefOffsetY);
        }

        public static QuantifiedTile<IContextual<T>> Translate<T>(this IQuantifiedTile<IContextual<T>> t,
            TranslateSource source)
            where T : class, ICoordinate
        {
            return t.AsTile()
                .Translate(source)
                .ToQuantified(t.ElementSizeX, t.ElementSizeY, t.ElementStepX, t.ElementStepY, t.RefOffsetX, t.RefOffsetY);
        }


        /// <summary>
        ///     Translate all list so that source match destination
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="refOrigin"></param>
        /// <param name="refDest"></param>
        /// <returns></returns>
        public static IEnumerable<IContextual<T>> Translate<T>(this IEnumerable<T> c, ICoordinate refOrigin,
            ICoordinate refDest)
            where T : ICoordinate
        {
            var offsetX = refDest.X - refOrigin.X;
            var offsetY = refDest.Y - refOrigin.Y;

            foreach (var e in c)
                yield return new Contextual<T>(e.X + offsetX, e.Y + offsetY, e);
        }


        /// <summary>
        ///     Translate all list so that source match destination
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="refOrigin"></param>
        /// <param name="refDest"></param>
        /// <returns></returns>
        public static IEnumerable<IContextual<T>> Translate<T>(this IEnumerable<IContextual<T>> c, ICoordinate refOrigin,
            ICoordinate refDest)
            where T : ICoordinate
        {
            var offsetX = refDest.X - refOrigin.X;
            var offsetY = refDest.Y - refOrigin.Y;

            foreach (var e in c)
            {
                e.X += offsetX;
                e.Y += offsetY;
            }

            return c;
        }
    }
}