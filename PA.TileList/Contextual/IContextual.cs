using PA.TileList.Linear;

namespace PA.TileList.Contextual
{
    public interface IContextual<T> : ICoordinate
        where T : ICoordinate
    {
        T Context { get; }
    }
}