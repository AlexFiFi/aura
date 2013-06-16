using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.World.World;
using Aura.Shared.Const;

namespace Aura.World.Skills
{
	public class CombatFactory
	{
		public AttackerAction AttackerAction { get; protected set; }
		public List<TargetAction> TargetActions { get; protected set; }
		public TargetAction CurrentTargetAction { get; protected set; }

		public CombatFactory()
		{
			TargetActions = new List<TargetAction>();
		}

		public void SetAttackerAction(MabiCreature attacker, CombatActionType type, SkillConst skillId, ulong targetId)
		{
			if (this.AttackerAction != null)
				throw new InvalidOperationException("An attacker may only be set once!");

			this.AttackerAction = new AttackerAction(type, attacker, skillId, targetId);
		}

		public void AddTargetAction(MabiCreature target, CombatActionType targetType, MabiCreature attacker = null, SkillConst skillId = SkillConst.None)
		{
			var tAction = new TargetAction(targetType, target,
				attacker == null ? this.AttackerAction.Creature : attacker,
				skillId == SkillConst.None ? this.AttackerAction.SkillId : skillId);

			this.TargetActions.Add(tAction);
			this.CurrentTargetAction = tAction;
		}

		public void SetActiveTargetAction(MabiCreature target)
		{
			this.SetActiveTargetAction(this.TargetActions.FindIndex(c => c.Creature == target));
		}

		public void SetActiveTargetAction(int index)
		{
			this.CurrentTargetAction = this.TargetActions[index];
		}

		public void SetAttackerOptions(AttackerOptions opts)
		{
			this.AttackerAction.Options |= opts;
		}

		public void SetAttackerStun(ushort stun)
		{
			this.AttackerAction.StunTime = stun;
		}

		public void SetTargetOptions(TargetOptions opts)
		{
			this.CurrentTargetAction.Options |= opts;
		}

		public void SetTargetStun(ushort stun)
		{
			this.CurrentTargetAction.StunTime = stun;
		}

		public void SetTargetDelay(uint delay)
		{
			this.CurrentTargetAction.Delay = delay;
		}

		public void ExecuteDamage(Func<MabiCreature, MabiCreature, float> damageCalculater, CriticalOptions critOpts = CriticalOptions.Calculate)
		{
			foreach (var tAction in TargetActions)
			{
				if (tAction.Is(CombatActionType.None))
					continue;

				var target = tAction.Creature;

				var preLife = target.Life;

				var dmg = damageCalculater(this.AttackerAction.Creature, target); // Initial damage

				if (critOpts == CriticalOptions.Critical || (critOpts == CriticalOptions.Calculate && CombatHelper.TryCritical(tAction.Attacker.CriticalChanceAgainst(tAction.Creature))))
				{
					dmg = CombatHelper.AddCritical(tAction.Attacker, dmg);
					tAction.Options |= TargetOptions.Critical;
				}

				// TODO: PASSIVE DEF

				CombatHelper.ReduceDamage(ref dmg, target.DefenseTotal, target.Protection);

				tAction.ManaDamage = CombatHelper.DealManaDamage(target, ref dmg);

				if (dmg >0)
					target.TakeDamage(tAction.Damage = dmg, AttackerAction.Creature, tAction.SkillId);

				if (target.IsDead)
				{
					tAction.Options |= TargetOptions.FinishingHit | TargetOptions.Finished;

					if (preLife == target.LifeMax)
						tAction.Options |= TargetOptions.OneShotFinish;
				}
			}
		}

		public void ExecuteStun()
		{
			foreach (var tAction in TargetActions)
			{
				if (tAction.Is(CombatActionType.None))
					continue;

				// TODO: P/D
				tAction.Creature.Stun = tAction.StunTime;
			}
		}

		public void ExecuteKnockback(float knockback)
		{
			foreach (var tAction in TargetActions)
			{
				if (tAction.Is(CombatActionType.None))
					continue;

				var target = tAction.Creature;

				if (target.IsDead)
				{
					tAction.Options |= TargetOptions.FinishingKnockDown;
				}
				else
				{
					// TODO: P/D
					target.KnockBack += knockback;

					if (target.KnockBack > CombatHelper.LimitKnockBack)
						tAction.Options |= (tAction.Has(TargetOptions.Critical) && target.RaceInfo.Is(Data.RaceStands.KnockDownable) ? TargetOptions.KnockDown : TargetOptions.KnockBack);
				}
				if (tAction.IsKnock)
				{
					if (tAction.Creature.RaceInfo.Is(Data.RaceStands.KnockBackable))
					{
						tAction.OldPosition = CombatHelper.KnockBack(target, AttackerAction.Creature);
						this.AttackerAction.Options |= AttackerOptions.KnockBackHit2;
					}
					else
					{
						tAction.Options &= ~(TargetOptions.KnockBack | TargetOptions.KnockDown | TargetOptions.KnockDownFinish);
					}
				}
			}
		}

		public CombatActionPack GetCap()
		{
			var cap = new CombatActionPack(this.AttackerAction.Creature, this.AttackerAction.SkillId);
			cap.Add(this.AttackerAction);
			cap.Add(this.TargetActions.ToArray());

			return cap;
		}

		public enum CriticalOptions
		{
			NoCritical,
			Calculate,
			Critical
		}
	}
}
