// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Text;
using System.IO.Compression;

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
			var mapInfo = this.Find(id);
			if (mapInfo == null)
				return 0;

			var rand = new Random(Environment.TickCount);
			return mapInfo.ColorMap[rand.Next(mapInfo.ColorMap.Length)];
		}

		public void LoadFromDat(string filePath, bool reload=false)
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
