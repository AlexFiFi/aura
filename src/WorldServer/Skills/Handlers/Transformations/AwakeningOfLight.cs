// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Shared.Const;
using Aura.World.World;
using Aura.Shared.Network;
using Aura.World.Network;
using Aura.Shared.Util;

namespace Aura.World.Skills.Handlers.Transformations
{
	/// <summary>
	/// Var1: Duration
	/// Var2: Cooldown
	/// Var3/4/5: HP/MP/STM recovery
	/// Var6: ? (1500-1900)
	/// Var7: Arat Berry Radius (5k-8k)
	/// Var8: Stun Negated (%) (20-40) 
	/// Var9: Skill exp used? (600 (/1000))
	/// </summary>
	[SkillAttr(SkillConst.AwakeningofLight)]
	public class AwakeningOfLightHandler : StartStopSkillHandler
	{
		private const uint EruptionRadius = 500;
		private const uint EruptionDamage = 120;
		private const ushort EruptionStun = 5000;
		private const int EruptionKnockBack = 375;

		public override SkillResults Start(MabiCreature creature, MabiSkill skill)
		{
			creature.Activate(CreatureConditionB.Demigod);

			creature.StopMove();

			// Spawn eruption
			{
				var pos = creature.GetPosition();
				var targets = WorldManager.Instance.GetAttackableCreaturesInRange(creature, EruptionRadius);

				var cap = new CombatActionPack(creature, skill.Id);
				var aAction = new AttackerAction(CombatActionType.SpecialHit, creature, skill.Id, SkillHelper.GetAreaTargetID(creature.Region, pos.X, pos.Y));
				aAction.Options |= AttackerOptions.KnockBackHit1 | AttackerOptions.UseEffect;

				cap.Add(aAction);

				foreach (var target in targets)
				{
					target.StopMove();

					// Officials use CM skill id.
					var tAction = new TargetAction(CombatActionType.TakeHit, target, creature, skill.Id);
					tAction.StunTime = EruptionStun;
					tAction.Delay = 1000;

					// Supposedly it's magic damage
					tAction.Damage = creature.GetMagicDamage(null, EruptionDamage);

					target.TakeDamage(tAction.Damage);
					tAction.OldPosition = CombatHelper.KnockBack(target, creature, EruptionKnockBack);
					if (target.IsDead)
						tAction.Options |= TargetOptions.FinishingKnockDown;

					cap.Add(tAction);
				}

				WorldManager.Instance.HandleCombatActionPack(cap);
			}

			Send.EffectDelayed(Effect.AwakeningOfLight1, 800, creature);
			Send.EffectDelayed(Effect.AwakeningOfLight2, 800, creature);
			Send.UseMotion(creature, 67, 3, false, false);

			creature.StatRegens.Add(creature.Temp.DemiHpRegen = new MabiStatRegen(Stat.Life, skill.RankInfo.Var3, creature.LifeMax));
			creature.StatRegens.Add(creature.Temp.DemiMpRegen = new MabiStatRegen(Stat.Mana, skill.RankInfo.Var4, creature.ManaMax));
			creature.StatRegens.Add(creature.Temp.DemiStmRegen = new MabiStatRegen(Stat.Stamina, skill.RankInfo.Var5, creature.StaminaMax));
			WorldManager.Instance.CreatureStatsUpdate(creature);

			return SkillResults.Okay;
		}

		public override SkillResults Stop(MabiCreature creature, MabiSkill skill)
		{
			creature.Deactivate(CreatureConditionB.Demigod);

			creature.StatRegens.Remove(creature.Temp.DemiHpRegen);
			creature.StatRegens.Remove(creature.Temp.DemiMpRegen);
			creature.StatRegens.Remove(creature.Temp.DemiStmRegen);

			WorldManager.Instance.Broadcast(PacketCreator.StatRegenStop(creature, StatUpdateType.Public, creature.Temp.DemiHpRegen, creature.Temp.DemiMpRegen, creature.Temp.DemiStmRegen), SendTargets.Range, creature);
			WorldManager.Instance.Broadcast(PacketCreator.StatRegenStop(creature, StatUpdateType.Private, creature.Temp.DemiHpRegen, creature.Temp.DemiMpRegen, creature.Temp.DemiStmRegen), SendTargets.Range, creature);
			WorldManager.Instance.CreatureStatsUpdate(creature);

			return SkillResults.Okay;
		}
	}
}
