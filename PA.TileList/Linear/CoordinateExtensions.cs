using PA.TileList.Contextual;

namespace PA.TileList.Linear
{
    public static class CoordinateExtensions
    {
        public static string ToString<T>(this T c)
            where T : ICoordinate
        {
            return c.X + "," + c.Y;
        }

        public static Coordinate ToCoordinate<T>(this T c)
            where T : ICoordinate
        {
            return new Coordinate(c.X, c.Y);
        }

        public static IContextual<T> Offset<T>(this T c, int offsetX, int offsetY)
            where T : ICoordinate
        {
            return new Contextual<T>(c.X + offsetX, c.Y + offsetY, c);
        }

        public static IContextual<T> Offset<T>(this T c, ICoordinate o)
            where T : ICoordinate
        {
            return new Contextual<T>(c.X + o.X, c.Y + o.Y, c);
        }

        public static IContextual<T> Offset<T>(this IContextual<T> c, int shiftX, int shiftY)
            where T : ICoordinate
        {
            return new Contextual<T>(c.X + shiftX, c.Y + shiftY, c.Context);
        }

        public static IContextual<T> Offset<T>(this IContextual<T> c, ICoordinate o)
            where T : ICoordinate
        {
            return new Contextual<T>(c.X + o.X, c.Y + o.Y, c.Context);
        }
    }
}