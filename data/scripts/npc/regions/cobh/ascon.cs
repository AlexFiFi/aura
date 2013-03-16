using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class AsconScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_ascon");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 0.7f, upper: 0.7f, lower: 0.7f);
		SetFace(skin: 18, eye: 138, eyeColor: 27, lip: 46);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1340, 0xF79929, 0x5A695F, 0xB1DF);
		EquipItem(Pocket.Hair, 0x1028, 0xE2E1B7, 0xE2E1B7, 0xE2E1B7);
		EquipItem(Pocket.Armor, 0x3DF1, 0xB6D8EA, 0x1F5A99, 0x633C31);

		SetLocation(region: 23, x: 36869, y: 36212);

		SetDirection(101);
		SetStand("chapter4/human/male/anim/male_c4_npc_ascon");
        
		Phrases.Add("I still remember it vividly like it was yesterday.");
		Phrases.Add("In the end, we all die alone.");
		Phrases.Add("It's a bit chilly...");
		Phrases.Add("No matter how much this world changes, beautiful memories will live on.");
		Phrases.Add("No, Arranz! Don't leave!");
		Phrases.Add("There is no better motivation to live than the memories of happier times.");
		Phrases.Add("What I'd do to hold those hands again...");
	}
}
