// Aura Script
// --------------------------------------------------------------------------
// Sailors
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class _SailorBelfastBaseScript : NPCScript
{
	public override void OnLoad()
	{
		SetRace(10002);

		EquipItem(Pocket.Hair, 6009, 0x1B1D28, 0x1B1D28, 0x1B1D28);
		EquipItem(Pocket.RightHand1, 40470, 0x0, 0x0, 0x0);

		Phrases.Add("Cheers for the next journey!");
		Phrases.Add("Cheers!");
		Phrases.Add("Don't mess with Barry...you'll be sorry!");
		Phrases.Add("It's on me tonight!");
		Phrases.Add("Kya! What a taste!");
		Phrases.Add("When you go to Barry's pub, you MUST check out his seafood dishes.");
		Phrases.Add("Where will we go next...?");
		Phrases.Add("Whew, I needed that.");
		Phrases.Add("Whoa, I feel great now.");
	}
}

public class SailorBelfast1Script : _SailorBelfastBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_sailor_belfast_01");
		SetBody(height: 1.3f, fat: 1.3f, upper: 1.3f, lower: 1.3f);
		SetFace(skin: 22, eye: 6, eyeColor: 0, lip: 1);
		SetStand("chapter4/human/male/anim/male_c4_npc_pirate01");
		SetLocation(4005, 46670, 27420, 64);

		EquipItem(Pocket.Face, 4900, 0xF29B37, 0xD6D6EB, 0xFFE9B6);
		EquipItem(Pocket.Armor, 15356, 0x933C8D, 0x3D1F10, 0x51920);
		EquipItem(Pocket.Shoe, 17142, 0x855176, 0xA5B2A7, 0x399FF2);
		EquipItem(Pocket.Head, 18240, 0x855176, 0x6A6A6A, 0x399FF2);
		EquipItem(Pocket.RightHand2, 40010, 0xBFBBBA, 0x9F6E4D, 0x7B6B5E);
	}
}

public class SailorBelfast2Script : _SailorBelfastBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_sailor_belfast_02");
		SetBody(height: 1.3f, fat: 0.9f, upper: 1.2f, lower: 1.1f);
		SetFace(skin: 22, eye: 13, eyeColor: 0, lip: 57);
		SetStand("chapter4/human/male/anim/male_c4_npc_pirate02");
		SetLocation(4005, 46790, 27560, 142);

		EquipItem(Pocket.Face, 4900, 0xD0A065, 0xF4CA0A, 0xAB0C6);
		EquipItem(Pocket.Armor, 15350, 0x98651D, 0x541313, 0x3B2929);
		EquipItem(Pocket.Shoe, 17137, 0x404341, 0x38006B, 0xB7E3C6);
		EquipItem(Pocket.Head, 18240, 0x76380F, 0x5D6B76, 0x506270);
		EquipItem(Pocket.RightHand2, 40345, 0x2F373C, 0x333B41, 0x3F6378);
	}
}

public class SailorBelfast3Script : _SailorBelfastBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_sailor_belfast_03");
		SetBody(height: 1.3f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 22, eye: 6, eyeColor: 0, lip: 1);
		SetStand("chapter4/human/male/anim/male_c4_npc_pirate03");
		SetLocation(4005, 46620, 27575, 230);

		EquipItem(Pocket.Face, 4900, 0xAE368F, 0x94F, 0xBBB15C);
		EquipItem(Pocket.Armor, 15356, 0x713E52, 0x171F1A, 0x755454);
		EquipItem(Pocket.Shoe, 17141, 0x535B41, 0xC199F2, 0xE2F4EB);
		EquipItem(Pocket.Head, 18241, 0x7F4F45, 0x669098, 0x4F0045);
		EquipItem(Pocket.RightHand2, 40345, 0x5C5C5C, 0x868687, 0x6C4900);
	}
}
