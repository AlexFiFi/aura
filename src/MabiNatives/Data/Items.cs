// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json;

namespace MabiNatives
{
	public class ItemDbInfo
	{
		public uint Id;
		public string Name, KorName;

		public ushort Type;

		public ushort BundleType;
		public ushort BundleMax;
		public uint StackItem;

		public byte Width, Height;
		public uint Price;

		public uint Durability;
		public uint Defense;
		public short Protection;
		public ushort AttackMin, AttackMax;
		public byte Critical;
		public byte Balance;

		public byte WeaponType;
		public ushort EffectiveRange;
		public byte AttackSpeed;
		public byte DownHitCount;

		public byte ColorMap1, ColorMap2, ColorMap3;
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

		public override void LoadFromXml(string filePath)
		{
			if (!File.Exists(filePath))
				throw new FileNotFoundException("File not found: " + filePath);

			// Read official item names for later use in GM commands, scripts, etc.
			var itemNames = new Dictionary<uint, string>();
			var namesPath = Path.Combine(Path.GetDirectoryName(filePath), "../local/xml/itemdb.english.txt");

			if (File.Exists(namesPath))
			{
				using (var sr = new StreamReader(namesPath))
				{
					string line;
					while ((line = sr.ReadLine()) != null)
					{
						int pos = line.IndexOf('\t');
						if (pos < 0)
							continue;

						itemNames.Add(uint.Parse(line.Substring(0, pos)), line.Substring(pos + 1).Replace("@", ""));
					}
				}
			}

			// Read actual item db
			using (var itemInfoReader = new XmlTextReader(filePath))
			{
				while (itemInfoReader.Read())
				{
					if (itemInfoReader.NodeType == XmlNodeType.Element)
					{
						if (itemInfoReader.Name == "Mabi_Item")
						{
							var itemInfo = new ItemDbInfo();
							itemInfo.Id = Convert.ToUInt32(itemInfoReader.GetAttribute("ID"));
							itemInfo.BundleMax = Convert.ToUInt16(itemInfoReader.GetAttribute("Bundle_Max"));
							itemInfo.BundleType = Convert.ToUInt16(itemInfoReader.GetAttribute("Bundle_Type"));
							itemInfo.Price = Convert.ToUInt32(itemInfoReader.GetAttribute("Price_Buy"));
							itemInfo.Width = Convert.ToByte(itemInfoReader.GetAttribute("Inv_XSize"));
							itemInfo.Height = Convert.ToByte(itemInfoReader.GetAttribute("Inv_YSize"));
							itemInfo.Durability = Convert.ToUInt32(itemInfoReader.GetAttribute("Par_DurabilityMax"));
							itemInfo.Defense = Convert.ToUInt32(itemInfoReader.GetAttribute("Par_Defense"));
							itemInfo.Protection = Convert.ToInt16(itemInfoReader.GetAttribute("Par_ProtectRate"));
							itemInfo.AttackMin = Convert.ToUInt16(itemInfoReader.GetAttribute("Par_AttackMin"));
							itemInfo.AttackMax = Convert.ToUInt16(itemInfoReader.GetAttribute("Par_AttackMax"));
							itemInfo.Critical = Convert.ToByte(itemInfoReader.GetAttribute("Par_CriticalRate"));
							itemInfo.Balance = Convert.ToByte(itemInfoReader.GetAttribute("Par_AttackBalance"));
							itemInfo.EffectiveRange = Convert.ToUInt16(itemInfoReader.GetAttribute("Par_EffectiveRange"));
							itemInfo.AttackSpeed = Convert.ToByte(itemInfoReader.GetAttribute("Par_AttackSpeed"));
							itemInfo.DownHitCount = Convert.ToByte(itemInfoReader.GetAttribute("Par_DownHitCount"));
							itemInfo.Type = Convert.ToUInt16(itemInfoReader.GetAttribute("Attr_Type"));

							// We'll turn Gold and Gold bags into Sac items, that's easier.
							// XXX: This is easier for Aura, but for a general lib it might be wrong to do it. Still makes more sense =|
							if (itemInfo.Type == 5)
							{
								if (itemInfo.BundleType == 2)
								{
									itemInfo.StackItem = 2000; // Gold
									itemInfo.Type = 1000; // Sac
								}
								else if (itemInfo.BundleType == 1)
								{
									itemInfo.Type = 1000;
								}
							}
							else if (itemInfo.Type == 1000)
							{
								if (itemInfo.BundleType == 2)
								{
									var xml = itemInfoReader.GetAttribute("XML");
									if (xml != null)
									{
										var match = Regex.Match(xml, "stack_item=\"([0-9]+)\"", RegexOptions.IgnoreCase);
										if (match.Success)
										{
											itemInfo.StackItem = uint.Parse(match.Groups[1].Value);
										}
									}
								}
							}

							itemInfo.ColorMap1 = Convert.ToByte(itemInfoReader.GetAttribute("App_Color1"));
							itemInfo.ColorMap2 = Convert.ToByte(itemInfoReader.GetAttribute("App_Color2"));
							itemInfo.ColorMap3 = Convert.ToByte(itemInfoReader.GetAttribute("App_Color3"));

							itemInfo.WeaponType = Convert.ToByte(itemInfoReader.GetAttribute("App_WeaponActionType"));

							itemInfo.KorName = itemInfoReader.GetAttribute("Text_Name0");
							itemInfo.Name = itemInfoReader.GetAttribute("Text_Name1");

							if (itemNames.Count > 0 && itemInfo.Name.EndsWith("]"))
							{
								uint id = uint.Parse(itemInfo.Name.Substring(itemInfo.Name.LastIndexOf('.') + 1, itemInfo.Name.Length - 16));
								if (itemNames.ContainsKey(id))
								{
									itemInfo.Name = itemNames[id];
								}
								else
								{
									itemInfo.Name = "Unknown";
								}
							}

							this.Entries.Add(itemInfo);
						}
					}
				}
			}
		}
	}
}
