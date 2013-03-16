using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class JeniferScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_jenifer");
		SetRace(10001);
		SetBody(height: 1.1f, fat: 1.1f, upper: 1f, lower: 1.1f);
		SetFace(skin: 17, eye: 4, eyeColor: 119, lip: 1);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3D, 0xB4BDE0, 0xD4E1F3, 0x717172);
		EquipItem(Pocket.Hair, 0xBB9, 0x240C1A, 0x240C1A, 0x240C1A);
		EquipItem(Pocket.Armor, 0x3AAC, 0xF98C84, 0xFBDDD7, 0x351311);
		EquipItem(Pocket.Shoe, 0x4275, 0x0, 0x366961, 0xDAD6EB);

		SetLocation(region: 31, x: 14628, y: 8056);

		SetDirection(26);
		SetStand("human/female/anim/female_natural_stand_npc_lassar");
        
        Phrases.Add("Ah, I'm so bored...");
		Phrases.Add("Ah. What an unbelievably beautiful weather...");
		Phrases.Add("I could never keep this place clean... It always gets dirty.");
		Phrases.Add("I thought there was something else that needed to be done...");
		Phrases.Add("I wish it would rain so I could take a day off.");
		Phrases.Add("I'm gonna get drunk if I drink too much...");
		Phrases.Add("I'm so tired...");
		Phrases.Add("It would be nice if Riocard drank...");
		Phrases.Add("Perhaps I should lose some weight...");
		Phrases.Add("Riocard! Did you finish everything I asked you to do?");
		Phrases.Add("Riocard. Come play with me.");
		Phrases.Add("Today's fortune is... no profit?");
		Phrases.Add("Wait a minute...");
	}
}
