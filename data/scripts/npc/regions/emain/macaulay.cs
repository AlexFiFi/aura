using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MacaulayScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_macaulay");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1.3f, upper: 1f, lower: 1f);
		SetFace(skin: 18, eye: 106, eyeColor: 27, lip: 39);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0xFFF034, 0xB8B6, 0xB3D2EE);
		EquipItem(Pocket.Hair, 0x100E, 0xECEAC8, 0xECEAC8, 0xECEAC8);
		EquipItem(Pocket.Armor, 0x3AC4, 0xC5880B, 0xC3C3C3, 0x808080);
		EquipItem(Pocket.Shoe, 0x4293, 0x292B35, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x47D5, 0xECEAC8, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9E2B, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 52, x: 29067, y: 42350);

		SetDirection(62);
		SetStand("chapter4/human/male/anim/male_c4_npc_dollmaker");
	}
}
