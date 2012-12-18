// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Common.Tools;
using Common.World;

namespace Common.Data
{
	public class Line
	{
		public double Slope { get; protected set; }
		public double B { get; protected set; }
		public MabiVertex Point1 { get; protected set; }
		public MabiVertex Point2 { get; protected set; }

		public uint MinX { get; protected set; }
		public uint MaxX { get; protected set; }

		public Line(MabiVertex p1, MabiVertex p2)
		{
			if (p1.X == p2.X)
				this.Slope = .001; //double.MinValue produdes a B of Infinity.
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
		public Line(Point p1, Point p2)
			: this(new MabiVertex((uint)p1.X, (uint)p1.Y), new MabiVertex((uint)p2.X, (uint)p2.Y))
		{

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

		public SpawnRegion(params MabiVertex[] points)
		{
			if (points.Length < 3)
				throw new ArgumentException("A polygon must have a minimum of 3 verticies.");
			for (int i = 1; i < points.Length; i++)
			{
				_edges.Add(new Line(points[i - 1], points[i]));

				if (points[i].X < _minX)
					_minX = points[i].X;
				else if (points[i].X > _maxX)
					_maxX = points[i].X;

				if (points[i].Y < _minY)
					_minY = points[i].Y;
				else if (points[i].Y > _maxY)
					_maxY = points[i].Y;
			}
			_edges.Add(new Line(points[points.Length - 1], points[0]));
		}
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

		public bool InRegion(MabiVertex point)
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
		public MabiVertex SpawnPoint;
		public Line SpawnLine;
		public SpawnRegion SpawnPolyRegion;
		public RectangleF SpawnPolyBounds;
		public byte Amount;

		public MabiVertex GetRandomSpawnPoint()
		{
			return this.GetRandomSpawnPoint(RandomProvider.Get());
		}

		public MabiVertex GetRandomSpawnPoint(Random rand)
		{
			uint x, y;
			if (this.SpawnType == SpawnLocationType.Point)
			{
				return this.SpawnPoint;
			}
			else if (this.SpawnType == SpawnLocationType.Line)
			{
				x = (uint)rand.Next((int)this.SpawnLine.MinX, (int)this.SpawnLine.MaxX);
				y = this.SpawnLine.Evaluate(x);
			}
			else
			{
				do
				{
					x = (uint)rand.Next((int)this.SpawnPolyBounds.X, (int)(this.SpawnPolyBounds.X + this.SpawnPolyBounds.Width));
					y = (uint)rand.Next((int)this.SpawnPolyBounds.Y, (int)(this.SpawnPolyBounds.Y + this.SpawnPolyBounds.Height));
				}
				while (!SpawnPolyRegion.InRegion(x, y));
			}

			return new MabiVertex(x, y);
		}

	}

	public class SpawnDb : DataManager<SpawnInfo>
	{
		private uint _spawnId = 1;

		public SpawnInfo Find(uint id)
		{
			return this.Entries.FirstOrDefault(a => a.Id == id);
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 5);
		}

		protected override void CsvToEntry(SpawnInfo info, List<string> csv, int currentLine)
		{
			if ((csv.Count - 3) % 2 != 0)
			{
				Logger.Warning("Incomplete area specification on line " + currentLine.ToString() + " in spawns, skipping.");
				return;
			}

			var i = 0;
			info.Id = _spawnId++;
			info.RaceId = Convert.ToUInt32(csv[i++]);
			info.Amount = Convert.ToByte(csv[i++]);
			info.Region = Convert.ToUInt32(csv[i++]);

			switch (csv.Count - i)
			{
				case 2: // Point
					info.SpawnType = SpawnLocationType.Point;
					info.SpawnPoint = new MabiVertex(Convert.ToUInt32(csv[i++]), Convert.ToUInt32(csv[i++]));
					break;

				case 4: // Line
					info.SpawnType = SpawnLocationType.Line;
					info.SpawnLine = new Line(new MabiVertex(Convert.ToUInt32(csv[i++]), Convert.ToUInt32(csv[i++])), new MabiVertex(Convert.ToUInt32(csv[i++]), Convert.ToUInt32(csv[i++])));
					break;

				default: // Polygon
					info.SpawnType = SpawnLocationType.Polygon;

					var points = new List<MabiVertex>();
					while (i < csv.Count)
						points.Add(new MabiVertex(Convert.ToUInt32(csv[i++]), Convert.ToUInt32(csv[i++])));

					info.SpawnPolyRegion = new SpawnRegion(points.ToArray());
					info.SpawnPolyBounds = info.SpawnPolyRegion.GetBounds();

					break;
			}
		}
	}
}
