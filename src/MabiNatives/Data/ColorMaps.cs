// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;
using System.Text;
using System.IO.Compression;

namespace MabiNatives
{
	public class ColorMapInfo
	{
		public byte Id;
		public ushort Width, Height;
		public uint[] ColorMap;
	}

	/// <summary>
	/// Holds information about all colors a specific material can have.
	/// This is usually stored in several files, linked to from an
	/// XML. Because the resulting JSON is pretty huge, and it's unlikely
	/// people will directly edit this data, it's compressed using GZIP.
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

		public override void LoadFromXml(string filePath)
		{
			base.LoadFromXml(filePath);

			using (var reader = new XmlTextReader(filePath))
			{
				while (reader.Read())
				{
					if (reader.NodeType != XmlNodeType.Element || reader.Name != "ColorTable" || !reader.HasAttributes)
						continue;

					var colorMap = new ColorMapInfo();
					colorMap.Id = byte.Parse(reader.GetAttribute("ColorMapID"));
					colorMap.Width = ushort.Parse(reader.GetAttribute("Width"));
					colorMap.Height = ushort.Parse(reader.GetAttribute("Height"));
					var bmpPath = Path.Combine(Path.GetDirectoryName(filePath), "../..", reader.GetAttribute("Bitmap"));

					if (File.Exists(bmpPath))
					{
						colorMap.ColorMap = new uint[colorMap.Width * colorMap.Height];

						using (var br = new BinaryReader(File.Open(bmpPath, FileMode.Open, FileAccess.Read, FileShare.Read)))
						{
							for (int i = 0, j = 0; i < br.BaseStream.Length; i += 4, ++j)
							{
								colorMap.ColorMap[j] += (uint)br.ReadByte() << 16;
								colorMap.ColorMap[j] += (uint)br.ReadByte() << 8;
								colorMap.ColorMap[j] += (uint)br.ReadByte();
								br.ReadByte();
							}
						}

						this.Entries.Add(colorMap);
					}
				}
			}
		}

		//public override void LoadFromJson(string path)
		//{
		//    path += ".gz";
		//    if (!File.Exists(path))
		//        throw new FileNotFoundException("File not found: " + path);

		//    var data = Convert.FromBase64String(File.ReadAllText(path));

		//    using (var min = new MemoryStream(data))
		//    using (var mout = new MemoryStream())
		//    {
		//        using (var gzip = new GZipStream(min, CompressionMode.Decompress))
		//        {
		//            gzip.CopyTo(mout);
		//        }

		//        File.WriteAllText("test.txt", Encoding.UTF8.GetString(mout.ToArray()));

		//        this.Entries = JsonConvert.DeserializeObject<List<ColorMapInfo>>(Encoding.UTF8.GetString(mout.ToArray()));
		//    }
		//}

		//public override void ExportToJson(string path)
		//{
		//    path += ".gz";
		//    File.Delete(path);

		//    var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this.Entries));

		//    using (var min = new MemoryStream(data))
		//    using (var mout = new MemoryStream())
		//    {
		//        using (var gzip = new GZipStream(mout, CompressionMode.Compress))
		//        {
		//            min.CopyTo(gzip);
		//        }
		//        File.WriteAllText(path, Convert.ToBase64String(mout.ToArray()));
		//    }
		//}
	}
}
