using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class InflamesScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_inflames");
		SetRace(999998);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		SetLocation(region: 300, x: 214825, y: 203451);

		SetDirection(0);
		SetStand("");
        
		Phrases.Add("(Seems like he really wants to eat some Mana Herbs.)");
		Phrases.Add("....");
		Phrases.Add("I saw a dragon!");
		Phrases.Add("I want some Mana Herbs...");
		Phrases.Add("Iri....Iria....");
		Phrases.Add("My fever...");
		Phrases.Add("Oh, oh, ow....");
		Phrases.Add("Ooow...");
		Phrases.Add("Ow....");
	}
}
