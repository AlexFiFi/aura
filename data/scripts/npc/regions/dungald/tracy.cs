using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class TracyScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_tracy");
		SetRace(10002);
		SetBody(height: 1.2f, fat: 1.5f, upper: 2f, lower: 1f);
		SetFace(skin: 19, eye: 9, eyeColor: 27, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1328, 0x248B3E, 0x95958F, 0xB698);
		EquipItem(Pocket.Hair, 0xFB9, 0x754C2A, 0x754C2A, 0x754C2A);
		EquipItem(Pocket.Armor, 0x3A9D, 0x744D3C, 0xDDB372, 0xD6BDA3);
		EquipItem(Pocket.Glove, 0x3E8A, 0x755744, 0x5B40, 0x9E086C);
		EquipItem(Pocket.Shoe, 0x4272, 0x371E00, 0x47, 0x747374);
		EquipItem(Pocket.Head, 0x4661, 0x744D3C, 0xF79622, 0xBE7781);
		EquipItem(Pocket.RightHand1, 0x9C47, 0xA7A894, 0x625F44, 0x872F92);

		SetLocation(region: 16, x: 22900, y: 59500);

		SetDirection(56);
		SetStand("");
        
		Phrases.Add("*Yawn*");
		Phrases.Add("Gee, it's hot...");
		Phrases.Add("I tire out so easily these days...");
		Phrases.Add("It's so dull here...");
		Phrases.Add("Man, I'm so sweaty...");
		Phrases.Add("Oh, my arm...");
		Phrases.Add("Oh, my leg...");
		Phrases.Add("Oww, my muscles are sore all over.");
		Phrases.Add("Phew. Alright. Time to rest!");
		Phrases.Add("Should I take a break now?");
	}
}
