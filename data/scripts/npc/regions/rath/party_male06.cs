using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Party_male06Script : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_party_male06");
		SetRace(10002);
		SetBody(height: 1.2f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 12, eyeColor: 76, lip: 75);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0xF68734, 0x47278B, 0x4A1973);
		EquipItem(Pocket.Hair, 0x135B, 0x0, 0x0, 0x0);
		EquipItem(Pocket.Armor, 0x3B24, 0x1B1919, 0xF7EFFF, 0xF5E3F6);
		EquipItem(Pocket.Shoe, 0x429B, 0x221F1F, 0x816629, 0x221F1F);

		SetLocation(region: 411, x: 8881, y: 8487);

		SetDirection(197);
		SetStand("chapter4/elf/male/anim/elf_npc_siobhanin");

		Phrases.Add("Welcome to Rath Royal Castle.");
	}
}
