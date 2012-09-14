// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace MabiNatives
{
	public class AgeInfo
	{
		public byte Age;
		public byte Race;
		public Dictionary<string, byte> BaseStats = new Dictionary<string, byte>();
	}

	public class AgeDb : DataManager<AgeInfo>
	{
		/// <summary>
		/// Returns the age info (base stats) for the given race
		/// at the given age, or null.
		/// </summary>
		/// <param name="race">0 = Human, 1 = Elf, 2 = Giant</param>
		/// <param name="age">10-17</param>
		/// <returns></returns>
		public AgeInfo Find(byte race, byte age)
		{
			return this.Entries.FirstOrDefault(a => a.Race == race && a.Age == age);
		}

		public override void LoadFromXml(string filePath)
		{
			base.LoadFromXml(filePath);

			byte race = 0;
			foreach (var nodeName in new string[] { "BaseValueList", "ElfBaseValueList", "GiantBaseValueList" })
			{
				using (var reader1 = new XmlTextReader(filePath))
				{
					while (reader1.ReadToFollowing(nodeName))
					{
						using (var reader2 = reader1.ReadSubtree())
						{
							while (reader2.ReadToFollowing("Data"))
							{
								var ageInfo = new AgeInfo();
								ageInfo.Age = byte.Parse(reader2.GetAttribute("Age"));
								ageInfo.Race = race;
								while (reader2.MoveToNextAttribute())
								{
									ageInfo.BaseStats.Add(reader2.Name.ToLower(), byte.Parse(reader2.Value));
								}

								this.Entries.Add(ageInfo);
							}
						}
					}
				}
				race++;
			}
		}
	}
}
