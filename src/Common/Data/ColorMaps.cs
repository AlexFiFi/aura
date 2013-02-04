// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Common.Tools;

namespace Common.Data
{
	public class ColorMapInfo
	{
		public byte Id;
		public ushort Width, Height;
		public uint[] ColorMap;
	}

	/// <summary>
	/// Holds information about all colors a specific material can have.
	/// </summary>
	public class ColorMapDb : DataManager<ColorMapInfo>
	{
		/// <summary>
		/// Searches for the entry with the given Id and returns it.
		/// Returns null if it can't be found.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ColorMapInfo Find(byte id)
		{
			return this.Entries.FirstOrDefault(a => a.Id == id);
		}

		/// <summary>
		/// Returns a random color from the map with the given Id,
		/// or 0, if the map couldn't be found.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public uint GetRandom(byte id)
		{
			return this.GetRandom(id, RandomProvider.Get());
		}

		public uint GetRandom(byte id, Random rand)
		{
			var mapInfo = this.Find(id);
			if (mapInfo == null)
				return 0;

			return this.GetAt(mapInfo, rand.Next(mapInfo.ColorMap.Length));
		}

		public uint GetAt(byte id, int x, int y)
		{
			var mapInfo = this.Find(id);
			if (mapInfo == null)
				return 0;

			return this.GetAt(mapInfo, y * mapInfo.Height + x);
		}

		public uint GetAt(ColorMapInfo mapInfo, int idx)
		{
			var color = mapInfo.ColorMap[idx];
			if (color >> 24 == 0)
				color = ((color & 0xFF) << 16) + ((color >> 8 & 0xFF) << 8) + (color >> 16 & 0xFF);
			return color;
		}

		public void LoadFromDat(string filePath, bool reload = false)
		{
			if (reload)
				this.Entries.Clear();

			var data = File.ReadAllBytes(filePath);

			using (var min = new MemoryStream(data))
			using (var mout = new MemoryStream())
			{
				using (var gzip = new GZipStream(min, CompressionMode.Decompress))
				{
					gzip.CopyTo(mout);
				}

				using (var br = new BinaryReader(mout))
				{
					br.BaseStream.Position = 0;
					while (br.BaseStream.Position < br.BaseStream.Length)
					{
						var info = new ColorMapInfo();
						info.Id = br.ReadByte();
						info.Width = br.ReadUInt16();
						info.Height = br.ReadUInt16();
						info.ColorMap = new uint[info.Width * info.Height];
						for (int i = 0; i < info.ColorMap.Length; ++i)
						{
							info.ColorMap[i] = br.ReadUInt32();
						}
						this.Entries.Add(info);
					}
				}
			}
		}
	}
}
