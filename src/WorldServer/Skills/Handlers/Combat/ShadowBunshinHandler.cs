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
using Aura.Shared.Const;

namespace Aura.World.Skills
{
	/// <summary>
	/// Skill is hidden without gfShadowBunshin being enabled in the client.
	/// </summary>
	public class ShadowBunshinHandler : SkillHandler
	{
		private const uint Radius = 400;
		private const uint Range = 1200;

		public override SkillResults Prepare(MabiCreature creature, MabiSkill skill, MabiPacket packet, uint castTime)
		{
			// Is this check done client sided in every client by now?
			if (WorldConf.BunshinSouls && creature is MabiPC && creature.SoulCount < skill.RankInfo.Var1)
			{
				creature.Client.SendNotice("You need {0} more souls\nto be able to use Shadow Bunshin.", (skill.RankInfo.Var1 - creature.SoulCount));
				return SkillResults.Failure;
			}

			if (creature.IsMoving)
			{
				creature.StopMove();
			}

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.ShadowBunshin).PutByte(1), SendTargets.Range, creature);
			creature.Client.SendSkillPrepare(creature, skill.Id, castTime);

			return SkillResults.Okay;
		}

		public override SkillResults Ready(MabiCreature creature, MabiSkill skill)
		{
			creature.Client.SendSkillReady(creature, skill.Id);

			return SkillResults.Okay;
		}

		public override SkillResults Complete(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			creature.Client.SendSkillComplete(creature, skill.Id);

			return SkillResults.Okay;
		}

		public override SkillResults Cancel(MabiCreature creature, MabiSkill skill)
		{
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.ShadowBunshin).PutByte(4), SendTargets.Range, creature);

			return SkillResults.Okay;
		}

		public override SkillResults Use(MabiCreature attacker, MabiSkill skill, MabiPacket packet)
		{
			var targetId = packet.GetLong();
			var unk1 = packet.GetInt();
			var unk2 = packet.GetInt();

			var target = WorldManager.Instance.GetCreatureById(targetId);
			if (target == null)
				return SkillResults.InvalidTarget;

			//if (!WorldManager.InRange(creature, target, Range))
			//    return SkillResults.OutOfRange;

			// X% of Stamina
			var staminaCost = attacker.Stamina * (skill.RankInfo.Var2 / 100f);
			if (attacker is MabiPC)
				attacker.Stamina -= staminaCost;

			target.StopMove();

			var clones = (uint)skill.RankInfo.Var1;
			attacker.SoulCount = 0;

			// Spawn clones
			var pos = target.GetPosition();
			WorldManager.Instance.Broadcast(
				new MabiPacket(Op.Effect, attacker.Id)
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
			WorldManager.Instance.Broadcast(PacketCreator.TurnTo(attacker, target), SendTargets.Range, attacker);

			// Jump to clone circle
			var toPos = WorldManager.CalculatePosOnLine(attacker, target, -(int)Radius);
			attacker.SetPosition(toPos.X, toPos.Y);
			WorldManager.Instance.Broadcast(
				new MabiPacket(Op.SetLocation, attacker.Id)
				.PutByte(0)
				.PutInt(toPos.X)
				.PutInt(toPos.Y)
			, SendTargets.Range, attacker);

			bool alreadyDead = false;

			uint i = 0;
			Timer timer = null;
			timer = new Timer(_ =>
			{
				if (timer == null || i > clones)
					return;

				// Move
				WorldManager.Instance.Broadcast(
					new MabiPacket(Op.Effect, attacker.Id)
					.PutInt(262)
					.PutByte(3)
					.PutString("move")
					.PutLong(target.Id)
					.PutInt(i) // clone nr
					.PutInt(i) // clone nr
					.PutInt(450)
					.PutInt(clones) // ? (4)
					.PutInt(120) // disappear time?
				, SendTargets.Range, attacker);
				// Attack
				WorldManager.Instance.Broadcast(
					new MabiPacket(Op.EffectDelayed, attacker.Id)
					.PutInt(120) // delay?
					.PutInt(262)
					.PutByte(3)
					.PutString("attack")
					.PutInt(i) // clone nr
				, SendTargets.Range, attacker);

				var sAction = new AttackerAction(CombatActionType.SpecialHit, attacker, skill.Id, targetId);
				sAction.Options |= AttackerOptions.Result;

				var tAction = new TargetAction(CombatActionType.TakeHit, target, attacker, skill.Id);
				tAction.Delay = 100;

				var cap = new CombatActionPack(attacker, skill.Id);
				cap.Add(sAction);

				target.Stun = tAction.StunTime = 2000;
				CombatHelper.SetAggro(attacker, target);

				var rnd = RandomProvider.Get();

				float damage = rnd.Next((int)skill.RankInfo.Var5, (int)skill.RankInfo.Var6 + 1);
				damage += skill.RankInfo.Var7 * staminaCost;

				// Crit
				if (CombatHelper.TryAddCritical(attacker, ref damage, attacker.CriticalChanceAgainst(target)))
					tAction.Options |= TargetOptions.Critical;

				// Def/Prot
				CombatHelper.ReduceDamage(ref damage, target.DefenseTotal, target.Protection);

				// Mana Shield
				tAction.ManaDamage = CombatHelper.DealManaDamage(target, ref damage);

				// Deal Life Damage
				if (damage > 0)
					target.TakeDamage(tAction.Damage = damage, attacker, skill.Id);

				// Save if target was already dead, to not send
				// finish action twice.
				if (!alreadyDead)
				{
					target.TakeDamage(tAction.Damage, attacker, skill.Id);

					// Only send damage taking part if target isn't dead yet.
					cap.Add(tAction);
				}
				alreadyDead = target.IsDead;

				if (target.IsDead)
				{
					tAction.OldPosition = pos;
					if (!alreadyDead)
						tAction.Options |= TargetOptions.FinishingKnockDown;
					else
						tAction.Options |= TargetOptions.KnockDown;
				}
				else if (i == clones)
				{
					// Knock back if not dead after last attack.
					tAction.Options |= TargetOptions.KnockDown;
					tAction.OldPosition = CombatHelper.KnockBack(target, attacker, 400);
				}

				WorldManager.Instance.HandleCombatActionPack(cap);

				if (i >= clones)
				{
					// Cancel timer after last attack.
					timer.Dispose();
					timer = null;
				}

				i++;

				GC.KeepAlive(timer);
			}, null, 900, 450);

			// Something's messed up here, if the skill isn't explicitly
			// canceled the client gets confused.
			//WorldManager.Instance.CreatureSkillCancel(attacker);

			SkillHelper.GiveSkillExp(attacker, skill, 20);

			attacker.Client.SendSkillUse(attacker, skill.Id, targetId, unk1, unk2);

			return SkillResults.Okay;
		}
	}
}
