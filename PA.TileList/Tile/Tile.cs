using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using PA.TileList.Cropping;
using PA.TileList.Linear;
using System.Xml.Linq;

namespace PA.TileList.Tile
{
	public class Tile<T> : List<T>, ITile<T>
		where T : class, ICoordinate
	{
#if DEBUG
		public object Tag { get; set; }
#endif

		protected Tile(ITile<T> t)
			: base(t)
		{
			Contract.Requires(t != null, nameof(t));

			this.X = t.X;
			this.Y = t.Y;
			this.Reference = t.Reference;
			this.Zone = t.Zone;
		}

		public Tile(IEnumerable<T> t, int referenceIndex = 0)
			: base(t)
		{
			Contract.Requires(t != null, nameof(t));
			Contract.Requires(referenceIndex >= 0, nameof(referenceIndex));
			Contract.Requires(referenceIndex< this.Count, nameof(referenceIndex));


			this.X = 0;
			this.Y = 0;
			this.Reference = base[referenceIndex];
			this.UpdateZone();
		}


		public Tile(IZone zone, IEnumerable<T> t, int referenceIndex = 0)
			: base(t)
		{
			Contract.Requires(t != null, nameof(t));
			Contract.Requires(zone != null, nameof(zone));
			Contract.Requires(referenceIndex >= 0, nameof(referenceIndex));
			Contract.Requires(referenceIndex< this.Count, nameof(referenceIndex));

			this.X = 0;
			this.Y = 0;
			this.Reference = base[referenceIndex];
			this.Zone = new Zone(zone);
		}

		public Tile(IZone zone, T reference)
			: base(new[] { reference })
		{
			Contract.Requires(zone != null, nameof(zone));
			Contract.Requires(reference != null, nameof(reference));

			this.X = 0;
			this.Y = 0;
			this.Reference = base[0];
			this.Zone = new Zone(zone);
		}

		public Tile(int x, int y, IZone zone, IEnumerable<T> t, int referenceIndex = 0)
			: base(t)
		{
			Contract.Requires(t != null, nameof(t));
			Contract.Requires(zone != null, nameof(zone));
			Contract.Requires(referenceIndex >= 0, nameof(referenceIndex));
			Contract.Requires(referenceIndex < this.Count, nameof(referenceIndex));

			this.X = x;
			this.Y = y;
			this.Reference = base[referenceIndex];
			this.Zone = new Zone(zone);
		}

		public Tile(int x, int y, IZone zone, T reference)
			: base(new[] { reference })
		{
			Contract.Requires(reference != null, nameof(reference));
			Contract.Requires(zone != null, nameof(zone));

			this.X = x;
			this.Y = y;
			this.Reference = base[0];
			this.Zone = new Zone(zone);
		}

		public int X { get; set; }

		public int Y { get; set; }

		private T _reference;

		public T Reference
		{
			get
			{
				return (this._reference == null) || (this.IndexOf(this._reference) < 0) ? null : this._reference;
			}
			private set
			{
				Contract.Requires(this.Contains(value));
				this._reference = value;
			}
		}

		public Zone Zone { get; private set; }

		public void UpdateZone()
		{
			this.Zone = this.GetZone();
		}

		public virtual object Clone()
		{
			return new Tile<T>(this.X, this.Y, this.Zone, this.Select(t => (T)t.Clone()), this.IndexOf(this.Reference));
		}

		public virtual object Clone(int x, int y)
		{
			return new Tile<T>(x, y, this.Zone, this.Select(t => (T)t.Clone()), this.IndexOf(this.Reference));
		}

		public T FindOrCreate(int x, int y, Func<T> creator)
		{
			Contract.Requires(creator != null);

			var item = this.Find(x, y);

			if (item == null)
			{
				item = creator();
				item.X = x;
				item.Y = y;
				this.Add(item);
				this.UpdateZone();
			}

			return item;
		}

		public T FindOrCreate<U>(int x, int y)
			where U : T, new()
		{
			var item = this.Find(x, y);

			if (item == null)
			{
				item = new U();
				item.X = x;
				item.Y = y;
				this.Add(item);
				this.UpdateZone();
			}

			return item;
		}

		public T Find(int x, int y)
		{
			return this.Find(e => (e.X == x) && (e.Y == y));
		}

		public T Find(ICoordinate c)
		{
			Contract.Requires(c != null);

			return this.Find(e => (e.X == c.X) && (e.Y == c.Y));
		}

		public T FindOrCreate(ICoordinate c, Func<T> creator)
		{
			Contract.Requires(c != null);
			Contract.Requires(creator != null);

			return this.FindOrCreate(c.X, c.Y, creator);
		}

		public T FindOrCreate<U>(ICoordinate c)
			where U : T, new()
		{
			Contract.Requires(c != null);

			return this.FindOrCreate<U>(c.X, c.Y);
		}


		public List<T> FindAll(IZone zone)
		{
			Contract.Requires(zone != null);

			return base.FindAll(zone.Contains);
		}


		public void Remove(int x, int y)
		{
			var f = this.Find(x, y);

			if (this.Reference.Equals(f))
				this.SetReference(this.First(e => !e.Equals(f)));

			this.Remove(f);

			this.UpdateZone();
		}

		public void Remove(ICoordinate c)
		{
			var f = this.Find(c);

			if (this.Reference.Equals(f))
				this.SetReference(this.First(e => !e.Equals(f)));

			base.Remove(f);

			this.UpdateZone();
		}


		public void RemoveAll(IZone zone)
		{
			this.RemoveAll(this.Where(e => zone.Contains(e)));
		}

		public void RemoveAll(IEnumerable<T> list)
		{
			var toRemove = list.ToArray();

			if (toRemove.Contains(this.Reference))
			{
				var r = this.Except(toRemove).First();
				this.SetReference(r);
			}

			foreach (var e in toRemove)
				base.Remove(e);

			this.UpdateZone();
		}

		/// <summary>
		///     Fill area  with specified filler.
		/// </summary>
		/// <param name="filler">Filler.</param>
		/// <param name="overwrite">Overwrite existing element</param>
		public void Fill(Func<Coordinate, T> filler, bool overwrite = false)
		{
			Contract.Requires(filler != null);

			this.Fill(this.Zone, filler, overwrite);
		}


		public void Fill<U>(bool overwrite = false)
		where U : T, new()
		{
			this.Fill((c) =>
		   {
			   var i = new U();
			   i.X = c.X;
			   i.Y = c.Y;
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

			foreach (Coordinate c in zone)
			{
				var item = this.Find(c);

				if ((item != null) && overwrite)
					this.Remove(item);

				if ((item == null) || overwrite)
					this.Add(filler(c));
			}

			this.TrimExcess();
			this.UpdateZone();
		}


		public void Fill<U>(IZone zone, bool overwrite = false)
		where U : T, new()
		{
			Contract.Requires(zone != null);

			this.Fill(this.Zone, (c) =>
			{
				var i = new U();
				i.X = c.X;
				i.Y = c.Y;
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


			var StartX = Math.Min(this.Zone.Max.X, Math.Max(this.Zone.Min.X, Convert.ToInt32(ShiftX - SizeX / 2m)));
			var StartY = Math.Min(this.Zone.Max.Y, Math.Max(this.Zone.Min.Y, Convert.ToInt32(ShiftY - SizeY / 2m)));

			var area = new Zone(StartX, StartY, StartX + SizeX - 1, StartY + SizeY - 1);

			this.Fill(area, filler, overwrite);
		}


		public void Fill<U>(ushort SizeX, ushort SizeY, decimal ShiftX = 0, decimal ShiftY = 0,
		bool overwrite = false)
		where U : T, new()
		{
			this.Fill(SizeX, SizeY, (c) =>
					   {
						   var i = new U();
						   i.X = c.X;
						   i.Y = c.Y;
						   return i;
					   }, ShiftX, ShiftY, overwrite);
		}

		public IEnumerable<T> Inside(IZone a)
		{
			Contract.Requires(a != null);

			var zone = new Zone(a ?? this.Zone);

			foreach (var c in zone)
				yield return this.Find(c.X, c.Y);
		}

		public override string ToString()
		{
			return this.X + "," + this.Y;
		}

		#region Reference

		public ICoordinate GetReference()
		{
			return this.Reference;
		}

		public void SetReference(ICoordinate reference)
		{
			Contract.Requires(reference != null, "Reference must be specified");

			this.SetReference(this.Find(reference));
		}

		public virtual void SetReference(T reference)
		{
			Contract.Requires(reference != null, "Reference must be specified");
			Contract.Requires(this.Contains(reference), "Reference must belong to Tile");

			if (this.Contains(reference))
			{
				this.Reference = reference;
			}
		}

		#endregion
	}
}