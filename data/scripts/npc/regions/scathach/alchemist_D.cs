using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Alchemist_DScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_alchemist_D");
		SetRace(9001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 19, eye: 41, eyeColor: 192, lip: 1);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1713, 0x3D3B, 0x725F48, 0x88488);
		EquipItem(Pocket.Hair, 0x138D, 0xFF9FA4, 0xFF9FA4, 0xFF9FA4);
		EquipItem(Pocket.Armor, 0x3C76, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Shoe, 0x4319, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9DBC, 0x808080, 0x959595, 0x3C4155);

		SetLocation(region: 4014, x: 33049, y: 42353);

		SetDirection(126);
		SetStand("");
	}
}
