using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Sailor_belfast_01Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_sailor_belfast_01");
		SetRace(10002);
		SetBody(height: 1.3f, fat: 1.3f, upper: 1.3f, lower: 1.3f);
		SetFace(skin: 22, eye: 6, eyeColor: 0, lip: 1);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0xF29B37, 0xD6D6EB, 0xFFE9B6);
		EquipItem(Pocket.Hair, 0x1779, 0x1B1D28, 0x1B1D28, 0x1B1D28);
		EquipItem(Pocket.Armor, 0x3BFC, 0x933C8D, 0x3D1F10, 0x51920);
		EquipItem(Pocket.Shoe, 0x42F6, 0x855176, 0xA5B2A7, 0x399FF2);
		EquipItem(Pocket.Head, 0x4740, 0x855176, 0x6A6A6A, 0x399FF2);
		EquipItem(Pocket.RightHand1, 0x9E16, 0x0, 0x0, 0x0);
		EquipItem(Pocket.RightHand2, 0x9C4A, 0xBFBBBA, 0x9F6E4D, 0x7B6B5E);

		SetLocation(region: 4005, x: 46670, y: 27420);

		SetDirection(64);
		SetStand("chapter4/human/male/anim/male_c4_npc_pirate01");
	}
}
