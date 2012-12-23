using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class AugustineScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_augustine");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 61, eyeColor: 126, lip: 48);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0xDBE7F7, 0xFAB243, 0x5E5064);
		EquipItem(Pocket.Hair, 0xFB9, 0x393839, 0x393839, 0x393839);
		EquipItem(Pocket.Armor, 0x3C13, 0x75CAE4, 0x516181, 0xACC6FE);
		EquipItem(Pocket.Shoe, 0x4301, 0x4B5AA7, 0x8B99B5, 0x424563);

		SetLocation(region: 23, x: 27630, y: 41361);

		SetDirection(245);
		SetStand("chapter4/human/male/anim/male_c4_npc_augustine");
	}
}
