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
	class FireballHandler : SkillHandler
	{
		public const ushort UseStun = 0, KnockBack = 40;

		public override SkillResults Prepare(World.MabiCreature creature, World.MabiSkill skill, Shared.Network.MabiPacket packet, uint castTime)
		{
			if (creature.IsMoving)
			{
				creature.StopMove();
				WorldManager.Instance.SendStopMove(creature);
			}

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.SkillInit).PutString("fireball").PutShort((ushort)skill.Id), SendTargets.Range, creature);

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
				WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.StackUpdate).PutString("firebolt").PutBytes(creature.ActiveSkillStacks, 0), SendTargets.Range, creature);

				// Casting motion stop?
				WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.HoldMagic).PutString("fireball").PutShort(skill.Info.Id), SendTargets.Range, creature);
			}

			creature.Client.SendSkillReady(creature, skill.Id);

			return SkillResults.Okay;
		}

		public override SkillResults Use(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			return UseCombat(creature, packet.GetLong(), skill);
		}

		public override SkillResults UseCombat(MabiCreature attacker, ulong targetId, MabiSkill skill)
		{
			var target = WorldManager.Instance.GetCreatureById(targetId);
			if (target == null)
				return SkillResults.InvalidTarget;

			if (!WorldManager.InRange(attacker, target, 2000))
				return SkillResults.OutOfRange;

			if (attacker.ActiveSkillStacks != 5)
				return SkillResults.Failure;

			attacker.StopMove();

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, attacker.Id).PutInt(Effect.UseMagic).PutString("fireball").PutByte(1).PutLong(targetId).PutShort((ushort)skill.Id), SendTargets.Range, attacker);

			WorldManager.Instance.Broadcast(PacketCreator.TurnTo(attacker, target).PutByte(1), SendTargets.Range, attacker);

			var pos = target.GetPosition();
			var bomb = new MabiProp(target.Region, MabiData.RegionDb.GetAreaId(target.Region, pos.X, pos.Y));
			bomb.Info.X = pos.X;
			bomb.Info.Y = pos.Y;
			bomb.Info.Scale = bomb.Info.Direction = 1f;
			bomb.Info.Class = 280;
			WorldManager.Instance.AddProp(bomb);

			var frompos = attacker.GetPosition();

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, bomb.Id).PutInts(Effect.FireballFly, bomb.Region).PutFloats(frompos.X, frompos.Y, pos.X, pos.Y).PutInt(4000).PutByte(0).PutInt((uint)skill.Id), SendTargets.Range, attacker);

			attacker.Client.SendSkillUse(attacker, skill.Id, UseStun, 1);

			SkillHelper.ClearStack(attacker, skill);

			// End the fire bolts
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, attacker.Id).PutInt(Effect.StackUpdate).PutString("firebolt").PutBytes(0, 0), SendTargets.Range, attacker);

			SkillHelper.GiveSkillExp(attacker, skill, 20);

			System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
				{
					this.FireballProcessing(attacker, skill, bomb, targetId);
				}));
			t.Start();

			return SkillResults.Okay;
		}

		public void FireballProcessing(MabiCreature attacker, MabiSkill skill, MabiProp bomb, ulong targetId)
		{
			System.Threading.Thread.Sleep(4000);

			var victims = WorldManager.Instance.GetCreaturesInRange(bomb, 800).Where(c => c.IsAttackableBy(attacker)).ToList();

			ulong areaTarget = SkillHelper.GetAreaTargetID(bomb.Region, (uint)bomb.Info.X, (uint)bomb.Info.Y);

			var sAction = new SourceAction(CombatActionType.SpecialHit, attacker, skill.Id, areaTarget, bomb.Id);
			sAction.Options = SourceOptions.KnockBackHit1 | SourceOptions.UseEffect;

			var cap = new CombatActionPack(attacker, skill.Id);
			cap.Add(sAction);

			var rnd = RandomProvider.Get();

			foreach (var target in victims)
			{
				target.StopMove();

				var tAction = new TargetAction(CombatActionType.TakeHit, target, attacker, skill.Id);
				tAction.Options |= TargetOptions.Result;
				tAction.StunTime = target.Stun = 3000;
				tAction.Delay = 200;

				var damage = attacker.GetMagicDamage(attacker.RightHand, rnd.Next((int)skill.RankInfo.Var1, (int)skill.RankInfo.Var2 + 1));

				if (CombatHelper.TryAddCritical(target, ref damage, attacker.CriticalChance))
					tAction.Options |= TargetOptions.Critical;

				target.TakeDamage(tAction.Damage = damage);

				// Knock down if dead
				tAction.OldPosition = CombatHelper.KnockBack(target, bomb);
				if (target.IsDead)
				{
					tAction.Options |= TargetOptions.FinishingKnockDown;
				}
				else
				{
					tAction.Options |= TargetOptions.KnockDown;
					CombatHelper.SetAggro(attacker, target);
				}

				WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, target.Id).PutInt(Effect.Thunderbolt).PutByte(0), SendTargets.Range, target);
				cap.Add(tAction);
			}

			WorldManager.Instance.HandleCombatActionPack(cap);

			WorldManager.Instance.RemoveProp(bomb);
		}

		private static void HitWithThunderbolt(MabiCreature attacker, MabiSkill skill, List<MabiCreature> targets, MabiProp cloud, float dmgModifier = 1)
		{
			var undead = targets.FirstOrDefault(c => !c.IsDead);

			if (undead == null)
				return;

			var sAction = new SourceAction(CombatActionType.RangeHit, attacker, skill.Id, undead.Id, cloud.Id);
			sAction.Options = SourceOptions.KnockBackHit1 | SourceOptions.UseEffect;

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

				if (CombatHelper.TryAddCritical(target, ref damage, attacker.CriticalChance))
					tAction.Options |= TargetOptions.Critical;

				target.TakeDamage(tAction.Damage = damage);

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

				WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, target.Id).PutInt(Effect.Thunderbolt).PutByte(0), SendTargets.Range, target);
				cap.Add(tAction);
			}

			WorldManager.Instance.HandleCombatActionPack(cap);
		}

		public void GetThunderChain(MabiCreature initialTarget, List<MabiCreature> targets, int maxTargets)
		{
			var inrange = WorldManager.Instance.GetCreaturesInRange(initialTarget, 500).Where(c => c is MabiNPC && !c.IsDead).ToList();

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
					GetThunderChain(t, targets, maxTargets);
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
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.StackUpdate).PutString("firebolt").PutBytes(creature.ActiveSkillStacks, 0), SendTargets.Range, creature);

			WorldManager.Instance.Broadcast(new MabiPacket(Op.MotionCancel2, creature.Id).PutByte(1), SendTargets.Range, creature);

			return SkillResults.Okay;
		}
	}
}
