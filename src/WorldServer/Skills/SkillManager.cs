// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using Aura.Shared.Const;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.World.World;
using System.Reflection;

namespace Aura.World.Skills
{
	[AttributeUsage(AttributeTargets.Class)]
	public class SkillAttr : Attribute
	{
		public SkillConst[] SkillIds { get; protected set; }

		public SkillAttr(params SkillConst[] skillIds)
		{
			this.SkillIds = skillIds;
		}
	}

	public static class SkillManager
	{
		private static Dictionary<SkillConst, SkillHandler> _handlers = new Dictionary<SkillConst, SkillHandler>();

		public static void Init()
		{
			// Search for classes with a SkillAttr attribute
			var skillTypes =
				from t in Assembly.GetExecutingAssembly().GetTypes()
				let attributes = t.GetCustomAttributes(typeof(SkillAttr), false)
				where attributes != null && attributes.Length > 0
				select new { Type = t, Attributes = attributes.Cast<SkillAttr>() };

			try
			{
				foreach (var type in skillTypes)
				{
					var handler = (SkillHandler)Activator.CreateInstance(type.Type);
					foreach (var skillId in type.Attributes.First().SkillIds)
					{
						if (_handlers.ContainsKey(skillId))
							Logger.Warning("Duplicate handler for '{0}', using '{1}'.", skillId, type.Type.Name);
						_handlers[skillId] = handler;
						//Logger.Info("Found skill handler for '{0}'.", skillId);
					}
				}
			}
			catch (ReflectionTypeLoadException ex)
			{
				Logger.Exception(ex);

				foreach (var e in ex.LoaderExceptions)
					Logger.Exception(e);
			}
		}

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
		public static void CheckOutSkill(MabiCreature creature, SkillConst skillId, out MabiSkill skill, out SkillHandler handler)
		{
			skill = creature.Skills.Get(skillId);
			if (skill == null)
			{
				Logger.Warning("'{0}' tried to use skill '{1}' without having it.", creature.Name, skillId);
			}

			handler = SkillManager.GetHandler(skillId);
			if (handler == null)
			{
				Logger.Unimplemented("Skill handler for '{0}'.", skillId);
				Send.SystemMessage(creature.Client, creature, Localization.Get("aura.unimplemented_skill")); // Partially unimplemented skill.
			}
		}
	}
}
