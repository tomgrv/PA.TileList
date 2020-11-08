namespace PA.TileList.Linear
{
    public interface ICoordinate
    {
        int X { get; set; }
        int Y { get; set; }
        object Clone(int x, int y);
        object Clone();

#if DEBUG
        object Tag { get; set; }
#endif

    }
}