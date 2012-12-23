using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Wardens_AScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_wardens_A");
		SetRace(8002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 22, eye: 150, eyeColor: 28, lip: 26);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x22C4, 0xFEE75F, 0x21003C, 0x2AB1A1);
		EquipItem(Pocket.Hair, 0x1F83, 0x6F3C3B, 0x6F3C3B, 0x6F3C3B);
		EquipItem(Pocket.Armor, 0x3E2E, 0xA69D8B, 0x2B2626, 0x6E724E);
		EquipItem(Pocket.RightHand2, 0x9E22, 0x808080, 0x959595, 0x3C4155);
		EquipItem(Pocket.LeftHand2, 0xB3B3, 0x808080, 0x959595, 0x3C4155);

		SetLocation(region: 4014, x: 30824, y: 42563);

		SetDirection(69);
		SetStand("giant/anim/giant_sit_01");
	}
}
