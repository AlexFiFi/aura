// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Common.Constants;
using Common.World;
using World.Scripting;
using System.Collections.Generic;
using System;
using Common.Data;

namespace World.World
{
	public class MabiNPC : MabiCreature
	{
		private static ulong _npcIdIndex = 0x10F00000000000;

		public NPCScript Script = null;
		public AIScript AIScript = null;
		public string ScriptPath;

		public uint SpawnId = 0;
		public int GoldMin, GoldMax;
		public List<MonsterDropInfo> Drops = new List<MonsterDropInfo>();

		public MabiNPC()
		{
			this.Name = "";
			this.Id = ++_npcIdIndex;
			this.Race = uint.MaxValue;
			this.Region = 0;
			this.SetPosition(0, 0);

			this.Height = 1.3f;
			this.Upper = 1.0f;
			this.Lower = 1.0f;
			this.Fat = 1.0f;

			this.LifeMaxBase = 10;
			this.Life = 10;

			this.Status |= CreatureStates.GoodNpc;
			this.Status |= CreatureStates.NamedNpc;
			this.Status |= CreatureStates.Npc;
		}

		public override EntityType EntityType
		{
			get { return EntityType.NPC; }
		}

		public override void Die()
		{
			base.Die();
			this.DisappearTime = DateTime.Now.AddSeconds(20); // TODO: Only for mobs
		}
	}
}
