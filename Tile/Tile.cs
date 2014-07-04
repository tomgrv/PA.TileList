using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace PA.TileList
{
    public class Tile<T> : List<T>, ITile<T> where T : ICoordinate
    {
        public int X { get; set; }
        public int Y { get; set; }

        public T Reference { get; private set; }
        public IArea Area { get; private set; }

        protected Tile(ITile<T> t)
            : base(t)
        {
            this.X = t.X;
            this.Y = t.Y;
            this.Reference = t.Reference;
            this.Area = t.Area;
        }

        public Tile(IEnumerable<T> t, int referenceIndex = 0)
            : base(t)
        {
            this.X = 0;
            this.Y = 0;
            this.Reference = t.ElementAt(referenceIndex);
            this.Area = t.GetArea();
        }


        public Tile(IArea area, IEnumerable<T> t, int referenceIndex = 0)
            : base(t)
        {
            if (base.Count == 0)
            {
                throw new ArgumentNullException();
            }

            this.X = 0;
            this.Y = 0;
            this.Reference = base[referenceIndex];
            this.Area = area;
        }

        public Tile(IArea area, T reference)
            : base(new T[] { reference })
        {
            this.X = 0;
            this.Y = 0;
            this.Reference = reference;
            this.Area = area;
        }

        public Tile(int x, int y, IArea area, IEnumerable<T> t, int referenceIndex = 0)
            : base(t)
        {
            if (base.Count == 0)
            {
                throw new ArgumentNullException();
            }

            this.X = x;
            this.Y = y;
            this.Reference = base[referenceIndex];
            this.Area = area;
        }

        public Tile(int x, int y, IArea area, T reference)
            : base(new T[] { reference })
        {
            this.X = x;
            this.Y = y;
            this.Reference = reference;
            this.Area = area;
        }


        public T Find(int x, int y)
        {
            return this.Find(e => e.X == x && e.Y == y);
        }

        public T Find(ICoordinate c)
        {
            return this.Find(e => e.X == c.X && e.Y == c.Y);
        }

        public List<T> FindAll(IArea a)
        {
            return base.FindAll(e => a.Contains(e));
        }

        public void Remove(int x, int y)
        {
            this.Remove(this.Find(x, y));
        }

        public void RemoveAll(IArea a)
        {
            this.RemoveAll(e => a.Contains(e));
        }

        public void UpdateArea()
        {
            this.Area = this.GetArea();
        }

        public void Fill<U>(ushort SizeX, ushort SizeY, Func<ICoordinate, U> filler, decimal ShiftX = 0, decimal ShiftY = 0) where U : T
        {
            int StartX = Math.Min(this.Area.Max.X, Math.Max(this.Area.Min.X, Convert.ToInt32(ShiftX - SizeX / 2m)));
            int StartY = Math.Min(this.Area.Max.Y, Math.Max(this.Area.Min.Y, Convert.ToInt32(ShiftY - SizeY / 2m)));

            Area a = new Area(StartX, StartY, StartX + SizeX - 1, StartY + SizeY - 1);

            this.Fill(filler, a);
        }

        public void Fill<U>(Func<ICoordinate, U> filler, IArea a = null) where U : T, ICoordinate
        {
            this.Clear();

            IArea area = a ?? this.Area;

            for (int i = area.Min.X; i <= area.Max.X; i++)
            {
                for (int j = area.Min.Y; j <= area.Max.Y; j++)
                {
                    U e = filler(new Coordinate(i,j));
                    // reassign  to ensure coherence...
                    e.X = i;
                    e.Y = j;
                    this.Add(e);
                }
            }

            this.Reference = this.Find(this.Reference.X, this.Reference.Y);
            this.TrimExcess();
            this.UpdateArea();
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

        public virtual  ICoordinate Clone()
        {
            return this.MemberwiseClone() as ICoordinate;
        }
    }
}
