// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Common.Constants;
using Common.World;
using World.World;
using Common.Network;

namespace World.Skills
{
	public class SpiritOfOrderHandler : SkillHandler
	{
		public override SkillResults Start(MabiCreature creature, MabiSkill skill)
		{
			WorldManager.Instance.Broadcast(GetPacket(creature, skill, true), SendTargets.Range, creature);

			//creature.Title = 40003;
			//WorldManager.Instance.CreatureChangeTitle(creature);

			//Pdef
			//creature.Client.Send(new MabiPacket(0x9091, creature.Id).PutInts(6500, 58).PutShort(40011));
			//creature.Client.Send(new MabiPacket(0x9091, creature.Id).PutInts(8000, 58).PutShort(40012));

			creature.StrMod += 999;
			creature.DexMod += 999;
			WorldManager.Instance.CreatureStatsUpdate(creature);

			return SkillResults.Okay;
		}

		public override SkillResults Stop(MabiCreature creature, MabiSkill skill)
		{
			WorldManager.Instance.Broadcast(GetPacket(creature, skill, false), SendTargets.Range, creature);

			creature.StrMod -= 999;
			creature.DexMod -= 999;
			WorldManager.Instance.CreatureStatsUpdate(creature);

			return SkillResults.Okay;
		}

		private MabiPacket GetPacket(MabiCreature creature, MabiSkill skill, bool transforming)
		{
			// TODO: Add actual transform level from rank info.
			// intVal1 = look, intVal2 = titleId
			var p = new MabiPacket(0xA41C, creature.Id);
			p.PutByte((byte)(transforming ? this.TransformId : (byte)0));
			p.PutShort((ushort)(transforming ? 15 : 0));
			p.PutShort((ushort)(transforming ? ((skill.Info.Rank + 1) / 4) : 0));
			p.PutByte(1);
			return p;
		}

		protected virtual byte TransformId { get { return 1; } }
	}

	public class SoulOfChaosHandler : SpiritOfOrderHandler
	{
		protected override byte TransformId { get { return 2; } }
	}

	public class FuryOfConnousHandler : SpiritOfOrderHandler
	{
		protected override byte TransformId { get { return 4; } }
	}
}
