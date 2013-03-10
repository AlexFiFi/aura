using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class BryceScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_bryce");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1f, upper: 1.5f, lower: 1f);
		SetFace(skin: 20, eye: 5, eyeColor: 76, lip: 12);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1326, 0x155782, 0x84B274, 0xFBC25C);
		EquipItem(Pocket.Hair, 0xFBB, 0x5B482B, 0x5B482B, 0x5B482B);
		EquipItem(Pocket.Armor, 0x3ABA, 0xFAF7EB, 0x3C2D22, 0x100C0A);
		EquipItem(Pocket.Shoe, 0x4271, 0x0, 0xF69A2B, 0x4B676F);

		SetLocation(region: 31, x: 11365, y: 9372);

		SetDirection(0);
		SetStand("human/male/anim/male_natural_stand_npc_bryce");
        
		Phrases.Add("*Cough* There's just too much dust in here.");
		Phrases.Add("Anyway, where did Ibbie go again?");
		Phrases.Add("Have my eyes really become this bad?");
		Phrases.Add("I don't even have time to read a book these days.");
		Phrases.Add("I'll just have to fight through it.");
		Phrases.Add("It's about the time Ibbie returned.");
		Phrases.Add("It's almost time.");
		Phrases.Add("Mmm... Up to where did I calculate?");
		Phrases.Add("Sion, you little punk... You'll pay if you bully my Ibbie.");
		Phrases.Add("Tomorrow will be better than today.");
		Phrases.Add("Well, cheer up!");
		Phrases.Add("What should I buy Ibbie today?");
		Phrases.Add("When was I supposed to be contacted from Dunbarton?");
	}
}
