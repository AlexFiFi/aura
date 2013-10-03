using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class SineadScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_sinead");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 104, eyeColor: 27, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0xF44, 0x990055, 0xC70037, 0xF89E3C);
		EquipItem(Pocket.Hair, 0xC27, 0xAF8754, 0xAF8754, 0xAF8754);
		EquipItem(Pocket.Armor, 0x3C4E, 0x7C5B46, 0x5B0000, 0xCDC5AE);
		EquipItem(Pocket.Shoe, 0x42AF, 0x4A2B31, 0x5B5253, 0x808080);

		SetLocation(region: 415, x: 4796, y: 4790);

		SetDirection(63);
		SetStand("");

		Phrases.Add("...");
		Phrases.Add("Hmm, what is the best way to approach this problem?");
		Phrases.Add("I better double-check today's schedule.");
		Phrases.Add("I have so many things to attend to, I feel my brain might explode!");
		Phrases.Add("I must make sure His Royal Majesty does not worry...");
		Phrases.Add("It's quite a lot of work to answer every letter personally.");
		Phrases.Add("I've just received an update on the Shadow Realm situation.");
		Phrases.Add("My, where has the time gone?");
		Phrases.Add("Now, where did I leave my extra pair of scissors?");
		Phrases.Add("Step by step, Sinead. You can solve this...");
		Phrases.Add("The Alchemists are stirring up trouble again.");
		Phrases.Add("What business brings you here?");
	}
}
