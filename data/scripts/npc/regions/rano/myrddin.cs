using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class MyrddinScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_myrddin");
		SetRace(10002);
		SetBody(height: 1.03f, fat: 1f, upper: 1.03f, lower: 1f);
		SetFace(skin: 17, eye: 3, eyeColor: 48, lip: 2);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0xFDAD86, 0xFFE3D0, 0xB29C);
		EquipItem(Pocket.Hair, 0xFC3, 0xFFC9966B, 0xFFC9966B, 0xFFC9966B);
		EquipItem(Pocket.Armor, 0x3AF7, 0xFFFFFFFF, 0xFF1C2444, 0xFF14295A);
		EquipItem(Pocket.Shoe, 0x42A1, 0xFF0C1123, 0xFF1A5350, 0xFFFFFF);
		EquipItem(Pocket.Head, 0x46B2, 0xFF000000, 0xFF28486F, 0xFF4378B6);

		SetLocation(region: 3001, x: 309582, y: 117816);

		SetDirection(9);
		SetStand("human/male/anim/male_natural_stand_npc_Ranald");
        
		Phrases.Add("Ahhhhh, are you gonna be working late again tonight?");
		Phrases.Add("All preparations for departure are in order.");
		Phrases.Add("Could you please check on your equipment? Check the bathroom and everywhere else...");
		Phrases.Add("Gosh, I should've plucked my eyebrows a bit when I had the time...");
		Phrases.Add("Hello, I'm First Officer Myrddin. Our first-class service will usher in a new era of hope!");
		Phrases.Add("I hope you brought your fishing rod since you'll be on a boat.");
		Phrases.Add("I thought the sunscreen was here somewhere...");
		Phrases.Add("If you want to know about fishing grounds, the captain will be answer your questions better than I.");
		Phrases.Add("I'm assuming you have brought your fishing rod?");
		Phrases.Add("Please don't use your owl when leaving or entering the Port. Those pests are a danger to the ship!");
		Phrases.Add("Should I tie my hair in a ponytail?");
		Phrases.Add("Well, I had a face mask on last night, and then I kind of fell asleep...");
	}
}
