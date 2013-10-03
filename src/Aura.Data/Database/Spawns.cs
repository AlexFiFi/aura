// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;
using System.Linq;
using System;

namespace Aura.Data
{
	public class Line
	{
		public double Slope { get; protected set; }
		public double B { get; protected set; }
		public Point Point1 { get; protected set; }
		public Point Point2 { get; protected set; }

		public uint MinX { get; protected set; }
		public uint MaxX { get; protected set; }

		public Line(Point p1, Point p2)
		{
			if (p1.X == p2.X)
				this.Slope = .001; // double.MinValue produdes a B of Infinity.
			else
				this.Slope = ((double)p2.Y - p1.Y) / ((double)p2.X - p1.X);

			this.B = p1.Y - Slope * p1.X;

			this.Point1 = p1;
			this.Point2 = p2;

			if (p1.X < p2.X)
			{
				this.MinX = p1.X;
				this.MaxX = p2.X;
			}
			else
			{
				this.MinX = p2.X;
				this.MaxX = p1.X;
			}
		}

		public bool InDomain(uint x)
		{
			return this.MinX <= x && x <= this.MaxX;
		}

		public uint Evaluate(uint x)
		{
			return (uint)(this.Slope * x + this.B);
		}
	}

	public class SpawnRegion
	{
		private List<Line> _edges = new List<Line>();
		private uint _minX, _maxX, _minY, _maxY;

		public SpawnRegion(params Point[] points)
		{
			if (points.Length < 3)
				throw new ArgumentException("A polygon must have a minimum of 3 verticies.");

			for (int i = 1; i < points.Length; i++)
			{
				_edges.Add(new Line(points[i - 1], points[i]));

				if (points[i].X < _minX)
					_minX = (uint)points[i].X;
				else if (points[i].X > _maxX)
					_maxX = (uint)points[i].X;

				if (points[i].Y < _minY)
					_minY = (uint)points[i].Y;
				else if (points[i].Y > _maxY)
					_maxY = (uint)points[i].Y;
			}
			_edges.Add(new Line(points[points.Length - 1], points[0]));
		}

		public bool InRegion(Point point)
		{
			return this.InRegion(point.X, point.Y);
		}

		public bool InRegion(uint x, uint y)
		{
			return _edges.Count(l => l.InDomain(x) && l.Evaluate(x) > y) % 2 != 0;
		}

		public RectangleF GetBounds()
		{
			return new RectangleF(_minX, _minY, _maxX - _minX, _maxY - _minY);
		}
	}

	public enum SpawnLocationType { Point, Line, Polygon }

	public class SpawnInfo
	{
		public uint Id;
		public uint RaceId;
		public uint Region;
		public SpawnLocationType SpawnType;
		public Point SpawnPoint;
		public Line SpawnLine;
		public SpawnRegion SpawnPolyRegion;
		public RectangleF SpawnPolyBounds;
		public uint Amount;

		public Point GetRandomSpawnPoint(Random rnd)
		{
			uint x, y;
			if (this.SpawnType == SpawnLocationType.Point)
			{
				return this.SpawnPoint;
			}
			else if (this.SpawnType == SpawnLocationType.Line)
			{
				x = (uint)rnd.Next((int)this.SpawnLine.MinX, (int)this.SpawnLine.MaxX);
				y = this.SpawnLine.Evaluate(x);
			}
			else
			{
				do
				{
					x = (uint)rnd.Next((int)this.SpawnPolyBounds.X, (int)(this.SpawnPolyBounds.X + this.SpawnPolyBounds.Width));
					y = (uint)rnd.Next((int)this.SpawnPolyBounds.Y, (int)(this.SpawnPolyBounds.Y + this.SpawnPolyBounds.Height));
				}
				while (!SpawnPolyRegion.InRegion(x, y));
			}

			return new Point(x, y);
		}
	}

	public class SpawnDb : DatabaseCSVIndexed<uint, SpawnInfo>
	{
		private uint _spawnId = 1;

		protected override void ReadEntry(CSVEntry entry)
		{
			if (entry.Count < 5)
				throw new FieldCountException(5);

			if ((entry.Count - 3) % 2 != 0)
				throw new DatabaseWarningException("Incomplete spawn area specification.");

			var info = new SpawnInfo();
			info.Id = _spawnId++;
			info.RaceId = entry.ReadUInt();
			info.Amount = entry.ReadUByte();
			info.Region = entry.ReadUInt();

			switch (entry.Remaining)
			{
				case 2: // Point
					info.SpawnType = SpawnLocationType.Point;
					info.SpawnPoint = new Point(entry.ReadUInt(), entry.ReadUInt());
					break;

				case 4: // Line
					info.SpawnType = SpawnLocationType.Line;
					info.SpawnLine = new Line(new Point(entry.ReadUInt(), entry.ReadUInt()), new Point(entry.ReadUInt(), entry.ReadUInt()));
					break;

				default: // Polygon
					info.SpawnType = SpawnLocationType.Polygon;

					var points = new List<Point>();
					while (!entry.End)
						points.Add(new Point(entry.ReadUInt(), entry.ReadUInt()));

					info.SpawnPolyRegion = new SpawnRegion(points.ToArray());
					info.SpawnPolyBounds = info.SpawnPolyRegion.GetBounds();

					break;
			}

			this.Entries.Add(info.Id, info);
		}
	}

	public struct RectangleF
	{
		public float X;
		public float Y;
		public float Width;
		public float Height;

		public RectangleF(float x, float y, float width, float height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}
	}

	public struct Point
	{
		public uint X;
		public uint Y;

		public Point(uint x, uint y)
		{
			this.X = x;
			this.Y = y;
		}
	}
}
