// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Aura.Data
{
	public class RegionInfo
	{
		public uint Id;
		public uint X1, Y1, X2, Y2;
		public Dictionary<uint, AreaInfo> Areas;
	}

	public class AreaInfo
	{
		public uint Id;
		public uint X1, Y1, X2, Y2;
		public Dictionary<ulong, PropInfo> Props;
		public Dictionary<ulong, EventInfo> Events;
	}

	public class PropInfo
	{
		public ulong Id;
		public uint Class;
		public float X, Y;
		public float Direction, Scale;
		public List<PropShapeInfo> Shapes;
	}

	public class PropShapeInfo
	{
		public uint X1, Y1, X2, Y2, X3, Y3, X4, Y4;
	}

	public class EventInfo
	{
		public ulong Id;
		public uint Type;
		public float X, Y;
		public bool IsAltar;
		public List<EventElementInfo> Elements;
	}

	public class EventElementInfo
	{
		public uint Type, Unk;
	}

	public class RegionInfoDb : DatabaseDatIndexed<uint, RegionInfo>
	{
		public Dictionary<ulong, PropInfo> PropEntries = new Dictionary<ulong, PropInfo>();
		public Dictionary<ulong, EventInfo> EventEntries = new Dictionary<ulong, EventInfo>();

		public override void Clear()
		{
			base.Clear();
			this.PropEntries.Clear();
			this.EventEntries.Clear();
		}

		public AreaInfo Find(uint region, uint area)
		{
			if (!this.Entries.ContainsKey(region))
				return null;
			if (!this.Entries[region].Areas.ContainsKey(area))
				return null;

			return this.Entries[region].Areas[area];
		}

		public EventInfo FindEvent(ulong id)
		{
			EventInfo result;
			this.EventEntries.TryGetValue(id, out result);
			return result;
		}

		public PropInfo FindProp(ulong id)
		{
			PropInfo result;
			this.PropEntries.TryGetValue(id, out result);
			return result;
		}

		public Point RandomCoord(uint region)
		{
			var result = new Point();

			var ri = this.Find(region);
			if (ri != null)
			{
				var rnd = new Random(Environment.TickCount);
				result.X = (uint)rnd.Next((int)ri.X1, (int)ri.X2);
				result.Y = (uint)rnd.Next((int)ri.Y1, (int)ri.Y2);
			}

			return result;
		}

		public uint GetAreaId(uint region, uint x, uint y)
		{
			var ri = this.Find(region);
			if (ri == null)
				return uint.MaxValue;

			foreach (var area in ri.Areas.Values)
			{
				if (x >= Math.Min(area.X1, area.X2) && x <= Math.Max(area.X1, area.X2) && y >= Math.Min(area.Y1, area.Y2) && y <= Math.Max(area.Y1, area.Y2))
					return area.Id;
			}

			return 0;
		}

		protected override void Read(BinaryReader br)
		{
			var cRegions = br.ReadInt32();
			for (int l = 0; l < cRegions; ++l)
			{
				var ri = new RegionInfo();

				ri.Id = br.ReadUInt32();
				ri.X1 = br.ReadUInt32();
				ri.Y1 = br.ReadUInt32();
				ri.X2 = br.ReadUInt32();
				ri.Y2 = br.ReadUInt32();

				var cAreas = br.ReadInt32();
				ri.Areas = new Dictionary<uint, AreaInfo>();
				for (int i = 0; i < cAreas; ++i)
				{
					var ai = new AreaInfo();

					ai.Id = br.ReadUInt32();
					ai.X1 = br.ReadUInt32();
					ai.Y1 = br.ReadUInt32();
					ai.X2 = br.ReadUInt32();
					ai.Y2 = br.ReadUInt32();

					var cProps = br.ReadInt32();
					ai.Props = new Dictionary<ulong, PropInfo>();
					for (int j = 0; j < cProps; ++j)
					{
						var pi = new PropInfo();
						pi.Id = br.ReadUInt64();
						pi.Class = br.ReadUInt32();
						pi.X = br.ReadSingle();
						pi.Y = br.ReadSingle();
						pi.Direction = br.ReadSingle();
						pi.Scale = br.ReadSingle();

						var cShapes = br.ReadInt32();
						pi.Shapes = new List<PropShapeInfo>();
						for (int k = 0; k < cShapes; ++k)
						{
							var si = new PropShapeInfo();
							si.X1 = br.ReadUInt32();
							si.Y1 = br.ReadUInt32();
							si.X2 = br.ReadUInt32();
							si.Y2 = br.ReadUInt32();
							si.X3 = br.ReadUInt32();
							si.Y3 = br.ReadUInt32();
							si.X4 = br.ReadUInt32();
							si.Y4 = br.ReadUInt32();

							pi.Shapes.Add(si);
						}

						ai.Props.Add(pi.Id, pi);
						this.PropEntries.Add(pi.Id, pi);
					}

					var cEvents = br.ReadInt32();
					ai.Events = new Dictionary<ulong, EventInfo>();
					for (int j = 0; j < cEvents; ++j)
					{
						var ei = new EventInfo();
						ei.Id = br.ReadUInt64();
						ei.X = br.ReadSingle();
						ei.Y = br.ReadSingle();
						ei.Type = br.ReadUInt32();

						var cElements = br.ReadInt32();
						ei.Elements = new List<EventElementInfo>();
						for (int k = 0; k < cElements; ++k)
						{
							var eei = new EventElementInfo();
							eei.Type = br.ReadUInt32();
							eei.Unk = br.ReadUInt32();

							if (!ei.IsAltar && eei.Type == 2110 && eei.Unk == 103)
								ei.IsAltar = true;

							ei.Elements.Add(eei);
						}

						ai.Events.Add(ei.Id, ei);
						this.EventEntries.Add(ei.Id, ei);
					}

					ri.Areas.Add(ai.Id, ai);
				}

				this.Entries.Add(ri.Id, ri);
			}
		}
	}
}
