using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class WanstScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_wanst");
		SetRace(30);
		SetBody(height: 1f, fat: 1.1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		SetLocation(region: 3200, x: 295393, y: 229140);

		SetDirection(163);
		SetStand("giant/male/anim/giant_npc_wanst");
        
		Phrases.Add("At this rate, everything will be taken away by those crafty Elves.");
		Phrases.Add("Drink up, friend. Let's party all night. Hahaha!");
		Phrases.Add("Everything going on right now is because of King Krug.");
		Phrases.Add("Hey there. Why don't you stay for a little bit?");
		Phrases.Add("I have a pounding headache.");
		Phrases.Add("I should drink some liquor to chase my hangover away.");
		Phrases.Add("I'm really particular when it comes to alcohol.");
		Phrases.Add("Just because it ages doesn't mean it's good liquor.");
		Phrases.Add("Sometimes I drink to forget about my drinking.  Hahaha!");
		Phrases.Add("We don't accept rain checks here!");
		Phrases.Add("Welcome to Schnabel, the best Pub in Physis.");
	}
}
