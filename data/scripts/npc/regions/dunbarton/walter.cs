using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class WalterScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_walter");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1.2f, upper: 1f, lower: 1.2f);
		SetFace(skin: 22, eye: 13, eyeColor: 27, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1327, 0xC5018A, 0xF79291, 0xFD852E);
		EquipItem(Pocket.Hair, 0xFBB, 0x554433, 0x554433, 0x554433);
		EquipItem(Pocket.Armor, 0x3AC4, 0x665033, 0xDDDDDD, 0xD5DBE4);
		EquipItem(Pocket.Shoe, 0x4271, 0x9D7012, 0xD3E3F4, 0xEEA23D);

		SetLocation(region: 14, x: 35770, y: 39528);

		SetDirection(252);
		SetStand("");
	}
}
