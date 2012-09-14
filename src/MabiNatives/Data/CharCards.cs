// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Globalization;
using Newtonsoft.Json;

namespace MabiNatives
{
	public class PCCardInfo
	{
		public uint Id;
		public string Name;
		public uint SetId;
		public bool RebirthOnly;
		public List<uint> Races = new List<uint>();

		[JsonIgnore]
		public Dictionary<uint, List<PCCardItem>> Set;

		public PCCardInfo(uint id, uint set, bool rebirthOnly)
		{
			this.Id = id;
			this.SetId = set;
			this.RebirthOnly = rebirthOnly;
		}
	}

	public class PCCardItem
	{
		public uint Id;
		public byte Pocket;
		public uint Race;
		public uint Color1, Color2, Color3;

		public PCCardItem()
		{
		}

		public PCCardItem(uint id, byte pocket, uint race, uint color1 = 0x808080, uint color2 = 0x808080, uint color3 = 0x808080)
		{
			this.Id = id;
			this.Pocket = pocket;
			this.Race = race;
			this.Color1 = color1;
			this.Color2 = color2;
			this.Color3 = color3;
		}
	}

	public class PCCardDb : DataManager<PCCardInfo>
	{
		// XXX: The sets might fit better into a separate db.
		Dictionary<uint, Dictionary<uint, List<PCCardItem>>> Sets = new Dictionary<uint, Dictionary<uint, List<PCCardItem>>>();

		/// <summary>
		/// Searches for the entry with the given Id returns it.
		/// Returns null if it can't be found.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public PCCardInfo Find(uint id)
		{
			return this.Entries.FirstOrDefault(a => a.Id == id);
		}

		public List<PCCardItem> FindSet(uint cardId, uint race)
		{
			var card = this.Find(cardId);
			return this.FindSet(card, race);
		}

		public List<PCCardItem> FindSet(PCCardInfo card, uint race)
		{
			if (card == null || !this.Sets[card.SetId].ContainsKey(race))
				return new List<PCCardItem>();

			return this.Sets[card.SetId][race];
		}

		public override void LoadFromXml(string filePath)
		{
			base.LoadFromXml(filePath);

			// Names
			var namesPath = Path.Combine(Path.GetDirectoryName(filePath), "../local/xml/pccarddescription.english.txt");
			var names = new Dictionary<uint, string>();
			if (File.Exists(namesPath))
			{
				using (var reader = new StreamReader(namesPath))
				{
					string line;
					while ((line = reader.ReadLine()) != null)
					{
						int pos = line.IndexOf('\t');
						if (pos < 0)
							continue;

						line = line.Replace("&lt;", "");
						line = line.Replace("&gt;", "");
						line = line.Replace("<", "");
						line = line.Replace(">", "");

						names.Add(uint.Parse(line.Substring(0, pos)), line.Substring(pos + 1).Trim());
					}
				}
			}

			// Settings
			using (var reader = new XmlTextReader(filePath))
			{
				while (reader.ReadToFollowing("Setting"))
				{
					var setId = uint.Parse(reader.GetAttribute("id"));
					this.Sets.Add(setId, new Dictionary<uint, List<PCCardItem>>());

					using (var reader2 = reader.ReadSubtree())
					{
						var items = new List<PCCardItem>();

						while (reader2.ReadToFollowing("Race"))
						{
							var raceid = uint.Parse(reader2.GetAttribute("raceid"));
							this.Sets[setId].Add(raceid, new List<PCCardItem>());

							using (var reader3 = reader2.ReadSubtree())
							{
								while (reader3.ReadToFollowing("Item"))
								{
									var cardItem = new PCCardItem();
									cardItem.Id = uint.Parse(reader3.GetAttribute("id"));
									cardItem.Pocket = byte.Parse(reader3.GetAttribute("pocket"));
									cardItem.Race = raceid;

									string color = "";
									cardItem.Color1 = ((color = reader3.GetAttribute("col1")) != null ? uint.Parse(color, NumberStyles.HexNumber) : 0);
									cardItem.Color2 = ((color = reader3.GetAttribute("col2")) != null ? uint.Parse(color, NumberStyles.HexNumber) : 0);
									cardItem.Color3 = ((color = reader3.GetAttribute("col3")) != null ? uint.Parse(color, NumberStyles.HexNumber) : 0);

									items.Add(cardItem);
									this.Sets[setId][raceid].Add(cardItem);
								}
							}
						}
					}
				}
			}

			// Cards
			using (var reader = new XmlTextReader(filePath))
			{
				while (reader.Read())
				{
					if (reader.NodeType != XmlNodeType.Element)
						continue;

					if (reader.Name != "Card")
						continue;

					var cardId = uint.Parse(reader.GetAttribute("id"));
					var setting = uint.Parse(reader.GetAttribute("settingid"));
					var rebirthOnly = bool.Parse(reader.GetAttribute("rebirth_only"));
					var races = reader.GetAttribute("race");

					var cardInfo = new PCCardInfo(cardId, setting, rebirthOnly);

					if (races != null)
					{
						var splitted = races.Split('|');
						foreach (string race in splitted)
						{
							cardInfo.Races.Add(uint.Parse(race));
						}
					}
					else
					{
						// Cards that don't have a race specified seem to be for all
						cardInfo.Races.Add(10001);
						cardInfo.Races.Add(10002);
						cardInfo.Races.Add(9001);
						cardInfo.Races.Add(9002);
						cardInfo.Races.Add(8001);
						cardInfo.Races.Add(8002);
					}

					cardInfo.Name = reader.GetAttribute("localname");

					if (names.Count > 0 && cardInfo.Name.EndsWith("]"))
					{
						uint id = uint.Parse(cardInfo.Name.Substring(cardInfo.Name.LastIndexOf('.') + 1, cardInfo.Name.Length - 27));
						if (names.ContainsKey(id))
						{
							cardInfo.Name = names[id];
						}
						else
						{
							cardInfo.Name = "Unknown";
						}
					}

					this.Entries.Add(cardInfo);
				}
			}
		}

