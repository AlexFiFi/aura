// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Aura.Data;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.World.Util;
using Aura.World.World;
using Aura.World.Events;

namespace Aura.World.Scripting
{
	public partial class BaseScript : IDisposable
	{
		public string ScriptPath { get; set; }
		public string ScriptName { get; set; }

		public bool Disposed { get; protected set; }

		/// <summary>
		/// Use this rnd to avoid "syncronized randomness"
		/// </summary>
		protected static readonly Random rnd = RandomProvider.Get();


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
		/// "Redirect" to WorldManager.Instance.SpawnCreature.
		/// </summary>
		protected void Spawn(uint race, uint amount, uint region, uint x, uint y, uint radius = 0, bool effect = false)
		{
			WorldManager.Instance.SpawnCreature(race, amount, region, x, y, radius, effect);
		}

		/// <summary>
		/// "Redirect" to WorldManager.Instance.SpawnCreature.
		/// </summary>
		protected void Spawn(uint race, uint amount, uint region, MabiVertex pos, uint radius = 0, bool effect = false)
		{
			WorldManager.Instance.SpawnCreature(race, amount, region, pos, radius, effect);
		}

		/// <summary>
		/// Returns random int between from and too (inclusive).
		/// </summary>
		protected int Rnd(int from, int to)
		{
			return rnd.Next(from, to);
		}

		/// <summary>
		/// Returns random double between 0.0 and 1.0.
		/// </summary>
		protected double Rnd()
		{
			return rnd.NextDouble();
		}

		protected void Notice(WorldClient client, string msg, NoticeType type = NoticeType.MiddleTop)
		{
			if (client == null)
				return;

			Send.Notice(client, msg, type);
		}

		protected void Broadcast(string msg, NoticeType type = NoticeType.Top)
		{
			Send.AllNotice(type, msg);
		}

		protected void AddHook(string npc, string hook, ScriptHook func)
		{
			ScriptManager.Instance.AddHook(npc, hook, func);
		}

		protected string L(string key)
		{
			return Localization.Get(key);
		}
	}

	public enum PropAction { None, Warp, Drop }
}
