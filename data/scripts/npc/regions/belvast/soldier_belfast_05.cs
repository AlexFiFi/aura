using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Soldier_belfast_05Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_soldier_belfast_05");
		SetRace(10001);
		SetBody(height: 0.8500001f, fat: 0.95f, upper: 1.1f, lower: 0.9f);
		SetFace(skin: 19, eye: 6, eyeColor: 76, lip: 1);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0x24B90, 0xC9003B, 0xFBF27C);
		EquipItem(Pocket.Hair, 0xFB9, 0x4F2727, 0x4F2727, 0x4F2727);
		EquipItem(Pocket.Armor, 0x36C4, 0x828182, 0xD2D46, 0x6F605E);
		EquipItem(Pocket.Glove, 0x3EC0, 0x828182, 0x6F605E, 0x399FF2);
		EquipItem(Pocket.Shoe, 0x4337, 0x828182, 0x6F605E, 0x66685A);
		EquipItem(Pocket.Head, 0x485D, 0x828182, 0x846707, 0x579247);
		EquipItem(Pocket.RightHand1, 0x9D32, 0x434343, 0x9F9F9F, 0xBFBFBF);
		EquipItem(Pocket.RightHand2, 0x9C91, 0x546C74, 0x274376, 0x2537C6);
		EquipItem(Pocket.LeftHand1, 0xB3B3, 0x464646, 0x37727F, 0x4E1EF4);

		SetLocation(region: 4005, x: 27304, y: 27369);

		SetDirection(0);
		SetStand("");
	}
}
