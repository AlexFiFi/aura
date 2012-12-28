using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Sailor_belfast_02Script : Sailor_belfast_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_sailor_belfast_02");
		SetBody(height: 1.3f, fat: 0.9f, upper: 1.2f, lower: 1.1f);
		SetFace(skin: 22, eye: 13, eyeColor: 0, lip: 57);

		EquipItem(Pocket.Face, 0x1324, 0xD0A065, 0xF4CA0A, 0xAB0C6);
		EquipItem(Pocket.Armor, 0x3BF6, 0x98651D, 0x541313, 0x3B2929);
		EquipItem(Pocket.Shoe, 0x42F1, 0x404341, 0x38006B, 0xB7E3C6);
		EquipItem(Pocket.Head, 0x4740, 0x76380F, 0x5D6B76, 0x506270);
		EquipItem(Pocket.RightHand2, 0x9D99, 0x2F373C, 0x333B41, 0x3F6378);

		SetLocation(region: 4005, x: 46790, y: 27560);

		SetDirection(142);
		SetStand("chapter4/human/male/anim/male_c4_npc_pirate02");
	}
}
