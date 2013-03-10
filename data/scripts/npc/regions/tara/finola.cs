using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class FinolaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_finola");
		SetRace(10001);
		SetBody(height: 0.6000001f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 29, eyeColor: 30, lip: 2);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0x486769, 0x8B7A89, 0x86752C);
		EquipItem(Pocket.Hair, 0xC20, 0x333333, 0x333333, 0x333333);
		EquipItem(Pocket.Armor, 0x3B0C, 0xC98B4C, 0xAEB398, 0x0);
		EquipItem(Pocket.Shoe, 0x429A, 0x8989AD, 0xFFFFFF, 0x808080);
		EquipItem(Pocket.Head, 0x46EF, 0xC61400, 0x59B1C2, 0x94B2AE);

		SetLocation(region: 434, x: 1631, y: 1174);

		SetDirection(21);
		SetStand("human/female/anim/female_natural_stand_npc_Lassar");

		Phrases.Add("Mana Crystallization looks fun too... Err, I should focus.");
		Phrases.Add("My Potion Making skill will go up faster since I've been making Mana Potions.");
		Phrases.Add("Ugh, there's no end to magic training.");
	}
}
