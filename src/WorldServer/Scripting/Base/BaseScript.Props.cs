// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Data;
using Aura.Shared.Util;
using Aura.World.Events;
using Aura.World.Util;
using Aura.World.World;

namespace Aura.World.Scripting
{
	public partial class BaseScript : IDisposable
	{
		/// <summary>
		/// Spawns prop with the specified behavior, using the region name.
		/// </summary>
		protected MabiProp SpawnProp(uint propClass, string region, uint x, uint y, float direction, float scale, MabiPropFunc behavior)
		{
			uint regionId = MabiData.RegionDb.TryGetRegionId(region);
			return this.SpawnProp(propClass, regionId, x, y, scale, direction, behavior);
		}

		/// <summary>
		/// Spawns prop with the specified behavior, using the region id.
		/// </summary>
		protected MabiProp SpawnProp(uint propClass, uint region, uint x, uint y, float direction, float scale, MabiPropFunc behavior)
		{
			var prop = this.SpawnProp(propClass, region, x, y, direction, scale);
			this.DefineProp(prop, behavior);

			return prop;
		}

		/// <summary>
		/// Simple prop spawning without behavior, using region name.
		/// Without "area" props are not hit or touchable.
		/// </summary>
		/// <returns>New prop</returns>
		protected MabiProp SpawnProp(uint propClass, string region, uint x, uint y, float direction = 0, float scale = 1)
		{
			uint regionId = MabiData.RegionDb.TryGetRegionId(region);
			return this.SpawnProp(propClass, regionId, x, y, direction, scale);
		}

		/// <summary>
		/// Simple prop spawning without behavior.
		/// Without "area" props are not hit or touchable.
		/// </summary>
		/// <returns>New prop</returns>
		protected MabiProp SpawnProp(uint propClass, uint region, uint x, uint y, float direction = 0, float scale = 1)
		{
			var prop = new MabiProp(propClass, region, x, y, direction, scale);

			WorldManager.Instance.AddProp(prop);

			return prop;
		}

		protected MabiProp SpawnProp(ulong id, string name, string title, string extra, uint propClass, uint region, uint x, uint y, float direction, float scale, MabiPropFunc behavior)
		{
			var prop = new MabiProp(id, name, title, extra, propClass, region, x, y, direction, scale);

			WorldManager.Instance.AddProp(prop);
			if (behavior != null)
				this.DefineProp(prop, behavior);

			return prop;
		}

		protected MabiProp SpawnProp(MabiProp prop, MabiPropFunc behavior = null)
		{
			WorldManager.Instance.AddProp(prop);
			if (behavior != null)
				this.DefineProp(prop, behavior);

			return prop;
		}

		protected void DefineProp(ulong propId, string region, uint x, uint y, MabiPropFunc behavior = null)
		{
			uint regionId = MabiData.RegionDb.TryGetRegionId(region);
			this.DefineProp(propId, regionId, x, y, behavior);
		}

		/// <summary>
		/// Adds a behavior for the prop with the given id. Since this is for
		/// client side props we also need a location that can be checked later on.
		/// </summary>
		protected void DefineProp(ulong propId, uint region, uint x, uint y, MabiPropFunc behavior = null)
		{
			this.DefineProp(new MabiProp(propId, region, x, y), behavior);
		}

		/// <summary>
		/// Adds the given prop and behavior to the behavior list.
		/// </summary>
		protected void DefineProp(MabiProp prop, MabiPropFunc behavior = null)
		{
			if (behavior != null)
				WorldManager.Instance.SetPropBehavior(new MabiPropBehavior(prop, behavior));
		}

		// Behaviours
		// ------------------------------------------------------------------

		protected MabiPropFunc PropWarp(uint region, uint x, uint y)
		{
			return (client, creature, prop) => { client.Warp(region, x, y); };
		}

		protected MabiPropFunc PropWarp(string region, uint x, uint y)
		{
			uint regionId = MabiData.RegionDb.TryGetRegionId(region);
			return this.PropWarp(regionId, x, y);
		}

		protected MabiPropFunc PropDrop(uint dropType)
		{
			return (client, creature, prop) =>
			{
				if (Rnd() > WorldConf.PropDropRate)
					return;

				var di = MabiData.PropDropDb.Find(dropType);
				if (di == null)
				{
					Logger.Warning("Unknown prop drop type '{0}'.", dropType);
					return;
				}

				var dii = di.GetRndItem(RandomProvider.Get());
				var item = new MabiItem(dii.ItemClass);
				item.Info.Amount = dii.Amount > 1 ? (ushort)this.Rnd(1, dii.Amount) : (ushort)1;
				WorldManager.Instance.DropItem(item, prop.Region, (uint)prop.Info.X, (uint)prop.Info.Y);
			};
		}
	}
}
