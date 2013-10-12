// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.World.Network;
using Aura.World.World;
using Aura.Shared.Util;

namespace Aura.World.Skills
{
	[SkillAttr(SkillConst.HiddenResurrection)]
	public class HiddenResurrectionHandler : SkillHandler
	{
		public override SkillResults Prepare(MabiCreature creature, MabiSkill skill, MabiPacket packet, uint castTime)
		{
			// Check item id parameter
			var parameters = packet.GetString();
			var itemOId = MabiTags.Fetch<ulong>("ITEMID", parameters);
			if (itemOId == 0)
				return SkillResults.Failure;

			// Check item
			var item = creature.Inventory.GetItem(itemOId);
			if (item == null)
				return SkillResults.Failure;

			creature.Temp.SkillItem1 = item;

			Send.SkillReady(creature.Client, creature, skill.Id, "");

			return SkillResults.Okay;
		}

		public override SkillResults Use(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			var targetId = packet.GetLong();
			var target = WorldManager.Instance.GetCreatureById(targetId);
			if (target == null || !target.IsDead || (target.RevivalOptions & DeadMenuOptions.WaitForRescue) == 0 || !target.WaitingForRes)
				return SkillResults.InvalidTarget;

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.UseMagic).PutString("healing_phoenix").PutLong(target.Id), SendTargets.Range, creature);

			Send.SkillUse(creature.Client, creature, skill.Id, targetId);

			return SkillResults.Okay;
		}

		public override SkillResults Complete(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			var targetId = packet.GetLong();
			// 2 unk ints in older logs
			//var unk1 = packet.GetInt();
			//var unk2 = packet.GetInt();

			var target = WorldManager.Instance.GetCreatureById(targetId);
			if (target == null || !target.IsDead)
				return SkillResults.InvalidTarget;

			if (creature.Temp.SkillItem1 == null || creature.Temp.SkillItem1.Info.Class != 63000 || creature.Temp.SkillItem1.Info.Amount < 1)
				return SkillResults.Failure;

			creature.Inventory.DecItem(creature.Temp.SkillItem1);

			// TODO: stats

			creature.Revive();

			Send.SkillComplete(creature.Client, creature, skill.Id, targetId);

			return SkillResults.Okay;
		}
	}
}
