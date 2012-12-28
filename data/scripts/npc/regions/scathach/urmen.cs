using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class UrmenScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_urmen");
		SetRace(8002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 22, eye: 95, eyeColor: 0, lip: 29);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x22C4, 0xFED3CF, 0x116A94, 0xF39E37);
		EquipItem(Pocket.Hair, 0x1F56, 0x0, 0x0, 0x0);
		EquipItem(Pocket.Armor, 0x3E2E, 0xA69D8B, 0x2B2626, 0x6E724E);
		EquipItem(Pocket.RightHand1, 0x9C58, 0x808080, 0x959595, 0x3C4155);
		EquipItem(Pocket.RightHand2, 0x9E22, 0x808080, 0x959595, 0x3C4155);

		SetLocation(region: 4014, x: 33015, y: 44133);

		SetDirection(198);
		SetStand("chapter4/giant/male/anim/giant_c4_npc_urmen");

		Phrases.Add("(He tempers an old sword.)");
		Phrases.Add("I couldn't have made a better sword myself. Wait, I DID make this sword myself!");
		Phrases.Add("Royal, shmoyal! All alchemists are the same.");
		Phrases.Add("Sieve needs to get lost...");
		Phrases.Add("That Sieve...!");
		Phrases.Add("Umph!");
		Phrases.Add("We believe in you, Odran!");
	}
}
