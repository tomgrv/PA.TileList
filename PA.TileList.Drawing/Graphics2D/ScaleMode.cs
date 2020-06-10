using System;

namespace PA.TileList.Drawing.Graphics2D
{
    [Flags]
    public enum ScaleMode
    {
        XYRATIO = 0x01,
        CENTER = 0x02,
        STRETCH = 0x04,
        PXLSNAP = 0x08,
		FILL = 0x0B
    }
}