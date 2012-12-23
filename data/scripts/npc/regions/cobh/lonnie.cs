using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class LonnieScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_lonnie");
		SetRace(124);
		SetBody(height: 0f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 32, eyeColor: 27, lip: 1);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0x367A41, 0xD5DE3D, 0x703000);
		EquipItem(Pocket.Hair, 0xC24, 0x840C18, 0x840C18, 0x840C18);
		EquipItem(Pocket.Armor, 0x3B9E, 0xC3C0BF, 0x748A08, 0x387700);
		EquipItem(Pocket.Shoe, 0x426E, 0x502001, 0x4B6D57, 0x808080);
		EquipItem(Pocket.Head, 0x465C, 0x0, 0xAAF4F7, 0xB5A1F8);
		EquipItem(Pocket.RightHand1, 0x9C42, 0x633C31, 0xB3B6FB, 0x905406);

		SetLocation(region: 23, x: 30827, y: 37182);

		SetDirection(127);
		SetStand("");
	}
}
