//
// Renderable.cs
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
namespace PA.TileList.Cacheable
{
    public class Cacheable : ICacheable
    {
        private Dictionary<object, bool> _changed;
        private bool _default;

        public Cacheable()
        {
            _changed = new Dictionary<object, bool>();
            _changed.Add(null, false);
            _default = false;
        }

        public Cacheable(bool alwaysCache)
        {
            _changed = new Dictionary<object, bool>();
            _changed.Add(null, alwaysCache);
            _default = alwaysCache;
        }

        public bool IsCached()
        {
            return IsCachedBy(null);
        }

        public bool IsCachedBy(object t)
        {
            if (!_changed.ContainsKey(t))
            {
                _changed.Add(t, _default);
            }

            return _changed[t];
        }

        public void NotifyCached()
        {
            NotifyCachedBy(null);
        }

        public void NotifyCachedBy(object t)
        {
            if (!_changed.ContainsKey(t))
            {
                _changed[t] = _default;
            }
            else
            {
                _changed.Add(t, _default);
            }
        }
    }
}
