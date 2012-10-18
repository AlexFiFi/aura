using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.World;
using Common.Events;
using World.Network;
using Common.Constants;

namespace World.World
{
	[Flags]
	public enum SkillResult
	{
		None = 0,
		Okay = 1 << 1,
		AttackStunned = 1 << 2,
		AttackOutOfRange = 1 << 3,
		InsufficientStats = 1 << 4,
		SuppressFlash = 1 << 5,
	}

	/* Single use skills
	 * Handlers called in the following order:
	 * Skill Prepare
	 * Skill Ready
	 * Skill Use
	 * Skill Complete
	 */
	public delegate SkillResult SkillPrepareHandler(MabiCreature creature, MabiSkill skill, string parameters);
	public delegate SkillResult SkillReadyHandler(MabiCreature creature, MabiSkill skill, string parameters);
	public delegate SkillResult SkillUseHandler(MabiCreature creature, MabiEntity target, SkillAction action, MabiSkill skill, uint var1, uint var2);
	public delegate SkillResult SkillCompletedHandler(MabiCreature creature,  MabiSkill skill, string parameters);

	/*
	 * Contiginous skills
	 */
	public delegate SkillResult SkillStartHandler(MabiCreature creature, MabiSkill skill, string parameters);
	public delegate SkillResult SkillStopHandler(MabiCreature creature, MabiSkill skill, string parameters);

	public static partial class Skills
	{
		private static Dictionary<SkillConst, SkillPrepareHandler> SkillPrepareHandlers = new Dictionary<SkillConst,SkillPrepareHandler>()
		{
			{SkillConst.Healing, new SkillPrepareHandler(HealingPrepare) },
			{SkillConst.HiddenResurrection, new SkillPrepareHandler(HiddenResurrectionPrepare)},
		};
		private static Dictionary<SkillConst, SkillReadyHandler> SkillReadyHandlers = new Dictionary<SkillConst,SkillReadyHandler>()
		{
			{SkillConst.Healing, new SkillReadyHandler(HealingReady)},
			{SkillConst.FinalHit, new SkillReadyHandler(FinalHitReady)},
		};
		private static Dictionary<SkillConst, SkillUseHandler> SkillUseHandlers = new Dictionary<SkillConst, SkillUseHandler>()
		{
			{SkillConst.MeleeCombatMastery, new SkillUseHandler(CombatMasteryUse)},
			{SkillConst.Smash, new SkillUseHandler(SmashUse)},
			{SkillConst.Windmill, new SkillUseHandler(WindmillUse)},
			{SkillConst.Healing, new SkillUseHandler(HealingUsed)},
			{SkillConst.HiddenResurrection, new SkillUseHandler(HiddenResurrectionUse)},
		};
		private static Dictionary<SkillConst, SkillCompletedHandler> SkillCompletedHandlers = new Dictionary<SkillConst, SkillCompletedHandler>()
		{
			{SkillConst.HiddenResurrection, new SkillCompletedHandler(HiddenResurectionCompleted)},
		};
		private static Dictionary<SkillConst, SkillStartHandler> SkillStartHandlers = new Dictionary<SkillConst, SkillStartHandler>()
		{
			{SkillConst.ManaShield, new SkillStartHandler(ManaShieldStart)},
			{SkillConst.Rest, new SkillStartHandler(RestStart)},
		};
		private static Dictionary<SkillConst, SkillStopHandler> SkillStopHandlers = new Dictionary<SkillConst, SkillStopHandler>()
		{
			//{SkillConst.ManaShield, new SkillStopHandler(ManaShieldStop)},
			{SkillConst.Rest, new SkillStopHandler(RestStop)},
		};

		public static SkillPrepareHandler GetSkillPrepareHandler(SkillConst id)
		{
			if (SkillPrepareHandlers.ContainsKey(id))
				return SkillPrepareHandlers[id];
			return null;
		}
		public static SkillReadyHandler GetSkillReadyHandler(SkillConst id)
		{
			if (SkillReadyHandlers.ContainsKey(id))
				return SkillReadyHandlers[id];
			return null;
		}
		public static SkillUseHandler GetSkillUsedHandler(SkillConst id)
		{
			if (SkillUseHandlers.ContainsKey(id))
				return SkillUseHandlers[id];
			return null;
		}
		public static SkillCompletedHandler GetSkillCompletedHandler(SkillConst id)
		{
			if (SkillCompletedHandlers.ContainsKey(id))
				return SkillCompletedHandlers[id];
			return null;
		}
		public static SkillStartHandler GetSkillStartHandler(SkillConst id)
		{
			if (SkillStartHandlers.ContainsKey(id))
				return SkillStartHandlers[id];
			return null;
		}
		public static SkillStopHandler GetSkillStopHandler(SkillConst id)
		{
			if (SkillStopHandlers.ContainsKey(id))
				return SkillStopHandlers[id];
			return null;
		}


		public static uint ActionId = 1;
		public static SkillResult CheckMP(MabiCreature creature, float amount, bool message = true, bool take = true)
		{
			if (creature.Mana < amount)
			{
				if (message)
					creature.Client.Send(PacketCreator.Notice("Insufficient MP"));
				return SkillResult.InsufficientStats;
			}
			if (take)
			{
				creature.Mana -= amount;
				WorldManager.Instance.CreatureStatsUpdate(creature);
			}
			return SkillResult.Okay;
		}
        public static SkillResult CheckMP(MabiCreature creature, MabiSkill skill, bool message = true, bool take = true)
		{
			return CheckMP(creature, skill.RankInfo.ManaCost, message, take);
		}
		public static SkillResult CheckSP(MabiCreature creature, float amount, bool message = true, bool take = true)
		{
			if (creature.Stamina < amount)
			{
				if (message)
					creature.Client.Send(PacketCreator.Notice("Unable to use the skill. Insufficient Stamina."));
				return SkillResult.InsufficientStats;
			}
			if (take)
			{
				creature.Stamina -= amount;
				WorldManager.Instance.CreatureStatsUpdate(creature);
			}
			return SkillResult.Okay;
		}
		public static SkillResult CheckSP(MabiCreature creature, MabiSkill skill, bool message = true, bool take = true)
		{
			return CheckSP(creature, skill.RankInfo.StaminaCost, message, take);
		}
	}
}
