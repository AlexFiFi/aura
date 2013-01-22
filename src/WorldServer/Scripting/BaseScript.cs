// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Common.Data;
using Common.Tools;
using World.World;
using Common.World;
using World.Network;
using Common.Network;

namespace World.Scripting
{
	public class BaseScript : IDisposable
	{
		protected Random _rnd = RandomProvider.Get();

		public string ScriptPath { get; set; }
		public string ScriptName { get; set; }

		public bool Disposed { get; protected set; }

		public virtual void OnLoad()
		{
		}

		public virtual void OnLoadDone()
		{
		}

		/// <inheritdoc/>
		/// <summary>
		/// Cleans up after the NPC (In case of reloading)
		/// Every derived class should call base.Dispose()
		/// </summary>
		public virtual void Dispose()
		{
			this.Disposed = true;
		}

		// Built in methods
		// ------------------------------------------------------------------

		/// <summary>
		/// Shortcut for warps, using region names.
		/// </summary>
		protected void SpawnProp(uint propClass, string region, uint x, uint y, uint area, float scale, float direction, PropAction action, string tregion, uint tx, uint ty)
		{
			uint regionId = MabiData.MapDb.TryGetRegionId(region, this.ScriptName);
			uint tregionId = MabiData.MapDb.TryGetRegionId(tregion, this.ScriptName);
			this.SpawnProp(propClass, regionId, x, y, area, scale, direction, action, tregionId, tx, ty);
		}

		/// <summary>
		/// Shortcut for warps, using region ids.
		/// </summary>
		protected void SpawnProp(uint propClass, uint region, uint x, uint y, uint area, float scale, float direction, PropAction action, uint tregion, uint tx, uint ty)
		{
			MabiPropFunc behavior = (client, creature, prop) => { client.Warp(tregion, tx, ty); };
			this.SpawnProp(propClass, region, x, y, area, scale, direction, behavior);
		}

		/// <summary>
		/// Spawns prop with the specified behavior, using the region name.
		/// </summary>
		protected void SpawnProp(uint propClass, string region, uint x, uint y, uint area, float scale, float direction, MabiPropFunc behavior)
		{
			uint regionId = MabiData.MapDb.TryGetRegionId(region, this.ScriptName);
			this.SpawnProp(propClass, regionId, x, y, area, scale, direction, behavior);
		}

		/// <summary>
		/// Spawns prop with the specified behavior, using the region id.
		/// </summary>
		protected void SpawnProp(uint propClass, uint region, uint x, uint y, uint area, float scale, float direction, MabiPropFunc behavior)
		{
			var prop = this.SpawnProp(propClass, region, x, y, area, scale, direction);
			this.DefineProp(prop, behavior);
		}

		/// <summary>
		/// Simple prop spawning without behavior, using region name.
		/// Without "area" props are not hit or touchable.
		/// </summary>
		/// <returns>New prop</returns>
		protected MabiProp SpawnProp(uint propClass, string region, uint x, uint y, uint area = 0, float scale = 1f, float direction = 1f)
		{
			uint regionId = MabiData.MapDb.TryGetRegionId(region, this.ScriptName);
			return this.SpawnProp(propClass, regionId, x, y, area, scale, direction);
		}

		/// <summary>
		/// Simple prop spawning without behavior.
		/// Without "area" props are not hit or touchable.
		/// </summary>
		/// <returns>New prop</returns>
		protected MabiProp SpawnProp(uint propClass, uint region, uint x, uint y, uint area = 0, float scale = 1f, float direction = 1f)
		{
			var prop = new MabiProp(region, area);
			prop.Info.Class = propClass;
			prop.Info.X = x;
			prop.Info.Y = y;
			prop.Info.Scale = scale;
			prop.Info.Direction = direction;

			WorldManager.Instance.AddProp(prop);

			return prop;
		}

		/// <summary>
		/// Shortcut for warps using a region name.
		/// </summary>
		protected void DefineProp(ulong propId, string region, uint x, uint y, PropAction action, string tregion, uint tx, uint ty)
		{
			uint regionId = MabiData.MapDb.TryGetRegionId(region, this.ScriptName);
			uint tregionId = MabiData.MapDb.TryGetRegionId(tregion, this.ScriptName);
			this.DefineProp(propId, regionId, x, y, action, tregionId, tx, ty);
		}

		/// <summary>
		/// Shortcut for warps using a region id.
		/// </summary>
		protected void DefineProp(ulong propId, uint region, uint x, uint y, PropAction action, uint tregion, uint tx, uint ty)
		{
			MabiPropFunc behavior = (client, creature, prop) => { client.Warp(tregion, tx, ty); };
			this.DefineProp(propId, region, x, y, behavior);
		}

		protected void DefineProp(ulong propId, string region, uint x, uint y, MabiPropFunc behavior = null)
		{
			uint regionId = MabiData.MapDb.TryGetRegionId(region, this.ScriptName);
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

		/// <summary>
		/// "Redirect" to WorldManager.Instance.SpawnCreature.
		/// </summary>
		protected void Spawn(uint race, uint amount, uint region, uint x, uint y)
		{
			WorldManager.Instance.SpawnCreature(race, amount, region, x, y);
		}

		/// <summary>
		/// "Redirect" to WorldManager.Instance.SpawnCreature.
		/// </summary>
		protected void Spawn(uint race, uint amount, uint region, MabiVertex pos, uint radius = 0)
		{
			WorldManager.Instance.SpawnCreature(race, amount, region, pos, radius);
		}

		/// <summary>
		/// Returns random int between from and too (inclusive).
		/// </summary>
		protected int Rnd(int from, int to)
		{
			return _rnd.Next(from, to);
		}

		protected void Notice(WorldClient client, string msg, NoticeType type = NoticeType.MiddleTop)
		{
			if (client == null)
				return;

			client.Send(PacketCreator.Notice(msg, type));
		}

		protected void Broadcast(string msg, NoticeType type = NoticeType.Top)
		{
			WorldManager.Instance.Broadcast(PacketCreator.Notice(msg, type), SendTargets.All);
		}
	}

	public enum PropAction { None, Warp, Drop }
}
