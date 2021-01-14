using PA.TileList.Linear;

namespace PA.TileList.Contextual
{
    public class Contextual<T> : Coordinate, IContextual<T>
        where T : ICoordinate
    {
        public Contextual(int x, int y, T context)
            : base(x, y)
        {
            Context = context;
        }
#if DEBUG
        public new object Tag
        {
            get => Context.Tag;
            set => Context.Tag = value;
        }
#endif

        public T Context { get; }

        public override string ToString()
        {
            return base.ToString() + " [" + Context + "]";
        }

        public static IContextual<T> FromContext(T e)
        {
            return new Contextual<T>(e.X, e.Y, e);
        }
    }
}