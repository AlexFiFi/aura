// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Globalization;

namespace MabiNatives
{
	public class MotionInfo
	{
		public string Name;
		public ushort Category;
		public ushort Type;
	}

	public class MotionDb : DataManager<MotionInfo>
	{
		/// <summary>
		/// Searches for the entry with the given name returns it.
		/// Returns null if it can't be found.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public MotionInfo Find(string name)
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

					if (itemInfoReader.Name != "Motion")
						continue;

					var motion = new MotionInfo();
					motion.Name = itemInfoReader.GetAttribute("name");
					string[] motiondatas = itemInfoReader.GetAttribute("main_motion").Split(',');
					motion.Category = ushort.Parse(motiondatas[0]);
					motion.Type = ushort.Parse(motiondatas[1]);

					this.Entries.Add(motion);
				}
			}
		}
	}
}
