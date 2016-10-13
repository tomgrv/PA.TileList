﻿//
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
using System.Collections.Generic;
using PA.TileList.Cropping;
using PA.TileList.Quadrant;
using PA.TileList.Tile;

namespace PA.TileList.Tests.Utils
{
    public class SubTile : Tile<Item>, IQuadrant<Item>
    {
        public SubTile(IZone a, Item t)
            : base(a, t)
        {
        }

        public SubTile(Tile<Item> t, Quadrant.Quadrant q)
            : base(t)
        {
            this.Quadrant = q;
        }

        public SubTile(IEnumerable<Item> t, int referenceIndex = 0)
            : base(t, referenceIndex)
        {
        }

        public Quadrant.Quadrant Quadrant { get; }

        public void SetQuadrant(Quadrant.Quadrant q)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            return new SubTile((Tile<Item>) base.Clone(), this.Quadrant);
        }

        public override object Clone(int x, int y)
        {
            return new SubTile((Tile<Item>) base.Clone(x, y), this.Quadrant);
        }
    }
}