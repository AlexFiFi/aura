using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class JeniferScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_jenifer");
		SetRace(10001);
		SetBody(height: 1.1f, fat: 1.1f, upper: 1f, lower: 1.1f);
		SetFace(skin: 17, eye: 4, eyeColor: 119, lip: 1);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3D, 0xB4BDE0, 0xD4E1F3, 0x717172);
		EquipItem(Pocket.Hair, 0xBB9, 0x240C1A, 0x240C1A, 0x240C1A);
		EquipItem(Pocket.Armor, 0x3AAC, 0xF98C84, 0xFBDDD7, 0x351311);
		EquipItem(Pocket.Shoe, 0x4275, 0x0, 0x366961, 0xDAD6EB);

		SetLocation(region: 31, x: 14628, y: 8056);

		SetDirection(26);
		SetStand("human/female/anim/female_natural_stand_npc_lassar");
	}
}
