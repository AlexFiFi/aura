// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;
using Aura.Shared.Const;
using Aura.Shared.Util;
using Aura.World.World;
using Aura.World.Network;

namespace Aura.World.Skills
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
			_handlers.Add(SkillConst.MeleeCounterattack, new CounterHandler());
			_handlers.Add(SkillConst.ShadowBunshin, new ShadowBunshinHandler());
			_handlers.Add(SkillConst.RangedCombatMastery, new RangeCombatMasteryHandler());
			_handlers.Add(SkillConst.ArrowRevolver, new ArrowRevolverHandler());
			_handlers.Add(SkillConst.ArrowRevolver2, new ArrowRevolverHandler());
			_handlers.Add(SkillConst.MagnumShot, new MagnumShotHandler());
			_handlers.Add(SkillConst.SupportShot, new SupportShotHandler());

			// Life
			_handlers.Add(SkillConst.Rest, new RestHandler());
			_handlers.Add(SkillConst.Composing, new ComposingHandler());
			_handlers.Add(SkillConst.PlayingInstrument, new PlayingInstrumentHandler());

			// Magic
			_handlers.Add(SkillConst.ManaShield, new ManaShieldHandler());
			_handlers.Add(SkillConst.Healing, new HealingHandler());
			_handlers.Add(SkillConst.Icebolt, new IceboltHandler());
			_handlers.Add(SkillConst.Firebolt, new FireboltHandler());
			_handlers.Add(SkillConst.Lightningbolt, new LightningboltHandler());
			_handlers.Add(SkillConst.Thunder, new ThunderHandler());
			_handlers.Add(SkillConst.IceSpear, new IcespearHandler());
			_handlers.Add(SkillConst.Fireball, new FireballHandler());

			// Action
			_handlers.Add(SkillConst.Umbrella, new UmbrellaSkillHandler());

			// Hidden
			_handlers.Add(SkillConst.HiddenResurrection, new HiddenResurrectionHandler());
			_handlers.Add(SkillConst.Dye, new DyeHandler());

			// Transformation
			_handlers.Add(SkillConst.SpiritOfOrder, new SpiritOfOrderHandler());
			_handlers.Add(SkillConst.SoulOfChaos, new SoulOfChaosHandler());
			_handlers.Add(SkillConst.FuryOfConnous, new FuryOfConnousHandler());

			// GM
			_handlers.Add(SkillConst.SuperWindmillGMSkill, new SuperWindmillHandler());
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
				Logger.Warning("'{0}' tried to use skill '{1}' without having it.", creature.Name, skillId);
			}

			handler = SkillManager.GetHandler(skillId);
			if (handler == null)
			{
				Logger.Unimplemented("Skill handler for '{0}'.", skillId);
				creature.Client.Send(PacketCreator.SystemMessage(creature, Localization.Get("aura.unimplemented_skill"))); // Partially unimplemented skill.
			}
		}
	}
}
