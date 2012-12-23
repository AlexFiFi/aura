using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class GreatHall_maidScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_greatHall_maid");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 99, eyeColor: 162, lip: 2);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0xC7C2DE, 0xF78745, 0x62B164);
		EquipItem(Pocket.Hair, 0x138B, 0x393839, 0x393839, 0x393839);
		EquipItem(Pocket.Armor, 0x3AE6, 0xE1E1E1, 0x383838, 0xB4B4B4);
		EquipItem(Pocket.Glove, 0xEA80, 0xE1E1E1, 0x393839, 0x5C98E9);
		EquipItem(Pocket.Shoe, 0x4467, 0x393839, 0xE1E1E1, 0xE1E1E1);
		EquipItem(Pocket.Head, 0x4700, 0xE1E1E1, 0x669933, 0x808080);
		EquipItem(Pocket.RightHand1, 0xA00A, 0x996600, 0xC6C3C6, 0x46856D);

		SetLocation(region: 413, x: 9863, y: 8564);

		SetDirection(163);
		SetStand("human/male/anim/male_stand_Tarlach_anguish");
	}
}
