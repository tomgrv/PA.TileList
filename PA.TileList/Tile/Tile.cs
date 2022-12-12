using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using PA.TileList.Cropping;
using PA.TileList.Linear;

namespace PA.TileList.Tile
{
    public class Tile<T> : List<T>, ITile<T>
        where T : class, ICoordinate
    {
        private T _reference;

        protected Tile(ITile<T> t)
            : base(t)
        {
            Contract.Requires(t != null, nameof(t));

            X = t.X;
            Y = t.Y;
            Reference = t.Reference;
            Zone = t.Zone;
        }

        public Tile(IEnumerable<T> t, int referenceIndex = 0)
            : base(t)
        {
            Contract.Requires(t != null, nameof(t));
            Contract.Requires(referenceIndex >= 0, nameof(referenceIndex));
            Contract.Requires(referenceIndex < Count, nameof(referenceIndex));


            X = 0;
            Y = 0;
            Reference = base[referenceIndex];
            UpdateZone();
        }


        public Tile(IZone zone, IEnumerable<T> t, int referenceIndex = 0)
            : base(t)
        {
            Contract.Requires(t != null, nameof(t));
            Contract.Requires(zone != null, nameof(zone));
            Contract.Requires(referenceIndex >= 0, nameof(referenceIndex));
            Contract.Requires(referenceIndex < Count, nameof(referenceIndex));

            X = 0;
            Y = 0;
            Reference = base[referenceIndex];
            Zone = new Zone(zone);
        }

        public Tile(IZone zone, T reference)
            : base(new[] {reference})
        {
            Contract.Requires(zone != null, nameof(zone));
            Contract.Requires(reference != null, nameof(reference));

            X = 0;
            Y = 0;
            Reference = base[0];
            Zone = new Zone(zone);
        }

        public Tile(int x, int y, IZone zone, IEnumerable<T> t, int referenceIndex = 0)
            : base(t)
        {
            Contract.Requires(t != null, nameof(t));
            Contract.Requires(zone != null, nameof(zone));
            Contract.Requires(referenceIndex >= 0, nameof(referenceIndex));
            Contract.Requires(referenceIndex < Count, nameof(referenceIndex));

            X = x;
            Y = y;
            Reference = base[referenceIndex];
            Zone = new Zone(zone);
        }

        public Tile(int x, int y, IZone zone, T reference)
            : base(new[] {reference})
        {
            Contract.Requires(reference != null, nameof(reference));
            Contract.Requires(zone != null, nameof(zone));

            X = x;
            Y = y;
            Reference = base[0];
            Zone = new Zone(zone);
        }
#if DEBUG
        public object Tag { get; set; }
#endif

        public int X { get; set; }

        public int Y { get; set; }

        public T Reference
        {
            get => _reference == null || IndexOf(_reference) < 0 ? null : _reference;
            private set
            {
                Contract.Requires(Contains(value));
                _reference = value;
            }
        }

        public Zone Zone { get; private set; }

        public void UpdateZone()
        {
            Zone = this.GetZone();
        }

        public virtual object Clone()
        {
            return new Tile<T>(X, Y, Zone, this.Select(t => (T) t.Clone()), IndexOf(Reference));
        }

        public virtual object Clone(int x, int y)
        {
            return new Tile<T>(x, y, Zone, this.Select(t => (T) t.Clone()), IndexOf(Reference));
        }

        public void RemoveAll(IEnumerable<T> list)
        {
            var toRemove = list.ToArray();

            if (toRemove.Contains(Reference))
            {
                var r = this.Except(toRemove).First();
                SetReference(r);
            }

            foreach (var e in toRemove)
                base.Remove(e);

            UpdateZone();
        }

        public T FindOrCreate(int x, int y, Func<T> creator)
        {
            Contract.Requires(creator != null);

            var item = Find(x, y);

            if (item == null)
            {
                item = creator();
                item.X = x;
                item.Y = y;
                Add(item);
                UpdateZone();
            }

            return item;
        }

        public T FindOrCreate<U>(int x, int y)
            where U : T, new()
        {
            var item = Find(x, y);

            if (item == null)
            {
                item = new U
                {
                    X = x,
                    Y = y
                };
                Add(item);
                UpdateZone();
            }

            return item;
        }

        public T Find(int x, int y)
        {
            return Find(e => e.X == x && e.Y == y);
        }

        public T Find(ICoordinate c)
        {
            Contract.Requires(c != null);

            return Find(e => e.X == c.X && e.Y == c.Y);
        }

        public T FindOrCreate(ICoordinate c, Func<T> creator)
        {
            Contract.Requires(c != null);
            Contract.Requires(creator != null);

            return FindOrCreate(c.X, c.Y, creator);
        }

        public T FindOrCreate<U>(ICoordinate c)
            where U : T, new()
        {
            Contract.Requires(c != null);

            return FindOrCreate<U>(c.X, c.Y);
        }


        public List<T> FindAll(IZone zone)
        {
            Contract.Requires(zone != null);

            return base.FindAll(zone.Contains);
        }


        public void Remove(int x, int y)
        {
            var f = Find(x, y);

            if (Reference.Equals(f))
                SetReference(this.First(e => !e.Equals(f)));

            Remove(f);

            UpdateZone();
        }

        public void Remove(ICoordinate c)
        {
            var f = Find(c);

            if (Reference.Equals(f))
                SetReference(this.First(e => !e.Equals(f)));

            base.Remove(f);

            UpdateZone();
        }


        public void RemoveAll(IZone zone)
        {
            RemoveAll(this.Where(e => zone.Contains(e)));
        }

        /// <summary>
        ///     Fill area  with specified filler.
        /// </summary>
        /// <param name="filler">Filler.</param>
        /// <param name="overwrite">Overwrite existing element</param>
        public void Fill(Func<Coordinate, T> filler, bool overwrite = false)
        {
            Contract.Requires(filler != null);

            Fill(Zone, filler, overwrite);
        }


        public void Fill<U>(bool overwrite = false)
            where U : T, new()
        {
            Fill(c =>
            {
                var i = new U
                {
                    X = c.X,
                    Y = c.Y
                };
                return i;
            }, overwrite);
        }


        /// <summary>
        ///     Fill area  with specified filler.
        /// </summary>
        /// <param name="zone">Area.</param>
        /// <param name="filler">Filler.</param>
        /// <param name="overwrite">Overwrite existing element</param>
        public void Fill(IZone zone, Func<Coordinate, T> filler, bool overwrite = false)
        {
            Contract.Requires(zone != null);
            Contract.Requires(filler != null);

            foreach (var c in zone)
            {
                var item = Find(c);

                if (item != null && overwrite)
                    Remove(item);

                if (item == null || overwrite)
                    Add(filler(c));
            }

            TrimExcess();
            UpdateZone();
        }


        public void Fill<U>(IZone zone, bool overwrite = false)
            where U : T, new()
        {
            Contract.Requires(zone != null);

            Fill(Zone, c =>
            {
                var i = new U
                {
                    X = c.X,
                    Y = c.Y
                };
                return i;
            }, overwrite);
        }

        /// <summary>
        ///     Fill zone with specified filler.
        /// </summary>
        /// <param name="SizeX">Size to fill</param>
        /// <param name="SizeY">Size to fill</param>
        /// <param name="filler"></param>
        /// <param name="ShiftX">Shift regarding area center</param>
        /// <param name="ShiftY">Shift regarding area center</param>
        /// <param name="overwrite">Overwrite existing element</param>
        public void Fill(ushort SizeX, ushort SizeY, Func<Coordinate, T> filler, decimal ShiftX = 0, decimal ShiftY = 0,
            bool overwrite = false)
        {
            Contract.Requires(filler != null);


            var StartX = Math.Min(Zone.Max.X, Math.Max(Zone.Min.X, Convert.ToInt32(ShiftX - SizeX / 2m)));
            var StartY = Math.Min(Zone.Max.Y, Math.Max(Zone.Min.Y, Convert.ToInt32(ShiftY - SizeY / 2m)));

            var area = new Zone(StartX, StartY, StartX + SizeX - 1, StartY + SizeY - 1);

            Fill(area, filler, overwrite);
        }


        public void Fill<U>(ushort SizeX, ushort SizeY, decimal ShiftX = 0, decimal ShiftY = 0,
            bool overwrite = false)
            where U : T, new()
        {
            Fill(SizeX, SizeY, c =>
            {
                var i = new U
                {
                    X = c.X,
                    Y = c.Y
                };
                return i;
            }, ShiftX, ShiftY, overwrite);
        }

        public IEnumerable<T> Inside(IZone a)
        {
            Contract.Requires(a != null);

            var zone = new Zone(a ?? Zone);

            foreach (var c in zone)
                yield return Find(c.X, c.Y);
        }

        public override string ToString()
        {
            return X + "," + Y;
        }

        #region Reference

        public ICoordinate GetReference()
        {
            return Reference;
        }

        public void SetReference(ICoordinate reference)
        {
            Contract.Requires(reference != null, "Reference must be specified");

            SetReference(Find(reference));
        }

        public virtual void SetReference(T reference)
        {
            Contract.Requires(reference != null, "Reference must be specified");
            Contract.Requires(Contains(reference), "Reference must belong to Tile");

            if (Contains(reference)) Reference = reference;
        }

        #endregion
    }
}