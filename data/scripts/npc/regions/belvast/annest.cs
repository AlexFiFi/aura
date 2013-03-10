using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class AnnestScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_annest");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 18, eye: 31, eyeColor: 29, lip: 2);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0x663131, 0x26EAA, 0xAE58);
		EquipItem(Pocket.Hair, 0xBDF, 0x1C2339, 0x1C2339, 0x1C2339);
		EquipItem(Pocket.Armor, 0x3CFE, 0x293965, 0xCACACA, 0x3E3E60);
		EquipItem(Pocket.Shoe, 0x42B4, 0x0, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x466D, 0x0, 0xC6C6C5, 0x1696BE);
		EquipItem(Pocket.LeftHand1, 0x4CD, 0x0, 0x0, 0x0);

		SetLocation(region: 4005, x: 22535, y: 29105);

		SetDirection(4);
		SetStand("chapter4/human/female/anim/female_c4_npc_annest");

		Phrases.Add("Admiral Owen is busy right now. Please speak to me.");
		Phrases.Add("Entrance to Admiral Owen's mansion is restricted.");
		Phrases.Add("Tell me anything you want Admiral Owen to hear.");
		Phrases.Add("Welcome.");
		Phrases.Add("What can I do for you?");
	}
}
