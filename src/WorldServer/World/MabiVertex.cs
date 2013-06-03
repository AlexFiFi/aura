// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Drawing;
using System;

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

		public MabiVertex(float x, float y, float h = 0)
		{
			this.X = (uint)x;
			this.Y = (uint)y;
			this.H = (uint)h;
		}

		public static MabiVertex operator +(MabiVertex a, MabiVertex b)
		{
			return new MabiVertex(a.X + b.X, a.Y + b.Y);
		}

		public static MabiVertex operator -(MabiVertex a, MabiVertex b)
		{
			return new MabiVertex(a.X - b.X, a.Y - b.Y);
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

	/// <summary>
	/// Holding two points, making up a path.
	/// </summary>
	public class LinePath : IQuadtreeObject
	{
		public Point P1 { get; protected set; }
		public Point P2 { get; protected set; }
		public Rectangle Rect { get; protected set; }

		public LinePath(MabiVertex p1, MabiVertex p2)
		{
			this.P1 = new Point((int)p1.X, (int)p1.Y);
			this.P2 = new Point((int)p2.X, (int)p2.Y);
			this.Rect = new Rectangle(
				(P1.X < P2.X ? P1.X : P2.X),
				(P1.Y < P2.Y ? P1.Y : P2.Y),
				Math.Abs(P1.X - P2.X),
				Math.Abs(P1.Y - P2.Y)
			);
		}
	}
}
