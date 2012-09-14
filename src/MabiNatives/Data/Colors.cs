// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MabiNatives
{
	public class ColorInfo
	{
		public ushort Num;
		public uint ARGB;
		public string Name;

		public ColorInfo() { }

		public ColorInfo(byte num, uint argb, string name)
		{
			this.Num = num;
			this.ARGB = argb;
			this.Name = name;
		}
	}

	public class ColorDb : DataManager<ColorInfo>
	{
		/// <summary>
		/// Searches for the entry with the given number and returns it.
		/// Returns null if it can't be found.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public ColorInfo Find(ulong num)
		{
			return this.Entries.FirstOrDefault(a => a.Num == num);
		}

		/// <summary>
		/// Searches for the entry with the given name and returns it.
		/// Returns null if it can't be found.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ColorInfo Find(string name)
		{
			return this.Entries.FirstOrDefault(a => a.Name == name);
		}

		public override void LoadFromXml(string filePath)
		{
			base.LoadFromXml(filePath);

			using (var itemInfoReader = new XmlTextReader(filePath))
			{
				while (itemInfoReader.Read())
				{
					if (itemInfoReader.NodeType != XmlNodeType.Element)
						continue;
					if (itemInfoReader.Name != "MabiSysPalette")
						continue;

					var color = new ColorInfo();
					color.Num = ushort.Parse(itemInfoReader.GetAttribute("number"));
					color.ARGB = uint.Parse(itemInfoReader.GetAttribute("RGB"), System.Globalization.NumberStyles.HexNumber);
					color.Name = itemInfoReader.GetAttribute("nameEng");

					this.Entries.Add(color);
				}
			}
		}

		protected override string FormatJson(string line)
		{
			line = base.FormatJson(line);
			line = Regex.Replace(line, "\"ARGB\": (?<color>[0-9]+),", delegate(Match match)
			{
				return "\"ARGB\": 0x" + Convert.ToUInt32(match.Groups["color"].Value).ToString("X") + ","; ;
			});

			return line;
		}
	}
}
