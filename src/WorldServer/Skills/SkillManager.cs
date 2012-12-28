// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;
using Common.Constants;
using Common.Tools;
using Common.World;
using World.Network;

namespace World.Skills
{
	public static class SkillManager
	{
		static SkillManager()
		{
			// Combat
			_handlers.Add(SkillConst.MeleeCombatMastery, new CombatMasteryHandler());
			_handlers.Add(SkillConst.Smash, new SmashHandler());
			_handlers.Add(SkillConst.Defense, new DefenseHandler());
			_handlers.Add(SkillConst.Windmill, new WindmillHandler());
			//_handlers.Add(SkillConst.MeleeCounterattack, new CounterHandler());
			_handlers.Add(SkillConst.ShadowBunshin, new ShadowBunshinHandler());

			// Life
			_handlers.Add(SkillConst.Rest, new RestHandler());

			// Magic
			_handlers.Add(SkillConst.ManaShield, new ManaShieldHandler());
			_handlers.Add(SkillConst.Healing, new HealingHandler());

			// Action
			_handlers.Add(SkillConst.UseUmbrella, new UmbrellaSkillHandler());

			// Hidden
			_handlers.Add(SkillConst.HiddenResurrection, new HiddenResurrectionHandler());

			// Transformation
			_handlers.Add(SkillConst.SpiritOfOrder, new SpiritOfOrderHandler());
			_handlers.Add(SkillConst.SoulOfChaos, new SoulOfChaosHandler());
			_handlers.Add(SkillConst.FuryOfConnous, new FuryOfConnousHandler());

			// GM
			_handlers.Add(SkillConst.SuperWindmill, new SuperWindmillHandler());
		}

		private static Dictionary<SkillConst, SkillHandler> _handlers = new Dictionary<SkillConst, SkillHandler>();

		public static SkillHandler GetHandler(SkillConst skill)
		{
			return (_handlers.ContainsKey(skill) ? _handlers[skill] : null);
		}

		public static SkillHandler GetHandler(ushort skillId)
		{
			return GetHandler((SkillConst)skillId);
		}

		/// <summary>
		/// Tries to get MabiSkill and SkillHandler based on creature
		/// and skillId. Prints warnings automatically, and returns both
		/// values (or null) via out parameter.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="skillId"></param>
		/// <param name="skill"></param>
		/// <param name="handler"></param>
		public static void CheckOutSkill(MabiCreature creature, ushort skillId, out MabiSkill skill, out SkillHandler handler)
		{
			skill = creature.GetSkill(skillId);
			if (skill == null)
			{
				Logger.Warning("'" + creature.Name + "' tried to use skill '" + skillId.ToString() + "' without having it.");
				creature.Client.Send(PacketCreator.SystemMessage(creature, "Partially unimplemented skill."));
			}

			handler = SkillManager.GetHandler(skillId);
			if (handler == null)
			{
				Logger.Unimplemented("Skill handler for '" + skillId.ToString() + "'.");
				creature.Client.Send(PacketCreator.SystemMessage(creature, "Unimplemented skill."));
			}
		}
	}
}
