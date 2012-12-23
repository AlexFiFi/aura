using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class SineadScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_sinead");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 104, eyeColor: 27, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF44, 0x990055, 0xC70037, 0xF89E3C);
		EquipItem(Pocket.Hair, 0xC27, 0xAF8754, 0xAF8754, 0xAF8754);
		EquipItem(Pocket.Armor, 0x3C4E, 0x7C5B46, 0x5B0000, 0xCDC5AE);
		EquipItem(Pocket.Shoe, 0x42AF, 0x4A2B31, 0x5B5253, 0x808080);

		SetLocation(region: 415, x: 4796, y: 4790);

		SetDirection(63);
		SetStand("");
	}
}
