using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class NiccaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_nicca");
		SetRace(10002);
		SetBody(height: 1.3f, fat: 0.99f, upper: 1.41f, lower: 1.05f);
		SetFace(skin: 19, eye: 5, eyeColor: 160, lip: 16);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1324, 0xFA9C70, 0x566D16, 0x8C7D00);
		EquipItem(Pocket.Hair, 0x135B, 0xFFCB864E, 0xFFCB864E, 0xFFCB864E);
		EquipItem(Pocket.Armor, 0x32E7, 0xFFFED9D1, 0xFF694549, 0xFF884444);
		EquipItem(Pocket.Shoe, 0x429A, 0xFF804040, 0xFFC6825E, 0xFFF9E6A2);

		SetLocation(region: 3001, x: 165786, y: 170346);

		SetDirection(153);
		SetStand("human/male/anim/male_stand_Tarlach_anguish");
        
		Phrases.Add("*Whistle* Hey you, yea you, pretty lady! Come look over here!");
		Phrases.Add("*Yawn* So bored.");
		Phrases.Add("Anybody wanna make a bet?");
		Phrases.Add("Argh. I wonder if I can find an interesting piece of artifact somewhere.");
		Phrases.Add("I've got some shiny new weapons in stock!");
		Phrases.Add("Man, I'm bound to get a break soon...");
		Phrases.Add("Sharpen your knives over here. You can change the handle too.");
	}
}
