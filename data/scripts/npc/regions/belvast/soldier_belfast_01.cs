using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Soldier_belfast_01Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_soldier_belfast_01");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 19, eye: 31, eyeColor: 134, lip: 2);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0xB6F9D, 0xA60F46, 0x9DDB);
		EquipItem(Pocket.Hair, 0xFB9, 0x4F2727, 0x4F2727, 0x4F2727);
		EquipItem(Pocket.Armor, 0x36BF, 0x15151A, 0x6F605E, 0xD2D46);
		EquipItem(Pocket.Glove, 0x3EC0, 0x1C2024, 0x424B, 0x399FF2);
		EquipItem(Pocket.Shoe, 0x4337, 0x2B2C35, 0x62404D, 0x66685A);
		EquipItem(Pocket.Head, 0x485D, 0x524F5D, 0x846707, 0x579247);
		EquipItem(Pocket.RightHand1, 0x9D32, 0x434343, 0x9F9F9F, 0xBFBFBF);
		EquipItem(Pocket.RightHand2, 0x9C91, 0x546C74, 0x274376, 0x2537C6);
		EquipItem(Pocket.LeftHand1, 0xB3B3, 0x464646, 0x37727F, 0x4E1EF4);
		EquipItem(Pocket.LeftHand2, 0xAFC9, 0x546C74, 0x274376, 0x2537C6);

		SetLocation(region: 4005, x: 23484, y: 48333);

		SetDirection(224);
		SetStand("");
	}
}
