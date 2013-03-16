using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Goblin_hicoScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_goblin_hico");
		SetRace(322);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 23, eye: 3, eyeColor: 7, lip: 2);

		SetColor(0x749F56, 0x908926, 0x333333);



		SetLocation(region: 4005, x: 45547, y: 24662);

		SetDirection(216);
		SetStand("");

		Phrases.Add("Dishes made by Vanalen are quite delicious...");
		Phrases.Add("Hmm, yes...vegetables are a must for diets.");
		Phrases.Add("How come I never get fat?");
		Phrases.Add("I'll make you the greatest diet menu...");
		Phrases.Add("I'm currently learning how to cook.");
		Phrases.Add("I'm gonna cook for my friends.");
		Phrases.Add("Let's do this diet, everyone!");
		Phrases.Add("Living with humans is so difficult...");
	}
}
