using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList.Contextual
{
    public class Contextual<T> : Coordinate, IContextual<T>
        where T : ICoordinate
    {

        public T Context { get; private set; }

        public Contextual(int x, int y, T context)
            : base(x, y)
        {
            this.Context = context;
        }

        public override string ToString()
        {
            return base.ToString() + " [" + this.Context.ToString() + "]";
        }

        public static IContextual<T> FromContext(T e)
        {
            return new Contextual<T>(e.X, e.Y, e);
        }
    }
}
