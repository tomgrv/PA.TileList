using System;

namespace PA.TileList.Drawing.Graphics2D
{
    [Flags]
    public enum ScaleMode
    {
        NONE = 0x00,
        NOSTRETCH = 0x01,
        CENTER = 0x02,
        //EXACTPIXEL = 0x08,
        ALL = 0xFF
    }
}

