using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class GornevesScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_gorneves");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 0.1f, upper: 1f, lower: 0.85f);
		SetFace(skin: 17, eye: 3, eyeColor: 39, lip: 2);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0xA5BD, 0x187C8, 0xEFAF4C);
		EquipItem(Pocket.Hair, 0xBE6, 0xFFCAAE7B, 0xFFCAAE7B, 0xFFCAAE7B);
		EquipItem(Pocket.Armor, 0x32CE, 0x785C3E, 0x633C31, 0x808080);
		EquipItem(Pocket.Glove, 0x4076, 0x785C3E, 0x633C31, 0x808080);
		EquipItem(Pocket.Shoe, 0x4475, 0x785C3E, 0x633C31, 0x808080);
		EquipItem(Pocket.Head, 0x4848, 0x785C3E, 0x633C31, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9D60, 0x5F5F5F, 0x808080, 0x808080);

		SetLocation(region: 428, x: 67256, y: 108111);

		SetDirection(43);
		SetStand("");

		Phrases.Add("...Hehe.");
		Phrases.Add("Don't get nervous.");
		Phrases.Add("You're lacking energy.");
	}
}
