﻿using System;
using PA.TileList.Linear;
using PA.TileList.Tile;
using System.Diagnostics.Contracts;
using System.Collections.Generic;

namespace PA.TileList.Quantified
{
	public class QuantifiedTile<T> : Tile<T>, IQuantifiedTile<T>, ITile<T>
		where T : class, ICoordinate
	{
		public QuantifiedTile(IEnumerable<T> t, int referenceIndex = 0)
			: base(t, referenceIndex)
		{
			this.ElementSizeX = 1;
			this.ElementSizeY = 1;
			this.ElementStepX = 1;
			this.ElementStepY = 1;
			this.RefOffsetX = 0;
			this.RefOffsetY = 0;
		}

		public QuantifiedTile(ITile<T> t)
					: this(t, 1, 1, 1, 1, 0, 0)
		{
		}

		public QuantifiedTile(ITile<T> t, double sizeX, double sizeY)
					: this(t, sizeX, sizeY, sizeX, sizeY, 0, 0)
		{
		}

		public QuantifiedTile(ITile<T> t, double sizeX, double sizeY, double stepX, double stepY)
			: this(t, sizeX, sizeY, stepX, stepY, 0, 0)
		{
		}

		public QuantifiedTile(ITile<T> t, double sizeX, double sizeY, double stepX, double stepY, double offsetX, double offsetY)
			: base(t)
		{
			Contract.Requires(stepX > sizeX, nameof(stepX) + " must be greater than " + nameof(sizeX));
			Contract.Requires(stepY > sizeY, nameof(stepY) + " must be greater than " + nameof(sizeY));
			Contract.Requires(sizeX > 0, nameof(sizeX) + " must be positive");
			Contract.Requires(sizeY > 0, nameof(sizeY) + " must be positive");
			Contract.Requires(stepX > 0, nameof(stepX) + " must be positive");
			Contract.Requires(stepY > 0, nameof(stepY) + " must be positive");

			this.ElementSizeX = sizeX;
			this.ElementSizeY = sizeY;
			this.ElementStepX = stepX;
			this.ElementStepY = stepY;
			this.RefOffsetX = offsetX;
			this.RefOffsetY = offsetY;
		}

		public QuantifiedTile(IQuantifiedTile<T> t)
			: this(t, t.ElementSizeX, t.ElementSizeY, t.ElementStepX, t.ElementStepY, t.RefOffsetX, t.RefOffsetY)
		{
		}

		public double ElementSizeX { get; }
		public double ElementSizeY { get; }
		public double ElementStepX { get; }
		public double ElementStepY { get; }
		public double RefOffsetX { get; private set; }
		public double RefOffsetY { get; private set; }

		public override void SetReference(T reference)
		{
			if (this.Contains(reference))
			{
				var n = new Coordinate(reference.X - this.Reference.X, reference.Y - this.Reference.Y);

				this.RefOffsetX += n.X * this.ElementStepX;
				this.RefOffsetY += n.Y * this.ElementStepY;

				base.SetReference(reference);
			}
		}

		public override object Clone()
		{
			return new QuantifiedTile<T>((Tile<T>)base.Clone(), this.ElementSizeX, this.ElementSizeY, this.ElementStepX, this.ElementStepY, this.RefOffsetX, this.RefOffsetY);
		}

		public override object Clone(int x, int y)
		{
			return new QuantifiedTile<T>((Tile<T>)base.Clone(x, y), this.ElementSizeX, this.ElementSizeY, this.ElementStepX, this.ElementStepY, this.RefOffsetX, this.RefOffsetY);
		}
	}
}