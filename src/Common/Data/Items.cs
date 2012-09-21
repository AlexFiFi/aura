// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace Common.Data
{
	public class ItemDbInfo
	{
		public uint Id;
		public string Name, KorName;
		public ushort Type;
		public byte Width, Height;
		public byte ColorMap1, ColorMap2, ColorMap3, ColorMode;
		public ushort StackType;
		public ushort StackMax;
		public uint StackItem;
		public uint Price;
		public uint Durability;
		public uint Defense;
		public short Protection;
		public byte WeaponType;
		public ushort Range;
		public ushort AttackMin, AttackMax;
		public byte Critical;
		public byte Balance;
		public byte AttackSpeed;
		public byte KnockCount;
		public short UsableType, UsableVar, UsablePerc, UsableStr, UsableInt, UsableDex, UsableWill, UsableLuck, UsableLife, UsableMana, UsableStamina, UsableFat, UsableUpper, UsableLower;
		public float UsableToxic;
	}

	public class ItemDb : DataManager<ItemDbInfo>
	{
		public ItemDbInfo Find(uint id)
		{
			return this.Entries.FirstOrDefault(a => a.Id == id);
		}

		public ItemDbInfo Find(string name)
		{
			name = name.ToLower();
			return this.Entries.FirstOrDefault(a => a.Name.ToLower() == name);
		}

		public List<ItemDbInfo> FindAll(string name)
		{
			name = name.ToLower();
			return this.Entries.FindAll(a => a.Name.ToLower().Contains(name));
		}

		public override void LoadFromCsv(string filePath, bool reload = false)
		{
			base.LoadFromCsv(filePath, reload);
			this.ReadCsv(filePath, 25);
		}

		protected override void CsvToEntry(ItemDbInfo info, List<string> csv, int currentLine)
		{
			var i = 0;
			info.Id = Convert.ToUInt32(csv[i++]);
			info.Name = csv[i++];
			info.KorName = csv[i++];
			info.Type = Convert.ToUInt16(csv[i++]);
			info.StackType = Convert.ToUInt16(csv[i++]);
			info.StackMax = Convert.ToUInt16(csv[i++]);
			info.StackItem = Convert.ToUInt32(csv[i++]);
			info.Width = Convert.ToByte(csv[i++]);
			info.Height = Convert.ToByte(csv[i++]);
			info.ColorMap1 = Convert.ToByte(csv[i++]);
			info.ColorMap2 = Convert.ToByte(csv[i++]);
			info.ColorMap3 = Convert.ToByte(csv[i++]);
			info.ColorMode = Convert.ToByte(csv[i++]);
			info.Price = Convert.ToUInt32(csv[i++]);
			info.Durability = Convert.ToUInt32(csv[i++]);
			info.Defense = Convert.ToUInt32(csv[i++]);
			info.Protection = Convert.ToInt16(csv[i++]);
			info.WeaponType = Convert.ToByte(csv[i++]);
			if (info.WeaponType == 0)
			{
				i += 7;
			}
			else
			{
				info.Range = Convert.ToUInt16(csv[i++]);
				info.AttackMin = Convert.ToUInt16(csv[i++]);
				info.AttackMax = Convert.ToUInt16(csv[i++]);
				info.Critical = Convert.ToByte(csv[i++]);
				info.Balance = Convert.ToByte(csv[i++]);
				info.AttackSpeed = Convert.ToByte(csv[i++]);
				info.KnockCount = Convert.ToByte(csv[i++]);
			}
			if (info.Type < 400 || info.Type > 503)
			{
				i += 15;
			}
			else
			{
				info.UsableType = Convert.ToInt16(csv[i++]);
				info.UsableVar = Convert.ToInt16(csv[i++]);
				info.UsablePerc = Convert.ToInt16(csv[i++]);
				info.UsableStr = Convert.ToInt16(csv[i++]);
				info.UsableInt = Convert.ToInt16(csv[i++]);
				info.UsableDex = Convert.ToInt16(csv[i++]);
				info.UsableWill = Convert.ToInt16(csv[i++]);
				info.UsableLuck = Convert.ToInt16(csv[i++]);
				info.UsableLife = Convert.ToInt16(csv[i++]);
				info.UsableMana = Convert.ToInt16(csv[i++]);
				info.UsableStamina = Convert.ToInt16(csv[i++]);
				info.UsableFat = Convert.ToInt16(csv[i++]);
				info.UsableUpper = Convert.ToInt16(csv[i++]);
				info.UsableLower = Convert.ToInt16(csv[i++]);
				info.UsableToxic = float.Parse(csv[i++], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
			}
		}
	}
}
