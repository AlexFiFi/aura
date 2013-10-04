// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.World.World;

namespace Aura.World.Skills
{
	/// <summary>
	/// Simplified handler for skills using Start/Stop, that's automatically
	/// checking for tags, and sends back SkillStart/SkillStop.
	/// </summary>
	public abstract class StartStopTagsSkillHandler : SkillHandler
	{
		public override SkillResults Start(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			// Usually the second parameter is an empty string,
			// but if it's not empty it seems to be tags.
			var parameter = (packet != null && packet.GetElementType() == ElementType.String ? packet.GetString() : null);
			var tags = new MabiTags(parameter);

			var result = this.Start(creature, skill, tags);

			Send.SkillStart(creature, skill.Id, parameter);

			return result;
		}

		public override SkillResults Stop(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			// Sometimes the second stop parameter is a byte,
			// possibly when canceling by moving (eg Rest).
			// The client doesn't really seem to care about what we send back though...
			var parameter = (packet != null && packet.GetElementType() == ElementType.String ? packet.GetString() : null);
			var tags = new MabiTags(parameter);

			var result = this.Stop(creature, skill, tags);

			if (parameter != null)
				Send.SkillStop(creature, skill.Id, parameter);
			else
				Send.SkillStop(creature, skill.Id, 1);

			return result;
		}

		public abstract SkillResults Start(MabiCreature creature, MabiSkill skill, MabiTags tags);
		public abstract SkillResults Stop(MabiCreature creature, MabiSkill skill, MabiTags tags);
	}
}
