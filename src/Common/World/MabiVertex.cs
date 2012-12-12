// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

namespace Common.World
{
	public class MabiVertex
	{
		public uint X, Y;

		public MabiVertex(uint x, uint y)
		{
			this.X = x;
			this.Y = y;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var pos = obj as MabiVertex;
			if (pos == null)
				return false;

			return (this.X == pos.X && this.Y == pos.Y);
		}

		public override int GetHashCode()
		{
			return (int)(this.X ^ this.Y);
		}

		public override string ToString()
		{
			return (this.X.ToString() + "/" + this.Y.ToString());
		}

		public MabiVertex Copy()
		{
			return new MabiVertex(this.X, this.Y);
		}

		public static MabiVertex operator -(MabiVertex v1, MabiVertex v2)
		{
			return new MabiVertex(v1.X - v2.X, v1.Y - v2.Y);
		}
	}
}
