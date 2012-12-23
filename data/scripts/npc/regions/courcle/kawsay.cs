using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class KawsayScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_kawsay");
		SetRace(10002);
		SetBody(height: 1.15f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 27, eye: 0, eyeColor: 135, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x132F, 0x769177, 0x737171, 0xF6EE61);
		EquipItem(Pocket.Hair, 0xFFD, 0xE7DDD1, 0xE7DDD1, 0xE7DDD1);
		EquipItem(Pocket.Armor, 0x3B88, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x46F7, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand1, 0xABE2, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 3300, x: 252790, y: 189150);

		SetDirection(213);
		SetStand("human/male/anim/male_natural_stand_npc_kawsay");
	}
}
