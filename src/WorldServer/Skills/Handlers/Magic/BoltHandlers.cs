// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.World.Network;
using Aura.World.World;
using Aura.Shared.Network;
using Aura.Shared.Const;
using Aura.Shared.Util;

namespace Aura.World.Skills
{
	public abstract class BoltHandler : SkillHandler
	{
		public virtual ushort UseStun { get { return 500; } }
		public virtual ushort TargetStun { get { return 1650; } }
		public virtual float KnockBack { get { return 40; } }
		public abstract string Name { get; }

		public override SkillResults Prepare(MabiCreature creature, MabiSkill skill, MabiPacket packet, uint castTime)
		{
			creature.StopMove();

			// Casting motion?
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.Casting).PutShort(skill.Info.Id).PutBytes(0, 1).PutShort(0), SendTargets.Range, creature);

			Send.SkillPrepare(creature.Client, creature, skill.Id, castTime);

			return SkillResults.Okay;
		}

		public override SkillResults Ready(MabiCreature creature, MabiSkill skill)
		{
			if (creature.ActiveSkillStacks < skill.RankInfo.StackMax)
			{
				// Stack
				if (!creature.Skills.Has(SkillConst.ChainCasting))
					SkillHelper.IncStack(creature, skill);
				else
					SkillHelper.FillStack(creature, skill);
				WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.StackUpdate).PutString(this.Name).PutBytes(creature.ActiveSkillStacks, 0), SendTargets.Range, creature);

				// Casting motion stop?
				WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.Casting).PutShort(skill.Info.Id).PutBytes(0, 2).PutShort(0), SendTargets.Range, creature);
			}

			Send.SkillReady(creature.Client, creature, skill.Id);

			return SkillResults.Okay;
		}

		public override SkillResults Complete(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			Send.SkillComplete(creature.Client, creature, skill.Id);

			return SkillResults.Okay;
		}

		public override SkillResults Cancel(MabiCreature creature, MabiSkill skill)
		{
			SkillHelper.ClearStack(creature, skill);
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.StackUpdate).PutString(this.Name).PutBytes(creature.ActiveSkillStacks, 0), SendTargets.Range, creature);

			WorldManager.Instance.Broadcast(new MabiPacket(Op.MotionCancel2, creature.Id).PutByte(1), SendTargets.Range, creature);

			return SkillResults.Okay;
		}
	}

	[SkillAttr(SkillConst.Icebolt)]
	public class IceboltHandler : BoltHandler
	{
		public override string Name
		{
			get { return "icebolt"; }
		}

		public override SkillResults UseCombat(MabiCreature attacker, ulong targetId, MabiSkill skill)
		{
			var target = WorldManager.Instance.GetCreatureById(targetId);
			if (target == null)
				return SkillResults.InvalidTarget;

			if (!WorldManager.InRange(attacker, target, 1200))
				return SkillResults.OutOfRange;

			attacker.StopMove();
			target.StopMove();

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, attacker.Id).PutInt(Effect.UseMagic).PutString(this.Name), SendTargets.Range, attacker);

			Send.SkillUse(attacker.Client, attacker, skill.Id, UseStun, 1);

			SkillHelper.DecStack(attacker, skill);

			var sAction = new AttackerAction(CombatActionType.RangeHit, attacker, skill.Id, targetId);
			sAction.Options |= AttackerOptions.Result;

			var tAction = new TargetAction(CombatActionType.TakeHit, target, attacker, skill.Id);
			tAction.Options |= TargetOptions.Result;

			var cap = new CombatActionPack(attacker, skill.Id);
			cap.Add(sAction, tAction);

			var rnd = RandomProvider.Get();

			var damage = attacker.GetMagicDamage(attacker.RightHand, rnd.Next((int)skill.RankInfo.Var1, (int)skill.RankInfo.Var2 + 1));

			if (CombatHelper.TryAddCritical(target, ref damage, attacker.CriticalChanceAgainst(target)))
				tAction.Options |= TargetOptions.Critical;

			target.TakeDamage(tAction.Damage = damage);

			attacker.Stun = sAction.StunTime = UseStun;

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

				target.Stun = tAction.StunTime = TargetStun;

				CombatHelper.SetAggro(attacker, target);
			}

			WorldManager.Instance.HandleCombatActionPack(cap);

			SkillHelper.GiveSkillExp(attacker, skill, 20);

			return SkillResults.Okay;
		}

	}

	[SkillAttr(SkillConst.Firebolt)]
	public class FireboltHandler : BoltHandler
	{
		public override string Name
		{
			get { return "firebolt"; }
		}

		public override ushort TargetStun
		{
			get
			{
				return 3000;
			}
		}

		public override SkillResults UseCombat(MabiCreature attacker, ulong targetId, MabiSkill skill)
		{
			var target = WorldManager.Instance.GetCreatureById(targetId);
			if (target == null)
				return SkillResults.InvalidTarget;

			if (!WorldManager.InRange(attacker, target, 1200))
				return SkillResults.OutOfRange;

			attacker.StopMove();
			target.StopMove();

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, attacker.Id).PutInt(Effect.UseMagic).PutString(this.Name), SendTargets.Range, attacker);

			Send.SkillUse(attacker.Client, attacker, skill.Id, UseStun, 1);

			var sAction = new AttackerAction(CombatActionType.RangeHit, attacker, skill.Id, targetId);
			sAction.Options |= AttackerOptions.Result;

			var tAction = new TargetAction(CombatActionType.TakeHit, target, attacker, skill.Id);
			tAction.Options |= TargetOptions.Result;

			var cap = new CombatActionPack(attacker, skill.Id);
			cap.Add(sAction, tAction);

			var rnd = RandomProvider.Get();

			var damage = attacker.GetMagicDamage(attacker.RightHand, rnd.Next((int)skill.RankInfo.Var1, (int)skill.RankInfo.Var2 + 1));

			damage *= (attacker.ActiveSkillStacks == 5 ? 6.5f : attacker.ActiveSkillStacks);

			if (CombatHelper.TryAddCritical(target, ref damage, attacker.CriticalChanceAgainst(target)))
				tAction.Options |= TargetOptions.Critical;

			SkillHelper.ClearStack(attacker, skill);

			target.TakeDamage(tAction.Damage = damage);

			attacker.Stun = sAction.StunTime = UseStun;

			// Knock down if no adv HS or it's dead AND no HS
			if (!target.Skills.Has(SkillConst.HeavyStander) || (target.IsDead && !target.Skills.Has(SkillConst.HeavyStander)))
			{
				tAction.OldPosition = CombatHelper.KnockBack(target, attacker);
				tAction.Options |= TargetOptions.FinishingKnockDown;
			}
			if (!target.IsDead)
			{
				target.Stun = tAction.StunTime = TargetStun;

				CombatHelper.SetAggro(attacker, target);
			}

			WorldManager.Instance.HandleCombatActionPack(cap);

			SkillHelper.GiveSkillExp(attacker, skill, 20);

			return SkillResults.Okay;
		}
	}

	[SkillAttr(SkillConst.Lightningbolt)]
	public class LightningboltHandler : BoltHandler
	{
		public override string Name
		{
			get { return "lightningbolt"; }
		}

		public override ushort TargetStun
		{
			get
			{
				return 1730;
			}
		}

		public override SkillResults UseCombat(MabiCreature attacker, ulong targetId, MabiSkill skill)
		{
			var target = WorldManager.Instance.GetCreatureById(targetId);
			if (target == null)
				return SkillResults.InvalidTarget;

			if (!WorldManager.InRange(attacker, target, 1200))
				return SkillResults.OutOfRange;

			attacker.StopMove();

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, attacker.Id).PutInt(Effect.UseMagic).PutString(this.Name), SendTargets.Range, attacker);

			Send.SkillUse(attacker.Client, attacker, skill.Id, UseStun, 1);

			var sAction = new AttackerAction(CombatActionType.RangeHit, attacker, skill.Id, targetId);
			sAction.Options |= AttackerOptions.Result;

			var cap = new CombatActionPack(attacker, skill.Id);
			cap.Add(sAction);

			var rnd = RandomProvider.Get();

			var damage = attacker.GetMagicDamage(attacker.RightHand, rnd.Next((int)skill.RankInfo.Var1, (int)skill.RankInfo.Var2 + 1));

			attacker.Stun = sAction.StunTime = UseStun;

			var targets = WorldManager.Instance.GetCreaturesInRange(target, 500).Where(c => !c.IsDead && c.IsAttackableBy(attacker)).ToList();
			targets.Insert(0, target);
			for (byte i = 0; i < attacker.ActiveSkillStacks && i < targets.Count; i++)
			{
				targets[i].StopMove();

				var splashAction = new TargetAction(CombatActionType.TakeHit, targets[i], attacker, skill.Id);
				splashAction.Options |= TargetOptions.Result;

				cap.Add(splashAction);

				splashAction.Damage = damage * ((100 - (i * 10)) / 100f);

				if (CombatHelper.TryAddCritical(targets[i], ref damage, attacker.CriticalChanceAgainst(targets[i])))
					splashAction.Options |= TargetOptions.Critical;

				targets[i].TakeDamage(splashAction.Damage);

				targets[i].Stun = splashAction.StunTime = TargetStun;

				// Knock down if dead
				if (targets[i].IsDead)
				{
					splashAction.OldPosition = CombatHelper.KnockBack(targets[i], (i == 0 ? attacker : targets[i - 1]));
					splashAction.Options |= TargetOptions.FinishingKnockDown;
				}
				else
				{
					if (targets[i].KnockBack >= CombatHelper.LimitKnockBack)
					{
						splashAction.Options |= TargetOptions.KnockDown;
					}
					else
					{
						targets[i].KnockBack += KnockBack;
						if (targets[i].KnockBack >= CombatHelper.LimitKnockBack)
						{
							splashAction.OldPosition = CombatHelper.KnockBack(targets[i], (i == 0 ? attacker : targets[i - 1]));
							splashAction.Options |= TargetOptions.KnockBack;
						}
					}

					targets[i].Stun = splashAction.StunTime = TargetStun;

					CombatHelper.SetAggro(attacker, targets[i]);
				}
			}

			SkillHelper.ClearStack(attacker, skill);

			WorldManager.Instance.HandleCombatActionPack(cap);

			SkillHelper.GiveSkillExp(attacker, skill, 20);

			return SkillResults.Okay;
		}
	}
}
