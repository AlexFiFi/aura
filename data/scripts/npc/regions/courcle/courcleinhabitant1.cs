using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Courcleinhabitant1Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_courcleinhabitant1");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 25, eye: 4, eyeColor: 27, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0xD7AA51, 0x960053, 0x684200);
		EquipItem(Pocket.Hair, 0xFF5, 0x3A1E03, 0x3A1E03, 0x3A1E03);
		EquipItem(Pocket.Armor, 0x3B99, 0x202324, 0x8189A8, 0x192A3C);

		SetLocation(region: 3300, x: 254858, y: 186245);

		SetDirection(72);
		SetStand("");
	}
}
