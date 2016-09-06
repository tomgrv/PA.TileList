using System;
using System.Linq;
using PA.TileList;
using PA.TileList.Linear;
using System.Collections.Generic;
using NUnit.Framework;

namespace PA.TileList.Tests
{
    [TestFixture]
    public class LinearTests
    {
        [Test]
        public void GetLine()
        {
            List<Coordinate> list = new List<Coordinate>()
            {
                new Coordinate(0, 0),
                new Coordinate(1, 0),
                new Coordinate(1, 1),
                new Coordinate(0, 1),
                new Coordinate(2, 0),
                new Coordinate(2, 1),
                new Coordinate(2, 2),
                new Coordinate(0, 2),
                new Coordinate(1, 2)
            };

            IEnumerable<Coordinate> line1 = list.GetLine(new Coordinate(0, 0), new Coordinate(2, 2), true);

            Assert.IsTrue(line1.Count() == 3);
            Assert.IsTrue(line1.Contains(list[2]));

            IEnumerable<Coordinate> line2 = list.GetLine(new Coordinate(0, 0), new Coordinate(2, 3), false);

            Assert.IsTrue(line2.Count() == 2);
            Assert.IsTrue(line2.Contains(list[8]));

            IEnumerable<Coordinate> line3 = list.GetLine(new Coordinate(0, 0), new Coordinate(2, 3), true);

            Assert.IsTrue(line3.Count() == 1);
            Assert.IsTrue(line3.Contains(list[0]));
        }
    }
}
