﻿//
// MainTile.cs
//
// Author:
//       Thomas <tomgrv@users.perspikapps.fr>
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
using System.Drawing;
using PA.TileList.Cacheable;
using PA.TileList.Cropping;
using PA.TileList.Drawing.Graphics2D;
using PA.TileList.Drawing.Quantified;
using PA.TileList.Quadrant;
using PA.TileList.Quantified;
using PA.TileList.Tile;

namespace PA.TileList.Tests.Utils
{
    public class SubTile : Tile<Item>, IQuadrant<Item>, ICacheable
    {
        public SubTile(IZone a, Item t)
            : base(a, t)
        {
            _changed = new Dictionary<object, bool>();
            _default = false;
        }

        public SubTile(Tile<Item> t, Quadrant.Quadrant q)
            : base(t)
        {
            _changed = new Dictionary<object, bool>();
            _default = false;
            Quadrant = q;
        }

        public SubTile(IEnumerable<Item> t, int referenceIndex = 0)
            : base(t, referenceIndex)
        {
            _changed = new Dictionary<object, bool>();
            _default = false;
        }

        public double ElementSizeX { get; internal set; }

        public double ElementSizeY { get; internal set; }

        public double ElementStepX { get; internal set; }

        public double ElementStepY { get; internal set; }

        public double RefOffsetX { get; internal set; }

        public double RefOffsetY { get; internal set; }

        public Quadrant.Quadrant Quadrant { get; }


        public void SetQuadrant(Quadrant.Quadrant q)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            return new SubTile((Tile<Item>) base.Clone(), Quadrant);
        }

        public override object Clone(int x, int y)
        {
            return new SubTile((Tile<Item>) base.Clone(x, y), Quadrant);
        }

        public Bitmap ToBitmap(int w, int h, IQuantifiedTile m, Pen p = null)
        {
            return this.ToQuantified(m.ElementSizeX / Zone.SizeX, m.ElementSizeY / Zone.SizeY,
                    m.ElementStepX / Zone.SizeX, m.ElementStepY / Zone.SizeY)
                .RenderImage(w, h, ScaleMode.STRETCH, new QuantifiedRenderer<Item>(
                    (z, s) => z.ToBitmap((int) s.Width, (int) s.Height, z.X + "\n" + z.Y), p)
                ).Item;
        }

        public void NotifyRendered()
        {
        }

        #region Cache

        private readonly Dictionary<object, bool> _changed;
        private readonly bool _default;

        public bool IsCached()
        {
            return IsCachedBy(null);
        }

        public bool IsCachedBy(object t)
        {
            if (!_changed.ContainsKey(t)) _changed.Add(t, _default);

            return _changed[t];
        }

        public void NotifyCached()
        {
            NotifyCachedBy(null);
        }

        public void NotifyCachedBy(object t)
        {
            if (!_changed.ContainsKey(t))
                _changed[t] = _default;
            else
                _changed.Add(t, _default);
        }

        #endregion
    }
}