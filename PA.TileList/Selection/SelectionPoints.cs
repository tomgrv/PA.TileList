//
// SelectionPoints.cs
//
// Author:
//       Thomas <tomgrv@users.perspikapps.fr>
//
// Copyright (c) 2020 
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

namespace PA.TileList.Selection
{
    public class SelectionPoints
    {
        public uint Inside { get; set; }
        public uint Outside { get; set; }
        public uint Under { get; set; }

        [Obsolete]
        public SelectionPosition GetPosition()
        {
            return (Outside > 0 ? SelectionPosition.Outside : 0x00) | (Inside > 0 ? SelectionPosition.Inside : 0x00) |
                   (Under > 0 ? SelectionPosition.Under : 0x00);
        }

        public uint Count()
        {
            return Outside + Inside + Under;
        }

        public uint Count(SelectionPosition selectionType)
        {
            uint points = 0;

            //SelectionPosition current = this.GetPosition();
            if (Outside > 0 && selectionType.HasFlag(SelectionPosition.Outside))
                points += Outside;

            if (Inside > 0 && selectionType.HasFlag(SelectionPosition.Inside))
                points += Inside;

            if (Under > 0 && selectionType.HasFlag(SelectionPosition.Under))
                points += Under;

            return points;
        }

        public bool IsSelected(SelectionConfiguration config)
        {
            if (config.SelectionType.HasFlag(SelectionPosition.Under))
                return this.IsAmbiguous() || this.GetSurface(config) >0;
            else
                return this.GetSurface(config) >0;
        }

        public bool IsAmbiguous()
        {
            return (Outside > 0 && Inside > 0) || Under > 0;
        }

        public uint GetSurface(SelectionConfiguration config)
        {
            var surface = Count(config.SelectionType);
            return surface >= config.MinSurface ? surface : 0;
        }

        public override string ToString()
        {
            return "[" + Inside + ", " + Under + ", " + Outside + "]";
        }
    }
}