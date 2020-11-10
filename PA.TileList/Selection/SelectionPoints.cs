//
// SelectionPoints.cs
//
// Author:
//       Thomas GERVAIS <thomas.gervais@gmail.com>
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
		public uint Inside { get; set;}
		public uint Outside { get; set;}
		public uint Under { get; set;}
		
		public SelectionPoints()
		{
		}

		public SelectionPosition GetPosition()
        {
			return (this.Outside > 0 ? SelectionPosition.Outside : 0x00) | (this.Inside > 0 ? SelectionPosition.Inside : 0x00) | (this.Under > 0 ? SelectionPosition.Under : 0x00);
		}

		public uint Count()
		{
			return this.Outside + this.Inside + this.Under;
		}

		public uint Count(SelectionPosition selectionType)
        {
			uint points = 0;

			//SelectionPosition current = this.GetPosition();
			if (this.Outside > 0 && selectionType.HasFlag(SelectionPosition.Outside))
				points += this.Outside;

			if (this.Inside > 0 && selectionType.HasFlag(SelectionPosition.Inside))
				points += this.Inside;

			if (this.Under > 0 && selectionType.HasFlag(SelectionPosition.Under))
				points += this.Under;

			return points;
		}

		public bool IsSelected(SelectionConfiguration config)
		{
			return this.Count(config.SelectionType) >= config.MinSurface ;
		}

		public uint GetSurface(SelectionConfiguration config)
        {
			uint surface = this.Count(config.SelectionType);
			return surface >= config.MinSurface ? surface : 0;
		}

        public override string ToString()
        {
			return "[" + this.Inside + ", " + this.Under + ", " + this.Outside + "]";

		}
    }
}
