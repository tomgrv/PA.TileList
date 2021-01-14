namespace PA.TileList.Linear
{
    public interface ICoordinate
    {
        int X { get; set; }
        int Y { get; set; }

#if DEBUG
        object Tag { get; set; }
#endif
        object Clone(int x, int y);
        object Clone();
    }
}