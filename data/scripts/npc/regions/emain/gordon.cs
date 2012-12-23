using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class GordonScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_gordon");
		SetRace(10002);
		SetBody(height: 1.3f, fat: 1f, upper: 1.5f, lower: 1.1f);
		SetFace(skin: 21, eye: 9, eyeColor: 154, lip: 9);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1327, 0x979FCF, 0xE2965C, 0xFDDB58);
		EquipItem(Pocket.Hair, 0xFBB, 0x4A2811, 0x4A2811, 0x4A2811);
		EquipItem(Pocket.Armor, 0x3AE5, 0xFAFAFA, 0xF5A929, 0x7D0217);
		EquipItem(Pocket.Shoe, 0x4271, 0x60381E, 0x82CA9C, 0xFF7200);
		EquipItem(Pocket.Head, 0x4687, 0xFFFFFF, 0x657999, 0x717F7B);
		EquipItem(Pocket.RightHand1, 0x9C6C, 0x808080, 0xFFFFFF, 0xFFFFFF);

		SetLocation(region: 52, x: 34890, y: 38980);

		SetDirection(10);
		SetStand("human/male/anim/male_natural_stand_npc_Ranald");
	}
}
