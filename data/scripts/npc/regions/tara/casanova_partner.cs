using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Casanova_partnerScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_casanova_partner");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 0.9f);
		SetFace(skin: 15, eye: 15, eyeColor: 8, lip: 23);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0xF65654, 0x68A2, 0xA2BE61);
		EquipItem(Pocket.Hair, 0xBDA, 0x21004C, 0x21004C, 0x21004C);
		EquipItem(Pocket.Armor, 0x3BEA, 0x9DBBF1, 0xB0BCA6, 0xBE8BD6);
		EquipItem(Pocket.Shoe, 0x4281, 0x5E5A8E, 0x3BA2F8, 0x808080);
		EquipItem(Pocket.Head, 0x4725, 0xF0E217, 0x55799C, 0xBFB40B);

		SetLocation(region: 401, x: 105516, y: 105870);

		SetDirection(98);
		SetStand("chapter3/human/female/anim/female_c3_npc_brenda");

		Phrases.Add("Is it getting hot in here?");
		Phrases.Add("Oh my, he's so hot!");
	}
}
