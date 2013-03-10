// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

namespace Aura.World.World
{
	public class MabiVertex
	{
		public uint X, Y, H;

		public MabiVertex(uint x, uint y, uint h = 0)
		{
			this.X = x;
			this.Y = y;
			this.H = h;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var pos = obj as MabiVertex;
			if (pos == null)
				return false;

			return (this.X == pos.X && this.Y == pos.Y && this.H == pos.H);
		}

		public override int GetHashCode()
		{
			return (int)(this.X ^ this.Y);
		}

		public override string ToString()
		{
			return (this.X.ToString() + "/" + this.Y.ToString() + " @ " + this.H.ToString());
		}

		public MabiVertex Copy()
		{
			return new MabiVertex(this.X, this.Y, this.H);
		}
	}
}
