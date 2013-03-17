// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Threading;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.World.Player;
using Aura.World.Util;
using Aura.World.World;
using Aura.World.Events;

namespace Aura.World.Skills
{
	/// <summary>
	/// Skill is hidden without gfShadowBunshin being enabled in the client.
	/// </summary>
	public class ShadowBunshinHandler : SkillHandler
	{
		private const uint Radius = 400;
		private const uint Range = 1200;

		public override SkillResults Prepare(MabiCreature creature, MabiSkill skill)
		{
			if (WorldConf.BunshinSouls && creature is MabiPC && creature.SoulCount < skill.RankInfo.Var1)
			{
				creature.Client.Send(PacketCreator.Notice(creature, "You need " + (skill.RankInfo.Var1 - creature.SoulCount).ToString() + " more souls\nto be able to use Shadow Bunshin."));
				return SkillResults.Failure;
			}

			this.SetActive(creature, skill);
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(262).PutByte(1), SendTargets.Range, creature);

			creature.StopMove();

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

		public override SkillResults Cancel(MabiCreature creature, MabiSkill skill)
		{
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(262).PutByte(4), SendTargets.Range, creature);

			return SkillResults.Okay;
		}

		public override SkillResults Use(MabiCreature creature, MabiCreature target, MabiSkill skill)
		{
			//if (!WorldManager.InRange(creature, target, Range))
			//    return SkillResults.OutOfRange;

			var staminaCost = creature.Stamina * (skill.RankInfo.Var2 / 100f);
			if (creature is MabiPC)
			{
				creature.Stamina -= staminaCost;
				WorldManager.Instance.CreatureStatsUpdate(creature);
			}

			target.StopMove();

			var clones = (uint)skill.RankInfo.Var1;
			creature.SoulCount = 0;

			// Spawn clones
			var pos = target.GetPosition();
			WorldManager.Instance.Broadcast(
				new MabiPacket(Op.Effect, creature.Id)
				.PutInt(262)
				.PutByte(3)
				.PutString("appear")
				.PutLong(target.Id)
				.PutInt(clones) // amount
				.PutInt(Radius) // radius
				.PutInt(450) // no changes?
				.PutFloat(pos.X)
				.PutFloat(pos.Y)
			, SendTargets.Range, target);

			// Change char look direction.
			WorldManager.Instance.Broadcast(PacketCreator.TurnTo(creature, target), SendTargets.Range, creature);

			// Jump to clone circle
			var toPos = WorldManager.CalculatePosOnLine(creature, target, -(int)Radius);
			creature.SetPosition(toPos.X, toPos.Y);
			WorldManager.Instance.Broadcast(
				new MabiPacket(Op.SetLocation, creature.Id)
				.PutByte(0)
				.PutInt(toPos.X)
				.PutInt(toPos.Y)
			, SendTargets.Range, creature);

			uint i = 0;
			Timer timer = null;
			timer = new Timer(_ =>
			{
				if (timer == null || i > clones)
					return;

				// Move
				WorldManager.Instance.Broadcast(
					new MabiPacket(Op.Effect, creature.Id)
					.PutInt(262)
					.PutByte(3)
					.PutString("move")
					.PutLong(target.Id)
					.PutInt(i) // clone nr
					.PutInt(i) // clone nr
					.PutInt(450)
					.PutInt(clones) // ? (4)
					.PutInt(120) // disappear time?
				, SendTargets.Range, creature);
				// Attack
				WorldManager.Instance.Broadcast(
					new MabiPacket(Op.EffectDelayed, creature.Id)
					.PutInt(120) // delay?
					.PutInt(262)
					.PutByte(3)
					.PutString("attack")
					.PutInt(i) // clone nr
				, SendTargets.Range, creature);

				var combatArgs = new CombatEventArgs();
				combatArgs.CombatActionId = CombatHelper.ActionId;

				var sourceAction = new CombatAction();
				sourceAction.ActionType = CombatActionType.ShadowBunshin;
				sourceAction.SkillId = skill.Id;
				sourceAction.Creature = creature;
				sourceAction.TargetId = target.Id;
				combatArgs.CombatActions.Add(sourceAction);

				var targetAction = new CombatAction();
				targetAction.ActionType = CombatActionType.TakeDamage;
				targetAction.SkillId = skill.Id;
				targetAction.Creature = target;
				targetAction.Target = creature;
				targetAction.ReactionDelay = 100;
				targetAction.StunTime = 2000;

				target.AddStun(targetAction.StunTime, true);

				this.SetAggro(creature, target);

				var rnd = RandomProvider.Get();

				float damage = rnd.Next((int)skill.RankInfo.Var5, (int)skill.RankInfo.Var6 + 1);
				damage += skill.RankInfo.Var7 * staminaCost;

				// Crit
				if (rnd.NextDouble() < creature.CriticalChance)
				{
					damage *= 1.5f; // R1
					targetAction.Critical = true;
				}

				targetAction.CombatDamage = damage;

				// Save if target was already dead, to not send
				// finish action twice.
				var alreadyDead = target.IsDead;
				if (!alreadyDead)
				{
					target.TakeDamage(targetAction.CombatDamage);

					// Only send damage taking part if target isn't dead yet.
					combatArgs.CombatActions.Add(targetAction);
				}

				if (target.IsDead)
				{
					targetAction.OldPosition = pos;
					if (!alreadyDead)
						targetAction.Finish = true;
					else
						targetAction.Knockdown = true;
				}
				else if (i == clones)
				{
					// Knock back if not dead after last attack.
					targetAction.Knockback = true;
					var knockPos = WorldManager.CalculatePosOnLine(creature, target, 400);
					target.SetPosition(knockPos.X, knockPos.Y);
				}

				WorldManager.Instance.CreatureCombatAction(creature, target, combatArgs);
				WorldManager.Instance.CreatureCombatSubmit(creature, combatArgs.CombatActionId);

				WorldManager.Instance.CreatureStatsUpdate(target);

				if (i >= clones)
				{
					// Cancel timer after last attack.
					timer.Dispose();
					timer = null;
				}

				i++;

				GC.KeepAlive(timer);
			}, null, 900, 450);

			this.GiveSkillExp(creature, skill, 20);

			return SkillResults.Okay;
		}
	}
}
