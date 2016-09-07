using System;
using PA.TileList;
using PA.TileList.Geometrics;
using NUnit.Framework;

namespace PA.TileList.Tests
{
    [TestFixtureAttribute]
    public class CoordinateTests
    {
        [Test]
        public void CreateCoordinate()
        {
            var a = new Coordinate(0, 0);
            var b = new Coordinate(-1, -1);
        }
    }



}
