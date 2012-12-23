using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Wardens_CScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_wardens_C");
		SetRace(8001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 22, eye: 72, eyeColor: 76, lip: 33);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1EDC, 0x5AC56, 0x562488, 0xF79B62);
		EquipItem(Pocket.Hair, 0x1B64, 0xD01D09, 0xD01D09, 0xD01D09);
		EquipItem(Pocket.Armor, 0x3E2E, 0xA69D8B, 0x2B2626, 0x6E724E);
		EquipItem(Pocket.RightHand2, 0x9E22, 0x808080, 0x959595, 0x3C4155);
		EquipItem(Pocket.LeftHand2, 0xB3B3, 0x808080, 0x959595, 0x3C4155);

		SetLocation(region: 4014, x: 33561, y: 42889);

		SetDirection(101);
		SetStand("chapter3/giant/female/anim/giant_female_c3_npc_karpfen");
	}
}
