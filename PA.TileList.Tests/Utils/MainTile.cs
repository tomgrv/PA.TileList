//
// MainTile.cs
//
// Author:
//       Thomas GERVAIS <thomas.gervais@gmail.com>
//
// Copyright (c) 2016 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Drawing;
using PA.TileList.Cropping;
using PA.TileList.Quadrant;
using PA.TileList.Quantified;
using PA.TileList.Tile;

namespace PA.TileList.Tests.Utils
{
    public class MainTile : Tile<SubTile>, IQuadrant<SubTile>, IQuantifiedTile<SubTile>, ITile<SubTile>
    {
        public MainTile(IZone a, SubTile t)
            : base(a, t)
        {
        }

        public Quadrant.Quadrant Quadrant { get; private set; }

        public void SetQuadrant(Quadrant.Quadrant q)
        {
            throw new NotImplementedException();
        }

        public double ElementSizeX { get; internal set; }

        public double ElementSizeY { get; internal set; }

        public double ElementStepX { get; internal set; }

        public double ElementStepY { get; internal set; }

        public double RefOffsetX { get; internal set; }

        public double RefOffsetY { get; internal set; }

        public static MainTile GetTile(float factor)
        {
            IZone first = new Zone((int) (-5*factor), (int) (-5*factor), (int) (5*factor), (int) (5*factor));
            IZone second = new Zone(1, 1, 5, 5);

            var t1 = new SubTile(second, new Item(3, 3, Color.Red));
            t1.Fill(c => new Item(c.X, c.Y, c.X + c.Y == 6 ? Color.Green : Color.Yellow), false);


            var t0 = new MainTile(first, t1);
            t0.Fill(c =>
            {
                var a = t1.Clone(c.X, c.Y) as SubTile;
                return a;
            });

            t0.ElementSizeX = 45f/factor*second.SizeX;
            t0.ElementSizeY = 50f/factor*second.SizeY;
            t0.ElementStepX = 50f/factor*second.SizeX;
            t0.ElementStepY = 60f/factor*second.SizeY;
            t0.RefOffsetX = 25;
            t0.RefOffsetY = 0;

            return t0;
        }

        public static MainTile GetTileFullSpace(float factor)
        {
            IZone first = new Zone((int) (-5*factor), (int) (-5*factor), (int) (5*factor), (int) (5*factor));
            IZone second = new Zone(1, 1, 5, 5);

            var t1 = new SubTile(second, new Item(3, 3, Color.Red));
            t1.Fill(c => new Item(c.X, c.Y, c.X + c.Y == 6 ? Color.Green : Color.Yellow));

            var t0 = new MainTile(first, t1);
            t0.Fill(c => t1.Clone(c.X, c.Y) as SubTile);

            t0.ElementSizeX = 50f/factor*second.SizeX;
            t0.ElementSizeY = 50f/factor*second.SizeY;
            t0.ElementStepX = 50f/factor*second.SizeX;
            t0.ElementStepY = 50f/factor*second.SizeY;
            t0.RefOffsetX = 50;
            t0.RefOffsetY = 0;

            return t0;
        }
    }
}