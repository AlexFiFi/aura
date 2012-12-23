using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Hidden_wardens_DScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_hidden_wardens_D");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 22, eye: 9, eyeColor: 8, lip: 2);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0xD4EEED, 0xFCD25D, 0x84002C);
		EquipItem(Pocket.Hair, 0xBE0, 0x864349, 0x864349, 0x864349);
		EquipItem(Pocket.Armor, 0x3E2E, 0xA69D8B, 0x2B2626, 0x6E724E);
		EquipItem(Pocket.RightHand2, 0x9E22, 0x808080, 0x959595, 0x3C4155);
		EquipItem(Pocket.LeftHand2, 0xB3B3, 0x808080, 0x959595, 0x3C4155);

		SetLocation(region: 4014, x: 74100, y: 59900);

		SetDirection(75);
		SetStand("");
	}
}
