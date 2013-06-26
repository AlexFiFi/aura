// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Network;
using Aura.World.World;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.Data;
using Aura.Shared.Const;

namespace Aura.World.Skills
{
	public class DyeHandler : SkillHandler
	{
		public override SkillResults Prepare(World.MabiCreature creature, MabiSkill skill, MabiPacket packet, uint castTime)
		{
			var itemId = packet.GetLong();
			var dyeId = packet.GetLong();

			var item = creature.GetItem(itemId);
			var dye = creature.GetItem(dyeId);
			if (item == null || dye == null)
				return SkillResults.Failure;

			creature.Temp.SkillItem1 = item;
			creature.Temp.SkillItem2 = dye;

			Send.SkillReady(creature.Client, creature, skill.Id, itemId, dyeId);

			return SkillResults.Okay;
		}

		public override SkillResults Use(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			if (creature.Temp.SkillItem1 == null || creature.Temp.SkillItem2 == null)
				return SkillResults.Failure;

			var part = packet.GetInt();

			// Regular
			if (packet.GetElementType() == ElementType.Short)
			{
				var x = packet.GetShort();
				var y = packet.GetShort();

				Send.SkillUseDye(creature.Client, creature, skill.Id, part, x, y);
			}
			// Fixed
			else if (packet.GetElementType() == ElementType.Byte)
			{
				var unk = packet.GetByte();

				Send.SkillUseDye(creature.Client, creature, skill.Id, part, unk);
			}

			return SkillResults.Okay;
		}

		public override SkillResults Complete(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			if (creature.Temp.SkillItem1 == null || creature.Temp.SkillItem2 == null)
				return SkillResults.Failure;

			var part = packet.GetInt();

			if (packet.GetElementType() == ElementType.Short)
				return this.CompleteRegular(creature, packet, skill.Id, part);
			else if (packet.GetElementType() == ElementType.Byte)
				return this.CompleteFixed(creature, packet, skill.Id, part);

			return SkillResults.Failure;
		}

		public SkillResults CompleteRegular(MabiCreature creature, MabiPacket packet, SkillConst skillId, uint part)
		{
			var x = packet.GetShort();
			var y = packet.GetShort();

			// Map for the selected slot
			byte map = creature.Temp.SkillItem1.DataInfo.ColorMap1;
			if (part == 1)
				map = creature.Temp.SkillItem1.DataInfo.ColorMap2;
			else if (part == 2)
				map = creature.Temp.SkillItem1.DataInfo.ColorMap3;

			// Imaginary index for the map
			switch (map)
			{
				default:
				case 1: map = 0xE0; break;  // Cloth
				case 2: map = 0xE1; break;  // Leather
				case 5: map = 0xE2; break;  // Metal
				case 23: map = 0xE3; break; // Weapon
				case 24: map = 0xE4; break; // Hilt
			}

			// Random color from the available ones
			var rndNr = (byte)RandomProvider.Get().Next(5);
			x = (ushort)((x + creature.Temp.DyeCursors[rndNr * 4 + 0] - creature.Temp.DyeCursors[rndNr * 4 + 1]) & 0xFF);
			y = (ushort)((y + creature.Temp.DyeCursors[rndNr * 4 + 2] - creature.Temp.DyeCursors[rndNr * 4 + 3]) & 0xFF);
			rndNr += 1;

			// Change color
			var color = MabiData.ColorMapDb.GetAt(map, x -= 1, y -= 1);
			switch (part)
			{
				default:
				case 0: creature.Temp.SkillItem1.Info.ColorA = color; break;
				case 1: creature.Temp.SkillItem1.Info.ColorB = color; break;
				case 2: creature.Temp.SkillItem1.Info.ColorC = color; break;
			}

			this.DyeSuccess(creature);

			Send.AcquireDyedItem(creature.Client, creature, creature.Temp.SkillItem1.Id, rndNr);
			Send.DyeSkillComplete(creature.Client, creature, skillId, part, x, y);

			return SkillResults.Okay;
		}

		public SkillResults CompleteFixed(MabiCreature creature, MabiPacket packet, SkillConst skillId, uint part)
		{
			// Older logs seem to have an additional byte (like Use?)
			//var unk = packet.GetByte();

			switch (part)
			{
				default:
				case 0: creature.Temp.SkillItem1.Info.ColorA = creature.Temp.SkillItem2.Info.ColorA; break;
				case 1: creature.Temp.SkillItem1.Info.ColorB = creature.Temp.SkillItem2.Info.ColorA; break;
				case 2: creature.Temp.SkillItem1.Info.ColorC = creature.Temp.SkillItem2.Info.ColorA; break;
			}

			this.DyeSuccess(creature);

			Send.AcquireDyedItem(creature.Client, creature, creature.Temp.SkillItem1.Id);
			Send.DyeSkillComplete(creature.Client, creature, skillId, part);

			return SkillResults.Okay;
		}

		/// <summary>
		/// Sends success effect, deletes dye, and updates item color.
		/// </summary>
		/// <param name="creature"></param>
		public void DyeSuccess(MabiCreature creature)
		{
			// delete dye

			// Success effect and aquire box
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(2).PutByte(4), SendTargets.Range, creature);
			Send.ItemUpdate(creature.Client, creature, creature.Temp.SkillItem1);

			// Update equipped item color
			if (creature.Temp.SkillItem1.IsEquipped())
				creature.ItemUpdate(creature.Temp.SkillItem1);
		}
	}
}
