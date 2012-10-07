// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Common.World;
using Common.Tools;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Common.Data
{
	public enum SpawnLocationType
	{
		Point,
		Line,
		Polygon
	}
	public class SpawnInfo
	{
		public uint Id;
		public uint MonsterId;
		public uint Region;
		public SpawnLocationType SpawnType;
		public MabiVertex SpawnPoint;
		public double SpawnLineSlope, SpawnLineB;
		public int SpawnLineXStart, SpawnLineXEnd;
		public Region SpawnPolyRegion;
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
				x = (uint)rand.Next(this.SpawnLineXStart, this.SpawnLineXEnd);
				y = (uint)(this.SpawnLineSlope * x + this.SpawnLineB);
			}
			else
			{
				do
				{
					x = (uint)rand.Next((int)this.SpawnPolyBounds.X, (int)(this.SpawnPolyBounds.X + this.SpawnPolyBounds.Width));
					y = (uint)rand.Next((int)this.SpawnPolyBounds.Y, (int)(this.SpawnPolyBounds.Y + this.SpawnPolyBounds.Height));
				} while (!SpawnPolyRegion.IsVisible(x, y));
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
			this.ReadCsv(filePath, 7);
		}

		protected override void CsvToEntry(SpawnInfo info, List<string> csv, int currentLine)
		{
			if ((csv.Count - 3) % 2 != 0)
			{
				throw new InvalidDataException(); //TODO: better?
			}
			var i = 0;
			info.Id = _spawnId++;
			info.MonsterId = Convert.ToUInt32(csv[i++]);
			info.Region = Convert.ToUInt32(csv[i++]);
			info.Amount = Convert.ToByte(csv[i++]);

			List<Point> points = new List<Point>();

			switch (csv.Count - i)
			{
				case 2: //Point
					info.SpawnType = SpawnLocationType.Point;
					info.SpawnPoint = new MabiVertex(Convert.ToUInt32(csv[i++]), Convert.ToUInt32(csv[i++]));
					break;

				case 4: //Line
					info.SpawnType = SpawnLocationType.Line;
					double x1 = Convert.ToDouble(csv[i++]), y1 = Convert.ToDouble(csv[i++]), x2 = Convert.ToDouble(csv[i++]), y2 = Convert.ToDouble(csv[i++]);
					info.SpawnLineSlope = ((y2 - y1) / (x2 - x1)); // m = rise/run
					info.SpawnLineB = y2 - (info.SpawnLineSlope * x1); // y = mx + b
					info.SpawnLineXStart = (int)x1;
					info.SpawnLineXEnd = (int)x2;
					break;

				default: //Polygon
					info.SpawnType = SpawnLocationType.Polygon;
					while (i < csv.Count)
					{
						points.Add(new Point(Convert.ToInt32(csv[i++]), Convert.ToInt32(csv[i++])));
					}
					using (GraphicsPath p = new GraphicsPath())
					{
						p.AddLines(points.ToArray());
						p.CloseFigure();
						info.SpawnPolyRegion = new Region(p);
						info.SpawnPolyBounds = p.GetBounds();
					}
					break;
			}
		}
	}
}
