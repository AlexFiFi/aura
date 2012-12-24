using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class ArtScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_art");
		SetRace(8002);
		SetBody(height: 1.2f, fat: 1f, upper: 1.2f, lower: 1f);
		SetFace(skin: 23, eye: 149, eyeColor: 54, lip: 52);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x22C4, 0x787592, 0x90B26E, 0xF9B4CE);
		EquipItem(Pocket.Hair, 0x1F63, 0xFFF38C, 0xFFF38C, 0xFFF38C);
		EquipItem(Pocket.Armor, 0x3C09, 0x1C2F42, 0x312A17, 0x593130);
		EquipItem(Pocket.Shoe, 0x4356, 0x1C2F42, 0x444444, 0x737171);
		EquipItem(Pocket.RightHand1, 0x9C58, 0x4E4B46, 0x37727F, 0x808080);

		SetLocation(region: 302, x: 126327, y: 84550);

		SetDirection(64);
		SetStand("");
	}
}
