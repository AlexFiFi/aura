// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.World.World;
using Aura.World.Network;
using Aura.Shared.Network;
using Aura.Shared.Const;
using Aura.Data;
using Aura.Shared.Util;

namespace Aura.World.Skills
{
	class ThunderHandler : SkillHandler
	{
		public const ushort UseStun = 500, KnockBack = 40;

		public override SkillResults Prepare(World.MabiCreature creature, World.MabiSkill skill, Shared.Network.MabiPacket packet, uint castTime)
		{
			if (creature.IsMoving)
			{
				creature.StopMove();
			}

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.SkillInit).PutString("thunder").PutShort((ushort)skill.Id), SendTargets.Range, creature);

			creature.Client.SendSkillPrepare(creature, skill.Id, castTime);

			return SkillResults.Okay;
		}

		public override SkillResults Ready(MabiCreature creature, MabiSkill skill)
		{
			if (creature.ActiveSkillStacks < skill.RankInfo.StackMax)
			{
				// Stack
				if (!creature.HasSkill(SkillConst.ChainCasting))
					SkillHelper.IncStack(creature, skill);
				else
					SkillHelper.FillStack(creature, skill);
				WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.StackUpdate).PutString("lightningbolt").PutBytes(creature.ActiveSkillStacks, 0), SendTargets.Range, creature);

				// Casting motion stop?
				WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.HoldMagic).PutString("thunder").PutShort(skill.Info.Id), SendTargets.Range, creature);
			}

			creature.Client.SendSkillReady(creature, skill.Id);

			return SkillResults.Okay;
		}

		public override SkillResults UseCombat(MabiCreature attacker, ulong targetId, MabiSkill skill)
		{
			var itarget = WorldManager.Instance.GetCreatureById(targetId);
			if (itarget == null)
				return SkillResults.InvalidTarget;

			if (!WorldManager.InRange(attacker, itarget, 2000))
				return SkillResults.OutOfRange;

			attacker.StopMove();

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, attacker.Id).PutInt(Effect.UseMagic).PutString("thunder").PutByte(0).PutLong(targetId).PutShort((ushort)skill.Id), SendTargets.Range, attacker);

			var charges = attacker.ActiveSkillStacks;

			List<MabiCreature> targets = new List<MabiCreature>() { itarget };
			this.GetThunderChain(attacker, itarget, targets, ((byte)skill.Rank >= (byte)SkillRank.R1 ? 4 : 1) + (charges - 1) * 2);

			var pos = itarget.GetPosition();
			var cloud = new MabiProp(280, itarget.Region, pos.X, pos.Y, 0);
			WorldManager.Instance.AddProp(cloud);

			var lbPacket = new MabiPacket(Op.Effect, Id.Broadcast).PutInt(Effect.Lightningbolt).PutLong(attacker.Id).PutInt((uint)targets.Count);

			foreach (var target in targets)
			{
				lbPacket.PutLong(target.Id);
			}

			WorldManager.Instance.Broadcast(lbPacket, SendTargets.Range, attacker);

			attacker.Client.SendSkillUse(attacker, skill.Id, UseStun, 1);

			SkillHelper.ClearStack(attacker, skill);

			// End the lightning balls
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, attacker.Id).PutInt(Effect.StackUpdate).PutString("lightningbolt").PutBytes(0, 0), SendTargets.Range, attacker);

			var sAction = new AttackerAction(CombatActionType.RangeHit, attacker, skill.Id, targetId);
			sAction.Options |= AttackerOptions.Result;

			attacker.Stun = sAction.StunTime = UseStun;

			var cap = new CombatActionPack(attacker, skill.Id);
			cap.Add(sAction);

			var rnd = RandomProvider.Get();

			foreach (var target in targets)
			{
				var tAction = new TargetAction(CombatActionType.TakeHit, target, attacker, skill.Id);
				tAction.Options |= TargetOptions.Result;

				cap.Add(tAction);

				var damage = attacker.GetMagicDamage(attacker.RightHand, rnd.Next((int)skill.RankInfo.Var4, (int)skill.RankInfo.Var5 + 1));

				if (CombatHelper.TryAddCritical(target, ref damage, attacker.CriticalChanceAgainst(target)))
					tAction.Options |= TargetOptions.Critical;

				target.TakeDamage(tAction.Damage = damage, attacker, skill.Id);

				// Knock down if dead
				if (target.IsDead)
				{
					tAction.OldPosition = CombatHelper.KnockBack(target, attacker);
					tAction.Options |= TargetOptions.FinishingKnockDown;
				}
				else
				{
					if (target.KnockBack >= CombatHelper.LimitKnockBack)
					{
						tAction.Options |= TargetOptions.KnockDown;
					}
					else
					{
						target.KnockBack += KnockBack;
						if (target.KnockBack >= CombatHelper.LimitKnockBack)
						{
							tAction.OldPosition = CombatHelper.KnockBack(target, attacker);
							tAction.Options |= TargetOptions.KnockBack;
						}
					}

					target.Stun = tAction.StunTime = (ushort)skill.RankInfo.Var3;

					CombatHelper.SetAggro(attacker, target);
				}
			}

			WorldManager.Instance.HandleCombatActionPack(cap);

			SkillHelper.GiveSkillExp(attacker, skill, 20);

			System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
				{
					this.ThunderProcessing(attacker, skill, targets, cloud, DateTime.Now.AddMilliseconds(skill.RankInfo.Var3), charges);
				}));
			t.Start();

			return SkillResults.Okay;
		}

		public void ThunderProcessing(MabiCreature attacker, MabiSkill skill, List<MabiCreature> targets, MabiProp cloud, DateTime stunEnd, int stack)
		{
			var lbPacket = new MabiPacket(Op.Effect, Id.Broadcast).PutInt(Effect.Lightningbolt).PutLong(attacker.Id).PutInt((uint)targets.Count);

			foreach (var target in targets)
			{
				lbPacket.PutLong(target.Id);
			}

			while (DateTime.Now < stunEnd)
			{
				System.Threading.Thread.Sleep(500);
				WorldManager.Instance.Broadcast(lbPacket, SendTargets.Range, attacker);
			}

			for (int i = 1; i < stack; i++)
			{
				HitWithThunderbolt(attacker, skill, targets, cloud);
				System.Threading.Thread.Sleep(500);
			}

			HitWithThunderbolt(attacker, skill, targets, cloud, (stack == 5 ? 2 : 1.5f));

			WorldManager.Instance.RemoveProp(cloud);
		}

		private static void HitWithThunderbolt(MabiCreature attacker, MabiSkill skill, List<MabiCreature> targets, MabiProp cloud, float dmgModifier = 1)
		{
			var undead = targets.FirstOrDefault(c => !c.IsDead);

			if (undead == null)
				return;

			var sAction = new AttackerAction(CombatActionType.RangeHit, attacker, skill.Id, undead.Id);
			sAction.PropId = cloud.Id;
			sAction.Options = AttackerOptions.KnockBackHit1 | AttackerOptions.UseEffect;

			var cap = new CombatActionPack(attacker, skill.Id);
			cap.Add(sAction);

			var rnd = RandomProvider.Get();

			foreach (var target in targets)
			{
				if (target.IsDead)
					continue;

				var tAction = new TargetAction(CombatActionType.TakeHit, target, attacker, skill.Id);
				tAction.Options |= TargetOptions.Result;

				var damage = attacker.GetMagicDamage(attacker.RightHand, rnd.Next((int)skill.RankInfo.Var1, (int)skill.RankInfo.Var2 + 1)) * dmgModifier;

				if (CombatHelper.TryAddCritical(target, ref damage, attacker.CriticalChanceAgainst(target)))
					tAction.Options |= TargetOptions.Critical;

				target.TakeDamage(tAction.Damage = damage, attacker, skill.Id);

				// Knock down if dead
				if (target.IsDead)
				{
					tAction.OldPosition = CombatHelper.KnockBack(target, attacker);
					tAction.Options |= TargetOptions.FinishingKnockDown;
				}
				else
				{
					if (target.KnockBack >= CombatHelper.LimitKnockBack)
					{
						tAction.Options |= TargetOptions.KnockDown;
					}
					else
					{
						target.KnockBack += KnockBack;
						if (target.KnockBack >= CombatHelper.LimitKnockBack)
						{
							tAction.OldPosition = CombatHelper.KnockBack(target, attacker);
							tAction.Options |= TargetOptions.KnockBack;
						}
					}

					target.Stun = tAction.StunTime = 2000;

					CombatHelper.SetAggro(attacker, target);
				}

				WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, target.Id).PutInt(Effect.Thunderbolt).PutByte(0), SendTargets.Range, target);
				cap.Add(tAction);
			}

			WorldManager.Instance.HandleCombatActionPack(cap);
		}

		public void GetThunderChain(MabiCreature attacker, MabiCreature initialTarget, List<MabiCreature> targets, int maxTargets)
		{
			var inrange = WorldManager.Instance.GetCreaturesInRange(initialTarget, 500).Where(c => c.IsAttackableBy(attacker) && !c.IsDead).ToList();

			foreach (var t in inrange)
			{
				if (targets.Count >= maxTargets)
					return;
				if (!targets.Contains(t))
					targets.Add(t);
			}

			foreach (var t in inrange)
			{
				if (targets.Count >= maxTargets)
					return;

				if (!targets.Contains(t))
					GetThunderChain(attacker, t, targets, maxTargets);
			}
		}

		public override SkillResults Complete(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			creature.Client.SendSkillComplete(creature, skill.Id);

			if (creature.ActiveSkillStacks > 0)
				creature.Client.SendSkillComplete(creature, skill.Id);

			return SkillResults.Okay;
		}

		public override SkillResults Cancel(MabiCreature creature, MabiSkill skill)
		{
			SkillHelper.ClearStack(creature, skill);
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.StackUpdate).PutString("lightningbolt").PutBytes(creature.ActiveSkillStacks, 0), SendTargets.Range, creature);

			WorldManager.Instance.Broadcast(new MabiPacket(Op.MotionCancel2, creature.Id).PutByte(1), SendTargets.Range, creature);

			return SkillResults.Okay;
		}
	}
}
