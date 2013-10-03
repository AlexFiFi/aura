using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class DorrenScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_dorren");
		SetRace(48);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0x808080, 0x808080, 0xF7F3DE);

		SetLocation(region: 300, x: 216610, y: 200328);

		SetDirection(186);
		SetStand("chapter3/human/female/anim/female_c3_npc_dorren");
        
		Phrases.Add("Ah, changing hearts is more difficult than changing substances through Alchemy.");
		Phrases.Add("Eabha, are you ready with the crystals?");
		Phrases.Add("I should add more fuel to the ovens.");
		Phrases.Add("It's perfect weather for an Alchemy experiment!");
		Phrases.Add("There is no reason to fear anything you face in life.");
		Phrases.Add("There is nothing that can't be understood with the principles of Alchemy.");
		Phrases.Add("You there, be careful of the ovens!");
	}
}
