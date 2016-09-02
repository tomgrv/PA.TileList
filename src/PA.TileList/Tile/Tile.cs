using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace PA.TileList
{
    public class Tile<T> : List<T>, ITile<T>
    where T : class, ICoordinate
    {
        public int X { get; set; }

        public int Y { get; set; }

        public T Reference { get; private set; }

        public IZone Zone { get; private set; }

        protected Tile(ITile<T> t)
            : base(t)
        {
            this.X = t.X;
            this.Y = t.Y;
            this.Reference = t.Reference;
            this.Zone = t.Zone;
        }

        public Tile(IEnumerable<T> t, int referenceIndex = 0)
            : base(t)
        {
            this.X = 0;
            this.Y = 0;
            this.Reference = t.ElementAt(referenceIndex);
            this.UpdateZone();
        }


        public Tile(IZone area, IEnumerable<T> t, int referenceIndex = 0)
            : base(t)
        {
            if (base.Count == 0)
            {
                throw new ArgumentNullException();
            }

            this.X = 0;
            this.Y = 0;
            this.Reference = base[referenceIndex];
            this.Zone = area;
        }

        public Tile(IZone area, T reference)
            : base(new T[] { reference })
        {
            this.X = 0;
            this.Y = 0;
            this.Reference = base[0];
            this.Zone = area;
        }

        public Tile(int x, int y, IZone area, IEnumerable<T> t, int referenceIndex = 0)
            : base(t)
        {
            if (base.Count == 0)
            {
                throw new ArgumentNullException();
            }

            this.X = x;
            this.Y = y;
            this.Reference = base[referenceIndex];
            this.Zone = area;
        }

        public Tile(int x, int y, IZone area, T reference)
            : base(new T[] { reference })
        {
            this.X = x;
            this.Y = y;
            this.Reference = base[0];
            this.Zone = area;
        }


        public T Find(int x, int y)
        {
            return this.Find(e => e.X == x && e.Y == y);
        }

        public T Find(ICoordinate c)
        {
            return this.Find(e => e.X == c.X && e.Y == c.Y);
        }

        public List<T> FindAll(IZone a)
        {
            return base.FindAll(a.Contains);
        }

        public void Remove(int x, int y)
        {
            this.Remove(this.Find(x, y));
        }

        public void RemoveAll(IZone a)
        {
            this.RemoveAll(a.Contains);
        }

        public void UpdateZone()
        {
            this.Zone = this.GetZone();
        }

        /// <summary>
        /// Fill area  with specified filler.
        /// </summary>
        /// <param name="filler">Filler.</param>
        /// <param name="overwrite">Overwrite existing element</param>
        public void Fill(Func<Coordinate, T> filler, bool overwrite = false)
        {
            this.Fill(this.Zone, filler, overwrite);
        }


        /// <summary>
        /// Fill area  with specified filler.
        /// </summary>
        /// <param name="zone">Area.</param>
        /// <param name="filler">Filler.</param>
        /// <param name="overwrite">Overwrite existing element</param>
        public void Fill(IZone zone, Func<Coordinate, T> filler, bool overwrite = false)
        {
            foreach (Coordinate c in zone)
            {
                T item = this.FirstOrDefault(t => t.X == c.X && t.Y == c.Y);

                if (item != null && overwrite)
                {
                    this.Remove(item);
                }

                if (item == null || overwrite)
                {
                    this.Add(filler(c));
                }
            }

            this.TrimExcess();
            this.UpdateZone();
        }

        /// <summary>
        /// Fill zone with specified filler.
        /// </summary>
        /// <param name="SizeX">Size to fill</param>
        /// <param name="SizeY">Size to fill</param>
        /// <param name="filler"></param>
        /// <param name="ShiftX">Shift regarding area center</param>
        /// <param name="ShiftY">Shift regarding area center</param>
        /// <param name="overwrite">Overwrite existing element</param>
        public void Fill(ushort SizeX, ushort SizeY, Func<Coordinate, T> filler, decimal ShiftX = 0, decimal ShiftY = 0, bool overwrite = false)
        {
            int StartX = Math.Min(this.Zone.Max.X, Math.Max(this.Zone.Min.X, Convert.ToInt32(ShiftX - SizeX / 2m)));
            int StartY = Math.Min(this.Zone.Max.Y, Math.Max(this.Zone.Min.Y, Convert.ToInt32(ShiftY - SizeY / 2m)));

            var area = new Zone(StartX, StartY, StartX + SizeX - 1, StartY + SizeY - 1);

            this.Fill(area, filler, overwrite);
        }

        public IEnumerable<T> Inside(IZone a)
        {
            var zone = new Zone(a ?? this.Zone);

            foreach (Coordinate c in zone)
            {
                yield return this.Find(c.X, c.Y);
            }
        }

        public void SetReference(T reference)
        {
            if (this.Contains(reference))
            {
                this.Reference = reference;
            }
        }

        public void SetReference(int reference)
        {
            if (0 <= reference && reference < this.Count)
            {
                this.Reference = this[reference];
            }
        }

        public override string ToString()
        {
            return this.X + "," + this.Y;
        }


        public virtual ICoordinate Clone()
        {
            return this.Clone(X, Y);
        }

        public virtual ICoordinate Clone(int x, int y)
        {
            var c = this.MemberwiseClone() as ICoordinate;
            c.X = x;
            c.Y = y;
            return c;
        }
    }
}
