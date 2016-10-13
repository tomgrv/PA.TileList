using PA.TileList.Linear;

namespace PA.TileList.Contextual
{
    public class Contextual<T> : Coordinate, IContextual<T>
        where T : ICoordinate
    {
        public Contextual(int x, int y, T context)
            : base(x, y)
        {
            this.Context = context;
        }

        public T Context { get; }

        public override string ToString()
        {
            return base.ToString() + " [" + this.Context + "]";
        }

        public static IContextual<T> FromContext(T e)
        {
            return new Contextual<T>(e.X, e.Y, e);
        }
    }
}