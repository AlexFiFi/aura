using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class BryceScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_bryce");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1f, upper: 1.5f, lower: 1f);
		SetFace(skin: 20, eye: 5, eyeColor: 76, lip: 12);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1326, 0x155782, 0x84B274, 0xFBC25C);
		EquipItem(Pocket.Hair, 0xFBB, 0x5B482B, 0x5B482B, 0x5B482B);
		EquipItem(Pocket.Armor, 0x3ABA, 0xFAF7EB, 0x3C2D22, 0x100C0A);
		EquipItem(Pocket.Shoe, 0x4271, 0x0, 0xF69A2B, 0x4B676F);

		SetLocation(region: 31, x: 11365, y: 9372);

		SetDirection(0);
		SetStand("human/male/anim/male_natural_stand_npc_bryce");
	}
}
