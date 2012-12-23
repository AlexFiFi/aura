using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Hidden_wardens_CScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_hidden_wardens_C");
		SetRace(8001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 22, eye: 72, eyeColor: 76, lip: 33);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1EDC, 0x74238F, 0xD4A463, 0x39247A);
		EquipItem(Pocket.Hair, 0x1B5D, 0x401D69, 0x401D69, 0x401D69);
		EquipItem(Pocket.Armor, 0x3E2E, 0xA69D8B, 0x2B2626, 0x6E724E);
		EquipItem(Pocket.RightHand2, 0x9E22, 0x808080, 0x959595, 0x3C4155);
		EquipItem(Pocket.LeftHand2, 0xB3B3, 0x808080, 0x959595, 0x3C4155);

		SetLocation(region: 4014, x: 66900, y: 53800);

		SetDirection(253);
		SetStand("");
	}
}
