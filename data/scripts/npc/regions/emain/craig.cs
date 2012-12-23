using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class CraigScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_craig");
		SetRace(10002);
		SetBody(height: 1.29f, fat: 1f, upper: 1.2f, lower: 1f);
		SetFace(skin: 20, eye: 9, eyeColor: 125, lip: 12);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1326, 0x769083, 0xCDD827, 0xF34538);
		EquipItem(Pocket.Hair, 0xFBE, 0x453012, 0x453012, 0x453012);
		EquipItem(Pocket.Armor, 0x32E0, 0x91755B, 0x463424, 0x36241B);
		EquipItem(Pocket.RightHand2, 0x9C61, 0xB7B6B8, 0xC48246, 0x9AAFA2);

		SetLocation(region: 52, x: 25168, y: 59202);

		SetDirection(125);
		SetStand("monster/anim/ghostarmor/Tequip_C/ghostarmor_Tequip_C01_stand_friendly");
	}
}
