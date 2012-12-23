using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class HagelScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_hagel");
		SetRace(9002);
		SetBody(height: 1.3f, fat: 1f, upper: 1.2f, lower: 1f);
		SetFace(skin: 20, eye: 38, eyeColor: 27, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1AF4, 0x550000, 0x36566E, 0x9075B6);
		EquipItem(Pocket.Hair, 0xFC6, 0xE8EFDC, 0xE8EFDC, 0xE8EFDC);
		EquipItem(Pocket.Armor, 0x36C9, 0xA39984, 0x24280D, 0x143422);
		EquipItem(Pocket.Glove, 0x408F, 0x4C5A35, 0x25302B, 0x808080);
		EquipItem(Pocket.Shoe, 0x4469, 0x222A30, 0x4D5475, 0xA0A686);

		SetLocation(region: 3100, x: 366322, y: 425294);

		SetDirection(218);
		SetStand("elf/male/anim/elf_npc_hagel_stand_friendly");
	}
}
