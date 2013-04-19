// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.World.World;
using Aura.World.Events;

namespace Aura.World.Skills
{
	/// <summary>
	/// Notes about official vs Aura:
	/// Since Windmill doesn't take one target, but works with the attackable
	/// entites around the player we get a pretty big number like
	/// 0x30000001079E08F9 as target id, which is used in the hit part of the
	/// combat action and in the reply to the use packet. Ignored for now,
	/// since the function is unclear, and it works anyway. 
	/// Also for some reason officially the skill id for Combat Mastery is
	/// being sent in the target parts for some reason.
	/// </summary>
	public class WindmillHandler : SkillHandler
	{
		public override SkillResults Prepare(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			this.SetActive(creature, skill);
			this.SkillInit(creature);

			return SkillResults.Okay;
		}

		public override SkillResults Ready(MabiCreature creature, MabiSkill skill)
		{
			return SkillResults.Okay;
		}

		public override SkillResults Complete(MabiCreature creature, MabiSkill skill)
		{
			return SkillResults.Okay;
		}

		public override SkillResults Use(MabiCreature creature, MabiCreature target, MabiSkill skill)
		{
			// Determine range, doesn't seem to be included in rank info.
			uint range = this.GetRange(skill);

			var enemies = WorldManager.Instance.GetAttackableCreaturesInRange(creature, range);
			if (enemies.Count < 1)
			{
				if (creature.Client != null)
				{
					creature.Client.Send(PacketCreator.Notice("Unable to use when there is no target."));
					creature.Client.Send(new MabiPacket(Op.SkillSilentCancel, creature.Id));
				}
				return SkillResults.OutOfRange | SkillResults.NoReply;
			}

			var rnd = RandomProvider.Get();

			creature.StopMove();
			WorldManager.Instance.CreatureUseMotion(creature, 8, 4);

			var combatArgs = new CombatEventArgs();
			combatArgs.CombatActionId = CombatHelper.ActionId;

			// One source action, target actions are added for every target
			// and then we send the combatArgs on their way.
			var sourceAction = new CombatAction();
			sourceAction.ActionType = CombatActionType.Hit;
			sourceAction.SkillId = skill.Id;
			sourceAction.Creature = creature;
			sourceAction.TargetId = 0; // big nr?
			sourceAction.Knockdown = true;
			sourceAction.StunTime = 2500;
			combatArgs.CombatActions.Add(sourceAction);

			creature.AddStun(sourceAction.StunTime, true);

			foreach (var enemy in enemies)
			{
				enemy.StopMove();

				this.SetAggro(creature, enemy);

				var targetAction = new CombatAction();
				targetAction.Creature = enemy;
				targetAction.Target = creature;
				targetAction.ActionType = CombatActionType.TakeDamage;
				targetAction.SkillId = skill.Id;

				var damage = creature.GetRndTotalDamage();
				damage *= skill.RankInfo.Var1 / 100;

				// Crit
				if (rnd.NextDouble() < creature.CriticalChance)
				{
					damage *= 1.5f; // R1
					targetAction.Critical = true;
				}

				targetAction.CombatDamage = damage;
				enemy.TakeDamage(damage);
				targetAction.Finish = enemy.IsDead;

				targetAction.Knockdown = true;
				targetAction.StunTime = 2000;
				enemy.AddStun(targetAction.StunTime, true);

				targetAction.OldPosition = enemy.GetPosition().Copy();
				var pos = WorldManager.CalculatePosOnLine(creature, enemy, 375);
				enemy.SetPosition(pos.X, pos.Y);

				targetAction.ReactionDelay = (uint)rnd.Next(300, 351);

				combatArgs.CombatActions.Add(targetAction);

				WorldManager.Instance.CreatureStatsUpdate(enemy);
			}

			WorldManager.Instance.CreatureCombatAction(creature, null, combatArgs);

			// Officially sent (actually for all skills I guess)
			//WorldManager.Instance.Broadcast(
			//    new MabiPacket(0x7927, creature.Id)
			//    .PutShort(22001)
			//, SendTargets.Range, creature);

			WorldManager.Instance.CreatureCombatSubmit(creature, combatArgs.CombatActionId);

			WorldManager.Instance.CreatureStatsUpdate(creature);

			this.GiveSkillExp(creature, skill, 20);

			return SkillResults.Okay;
		}

		protected virtual uint GetRange(MabiSkill skill)
		{
			uint range = 300;
			if (skill.Rank >= SkillRank.R5)
				range += 100;
			if (skill.Rank == SkillRank.R1)
				range += 100;
			return range;
		}
	}

	/// <summary>
	/// The GM skill has 2 vars, 1000 and 1500, we'll asume it never cost HP,
	/// var 1 is still the damage, and var 2 is here the range.
	/// </summary>
	public class SuperWindmillHandler : WindmillHandler
	{
		protected override uint GetRange(MabiSkill skill)
		{
			return (uint)skill.RankInfo.Var2;
		}
	}
}
