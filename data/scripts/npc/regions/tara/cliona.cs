using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class ClionaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_cliona");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 32, eyeColor: 54, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0xF43, 0xFAA750, 0xF89C35, 0x5F2C);
		EquipItem(Pocket.Hair, 0xC22, 0x3C1004, 0x3C1004, 0x3C1004);
		EquipItem(Pocket.Armor, 0x3C1C, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 401, x: 106183, y: 86320);

		SetDirection(194);
		SetStand("human/female/anim/female_natural_stand_npc_Dilys");

		Phrases.Add("A new day to start off productively!");
		Phrases.Add("Hopefully, by next year, I'll be able to move to a bigger house.");
		Phrases.Add("I hope I can sell it at a higher price.");
		Phrases.Add("My body is sore from the exercise I did yesterday.");
		Phrases.Add("When is the book I ordered from Buchanan going to arrive?");
	}
}
