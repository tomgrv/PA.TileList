using PA.TileList.Linear;

namespace PA.TileList.Geometrics.Extensions
{
    public static class SegmentExtensions
    {
        /// <summary>
        ///     Determine whether or not OA and OB are Collinear
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        /// <summary>
        ///     Determine whether or not OA and OB are Collinear
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool AreCollinear<T>(this Segment<T> s1, Segment<T> s2)
            where T : ICoordinate
        {
            return s1.Origin.AreCollinear(s1.Point, s2.Vector());
        }
    }
}