// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace MabiNatives
{
	public class PortalInfo
	{
		public ulong Id;
		public uint Region, X, Y;
		public uint DestinationRegion, DestinationX, DestinationY;
	}

	/// <summary>
	/// Contains information about portals, where they are, and where they go.
	/// </summary>
	public class PortalDb : DataManager<PortalInfo>
	{
		/// <summary>
		/// Returns the portal with the given Id or null.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public PortalInfo Find(ulong id)
		{
			return this.Entries.FirstOrDefault(a => a.Id == id);
		}

		public override void LoadFromXml(string path)
		{
			if (!File.Exists(path))
				throw new FileNotFoundException("File not found: " + path);

			var itemInfoReader = new XmlTextReader(path);

			uint portalRegion = 0;

			while (itemInfoReader.Read())
			{
				if (itemInfoReader.NodeType == XmlNodeType.Element)
				{
					if (itemInfoReader.Name == "region")
					{
						portalRegion = uint.Parse(itemInfoReader.GetAttribute("id"));
					}
					else if (itemInfoReader.Name == "portal")
					{
						var portal = new PortalInfo();
						portal.Id = ulong.Parse(itemInfoReader.GetAttribute("enterid"));
						portal.Region = portalRegion;
						portal.X = (uint)float.Parse(itemInfoReader.GetAttribute("enter_x"), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
						portal.Y = (uint)float.Parse(itemInfoReader.GetAttribute("enter_z"), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
						portal.DestinationRegion = uint.Parse(itemInfoReader.GetAttribute("target_regionid"));
						portal.DestinationX = (uint)float.Parse(itemInfoReader.GetAttribute("exit_x"), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));
						portal.DestinationY = (uint)float.Parse(itemInfoReader.GetAttribute("exit_z"), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"));

						this.Entries.Add(portal);
					}
				}
			}
		}
	}
}
