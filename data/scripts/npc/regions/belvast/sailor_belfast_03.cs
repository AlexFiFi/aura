using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Sailor_belfast_03Script : Sailor_belfast_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_sailor_belfast_03");
		SetBody(height: 1.3f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 22, eye: 6, eyeColor: 0, lip: 1);

		EquipItem(Pocket.Face, 0x1324, 0xAE368F, 0x94F, 0xBBB15C);
		EquipItem(Pocket.Armor, 0x3BFC, 0x713E52, 0x171F1A, 0x755454);
		EquipItem(Pocket.Shoe, 0x42F5, 0x535B41, 0xC199F2, 0xE2F4EB);
		EquipItem(Pocket.Head, 0x4741, 0x7F4F45, 0x669098, 0x4F0045);
		EquipItem(Pocket.RightHand2, 0x9D99, 0x5C5C5C, 0x868687, 0x6C4900);

		SetLocation(region: 4005, x: 46620, y: 27575);

		SetDirection(230);
		SetStand("chapter4/human/male/anim/male_c4_npc_pirate03");
	}
}