		public override void LoadFromJson(string path)
		{
			this.LoadFromJson(path, Path.Combine(Path.GetDirectoryName(path), "charcardsets.txt"));
		}

		public void LoadFromJson(string cardsPath, string setsPath)
		{
			base.LoadFromJson(cardsPath);

			if (!File.Exists(setsPath))
				throw new Exception("File not found: " + setsPath);

			var content = File.ReadAllText(setsPath);
			content = this.ParseJson(content);
			this.Sets = JsonConvert.DeserializeObject<Dictionary<uint, Dictionary<uint, List<PCCardItem>>>>(content);
		}

		public override void ExportToJson(string path)
		{
			this.ExportToJson(path, Path.Combine(Path.GetDirectoryName(path), "charcardsets.txt"));
		}

		public void ExportToJson(string cardsPath, string setsPath)
		{
			base.ExportToJson(cardsPath);

			// Seperate export for sets
			File.Delete(setsPath);

			var setsJson = JsonConvert.SerializeObject(this.Sets, Newtonsoft.Json.Formatting.Indented);
			setsJson = setsJson.Replace("\r\n        \"", " \"");
			setsJson = setsJson.Replace("\r\n      }", " }");
			setsJson = setsJson.Replace("\r\n    \"10001\":", "\r\n    // Female Human\r\n    \"10001\":");
			setsJson = setsJson.Replace("\r\n    \"10002\":", "\r\n    // Male Human\r\n    \"10002\":");
			setsJson = setsJson.Replace("\r\n    \"9001\":", "\r\n    // Female Elf\r\n    \"9001\":");
			setsJson = setsJson.Replace("\r\n    \"9002\":", "\r\n    // Male Elf\r\n    \"9002\":");
			setsJson = setsJson.Replace("\r\n    \"8001\":", "\r\n    // Female Giant\r\n    \"8001\":");
			setsJson = setsJson.Replace("\r\n    \"8002\":", "\r\n    // Male Giant\r\n    \"8002\":");
			setsJson = setsJson.Replace("\r\n    \"730201\":", "\r\n    // Female Partner\r\n    \"730201\":");
			setsJson = setsJson.Replace("\r\n    \"730202\":", "\r\n    // Male Partner\r\n    \"730202\":");
			setsJson = setsJson.Replace("\r\n    \"730203\":", "\r\n    // Partner Anohana ?\r\n    \"730203\":");
			setsJson = setsJson.Replace("\r\n    \"730204\":", "\r\n    // Partner Femalekor ?\r\n    \"730204\":");
			setsJson = setsJson.Replace("\r\n    \"730205\":", "\r\n    // Partner Malekor ?\r\n    \"730205\":");
			setsJson = setsJson.Replace("\r\n  ", "\r\n");

			using (var writer = new StreamWriter(setsPath))
			{
				writer.Write(this.GetHeader());
				writer.Write(setsJson);
			}
		}
	}
}
