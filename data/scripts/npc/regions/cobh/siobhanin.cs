using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class SiobhaninScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_siobhanin");
		SetRace(9002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 41, eyeColor: 168, lip: 1);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1AF4, 0x3348, 0xF8A561, 0xF49D33);
		EquipItem(Pocket.Hair, 0x177A, 0xE3A400, 0xE3A400, 0xE3A400);
		EquipItem(Pocket.Armor, 0x3CD4, 0xE6D5CA, 0x455562, 0x63696E);
		EquipItem(Pocket.Shoe, 0x4325, 0x9EB6BE, 0x8C5A00, 0x95BF49);

		SetLocation(region: 23, x: 26852, y: 36426);

		SetDirection(37);
		SetStand("chapter4/elf/male/anim/elf_npc_siobhanin");
        
		Phrases.Add("Admiral Owen really is quite an amazing person!");
		Phrases.Add("Don't worry about buying something. You can look all you want.");
		Phrases.Add("Ha-ha!");
		Phrases.Add("I'll give you 4 for 30 Gold!");
		Phrases.Add("My back is getting tired from standing here.");
		Phrases.Add("Sigh...");
		Phrases.Add("That one is 50 Gold.");
		Phrases.Add("What brings you here?");
	}
}
