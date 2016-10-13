using NUnit.Framework;
using PA.TileList.Cropping;
using PA.TileList.Linear;

namespace PA.TileList.Tests
{
    [TestFixture]
    public class ZoneTests
    {
        [Test]
        public void CreateArea()
        {
            var a = new Zone(-1, -1, 10, 10);
            var b = new Zone(new Coordinate(-1, -1), new Coordinate(10, 10));

            Assert.AreEqual(a.SizeX, b.SizeX);
            Assert.AreEqual(a.SizeY, b.SizeY);
        }
    }
}