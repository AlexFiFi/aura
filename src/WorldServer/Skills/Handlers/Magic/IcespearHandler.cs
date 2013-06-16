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
	class IcespearHandler : SkillHandler
	{
		public const ushort UseStun = 500, KnockBack = 40;

		public override SkillResults Prepare(World.MabiCreature creature, World.MabiSkill skill, Shared.Network.MabiPacket packet, uint castTime)
		{
			if (creature.IsMoving)
			{
				creature.StopMove();
			}

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.SkillInit).PutString("icespear").PutShort((ushort)skill.Id), SendTargets.Range, creature);

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

				WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.StackUpdate).PutString("icespear").PutBytes(creature.ActiveSkillStacks, 0), SendTargets.Range, creature);

				// Casting motion stop?
				WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.HoldMagic).PutString("icespear").PutShort(skill.Info.Id), SendTargets.Range, creature);
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

			var targets = this.GetIceSpearLOSChain(attacker, itarget, (uint)skill.RankInfo.Var5);

			foreach (var target in targets)
			{
				target.StopMove();
				WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, target.Id).PutInt(Effect.IcespearFreeze).PutByte(1).PutInt(13000), SendTargets.Range, target);
			}

			var aPos = attacker.GetPosition();
			var casterProp = new MabiProp(280, attacker.Region, aPos.X, aPos.Y, 0); // 3
			WorldManager.Instance.AddProp(casterProp);

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, attacker.Id).PutInt(Effect.UseMagic).PutString("icespear").PutByte(1).PutLong(targetId).PutShort((ushort)skill.Id), SendTargets.Range, attacker);

			var charges = attacker.ActiveSkillStacks;

			attacker.Client.SendSkillUse(attacker, skill.Id, targetId);

			SkillHelper.ClearStack(attacker, skill);

			// End the spear
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, attacker.Id).PutInt(Effect.StackUpdate).PutString("icespear").PutBytes(0, 0), SendTargets.Range, attacker);

			var sAction = new AttackerAction(CombatActionType.SpecialHit, attacker, skill.Id, targetId);
			sAction.Options = AttackerOptions.Result;

			var cap = new CombatActionPack(attacker, skill.Id);
			cap.Add(sAction);

			foreach (var target in targets)
			{
				var tAction = new TargetAction(CombatActionType.None, target, attacker, skill.Id);
				cap.Add(tAction);
			}

			WorldManager.Instance.HandleCombatActionPack(cap);

			SkillHelper.GiveSkillExp(attacker, skill, 20);

			System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
			{
				this.IcespearProcessing(attacker, skill, targets, casterProp, 3500, charges);
			}));
			t.Start();

			return SkillResults.Okay;
		}

		protected void IcespearProcessing(MabiCreature attacker, MabiSkill skill, List<MabiCreature> targets, MabiProp casterProp, int sleep, int stack)
		{
			float dmg = (stack == 5 ? 6.5f : stack);
			for (int i = stack; i >= 0; i--)
			{
				foreach (var target in targets)
				{
					target.StopMove();
					target.Stun = (ushort)sleep;
					WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, target.Id).PutInt(Effect.IcespearFreeze).PutByte(1).PutInt(13000), SendTargets.Range, attacker);
				}
				System.Threading.Thread.Sleep(sleep);
				var newTargets = new List<MabiCreature>();
				foreach (var target in targets)
				{
					this.Explode(attacker, target, skill, casterProp, dmg);
					var tmpTargets = new List<MabiCreature>();
					GetIceSpearExplosionChain(attacker, target, tmpTargets, 16, (uint)skill.RankInfo.Var4);
					if (tmpTargets.Contains(target))
						tmpTargets.Remove(target);

					newTargets.AddRange(tmpTargets);
				}
				targets = newTargets.Distinct().Where(c => !c.IsDead).Take(16).ToList();
			}

			WorldManager.Instance.RemoveProp(casterProp);
		}

		protected void Explode(MabiCreature attacker, MabiCreature target, MabiSkill skill, MabiProp casterProp, float dmgModifier = 1f)
		{
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, target.Id).PutInt(Effect.IcespearFreeze).PutByte(1).PutInt(0), SendTargets.Range, target); // Cancel freeze

			var tPos = target.GetPosition();
			var bombProp = new MabiProp(280, target.Region, tPos.X, tPos.Y, 0); //4
			WorldManager.Instance.AddProp(bombProp);

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, bombProp.Id).PutInts(Effect.IcespearBoom, target.Region, tPos.X, tPos.Y), SendTargets.Range, bombProp);

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, target.Id).PutInt(Effect.Thunderbolt).PutByte(0), SendTargets.Range, target);

			var sAction = new AttackerAction(CombatActionType.SpecialHit, attacker, skill.Id, SkillHelper.GetAreaTargetID(target.Region, tPos.X, tPos.Y));
			sAction.PropId = casterProp.Id;
			sAction.Options = AttackerOptions.KnockBackHit1 | AttackerOptions.UseEffect;

			var tAction = new TargetAction(CombatActionType.TakeHit, target, attacker, skill.Id);
			tAction.Options = TargetOptions.Result;
			tAction.StunTime = target.Stun = 2000;

			var rnd = RandomProvider.Get();

			var damage = attacker.GetMagicDamage(attacker.RightHand, rnd.Next((int)skill.RankInfo.Var1, (int)skill.RankInfo.Var2 + 1)) * dmgModifier;

			if (CombatHelper.TryAddCritical(target, ref damage, attacker.CriticalChanceAgainst(target)))
				tAction.Options |= TargetOptions.Critical;

			target.TakeDamage(tAction.Damage = damage, attacker, skill.Id);

			// Knock down if dead
			tAction.OldPosition = CombatHelper.KnockBack(target, bombProp);
			if (target.IsDead)
			{
				tAction.Options |= TargetOptions.FinishingKnockDown;
			}
			else
			{
				tAction.Options |= TargetOptions.KnockDown;
				CombatHelper.SetAggro(attacker, target);
			}

			var cap = new CombatActionPack(attacker, skill.Id);
			cap.Add(sAction, tAction);

			WorldManager.Instance.HandleCombatActionPack(cap);

			sAction = new AttackerAction(CombatActionType.SpecialHit, attacker, skill.Id, SkillHelper.GetAreaTargetID(target.Region, tPos.X, tPos.Y));
			sAction.PropId = bombProp.Id;
			sAction.Options = AttackerOptions.UseEffect;

			cap = new CombatActionPack(attacker, skill.Id);
			cap.Add(sAction);

			WorldManager.Instance.HandleCombatActionPack(cap);

			WorldManager.Instance.RemoveProp(bombProp);
		}

		public List<MabiCreature> GetIceSpearLOSChain(MabiCreature attacker, MabiCreature target, uint range)
		{
			var aPos = attacker.GetPosition();
			var tPos = target.GetPosition();

			var minX = Math.Min(aPos.X, tPos.X) - range;
			var maxX = Math.Max(aPos.X, tPos.X) + range;

			var m = ((double)tPos.Y - aPos.Y) / ((double)tPos.X - aPos.X);
			var bL = (double)aPos.Y - aPos.X * m - (range / 2);
			var bH = bL + range;

			var targets = WorldManager.Instance.GetCreaturesInRange(attacker, maxX - minX); // Rough filter

			targets = targets.FindAll((c) =>
				{
					var pos = c.GetPosition();
					return !c.IsDead && c.IsAttackableBy(attacker) && (minX < pos.X && pos.X < maxX) && ((m * pos.X + bL) < pos.Y && pos.Y < (m * pos.X + bH));
				});

			if (!targets.Contains(target))
				targets.Add(target);

			return targets;
		}

		public void GetIceSpearExplosionChain(MabiCreature attacker, MabiCreature initialTarget, List<MabiCreature> targets, int maxTargets, uint range)
		{
			var inrange = WorldManager.Instance.GetCreaturesInRange(initialTarget, range).Where(c => c.IsAttackableBy(attacker) && !c.IsDead).ToList();

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
					GetIceSpearExplosionChain(attacker, t, targets, maxTargets, range);
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
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.StackUpdate).PutString("icespear").PutBytes(creature.ActiveSkillStacks, 0), SendTargets.Range, creature);

			WorldManager.Instance.Broadcast(new MabiPacket(Op.MotionCancel2, creature.Id).PutByte(1), SendTargets.Range, creature);

			return SkillResults.Okay;
		}
	}
}
