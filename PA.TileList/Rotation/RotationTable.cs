namespace PA.TileList.Rotation
{
    public static class RotationTable
    {
        public enum Angle
        {
            d0 = 0,
            d90 = 1,
            d180 = 2,
            d270 = 3
        }

        private static readonly int[] cosTable = {1, 0, -1, 0};
        private static readonly int[] sinTable = {0, 1, 0, -1};

        internal static int Cos(Angle angle)
        {
            return cosTable[(int) angle];
        }

        internal static int Sin(Angle angle)
        {
            return sinTable[(int) angle];
        }
    }
}