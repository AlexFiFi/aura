// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using Aura.Shared.Const;
using Aura.Data;
using Aura.World.Scripting;
using Aura.World.Player;

namespace Aura.World.World
{
	public class MabiNPC : MabiCreature
	{
		private static ulong _npcIdIndex = 0x10F00000000000;

		public NPCScript Script = null;
		public AIScript AIScript = null;
		public string ScriptPath;

		public uint SpawnId = 0;
		public int GoldMin, GoldMax;
		public List<DropInfo> Drops = new List<DropInfo>();
		public MabiVertex AnchorPoint;

		/// <summary>
		/// Determines whether an NPC can become ancient.
		/// </summary>
		public bool AncientEligible = false;
		/// <summary>
		/// Soonest point in time at which an NPC can become ancient.
		/// </summary>
		public DateTime AncientTime;

		public MabiNPC()
		{
			this.Name = "";
			this.Id = ++_npcIdIndex;
			this.Race = uint.MaxValue;
			this.Region = 0;
			this.SetPosition(0, 0);

			this.Height = 1;
			this.Upper = 1;
			this.Lower = 1;
			this.Fat = 1;

			this.LifeMaxBase = 1000;
			this.Life = 1000;

			this.State |= CreatureStates.GoodNpc;
			this.State |= CreatureStates.NamedNpc;
			this.State |= CreatureStates.Npc;
		}

		public override EntityType EntityType
		{
			get { return EntityType.NPC; }
		}

		public override bool IsAttackableBy(MabiCreature other)
		{
			if (other is MabiPC)
				return (this.State & CreatureStates.GoodNpc) == 0; // Attackable if bad npc
			else
				return (this.State & CreatureStates.GoodNpc) != (other.State & CreatureStates.GoodNpc);
		}

		public override void Die()
		{
			base.Die();
			this.DisappearTime = DateTime.Now.AddSeconds(20); // TODO: Only for mobs
		}
	}
}
