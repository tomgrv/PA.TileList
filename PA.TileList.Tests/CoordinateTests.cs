using NUnit.Framework;
using PA.TileList.Linear;

namespace PA.TileList.Tests
{
    [TestFixture]
    public class CoordinateTests
    {
        [Test]
        [Category("Trustable")]
        public void CreateCoordinate()
        {
            var a = new Coordinate(0, 0);
            var b = new Coordinate(-1, -1);
        }
    }
}