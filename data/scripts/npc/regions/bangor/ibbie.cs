using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class IbbieScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_ibbie");
		SetRace(10001);
		SetBody(height: 0f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 0, eyeColor: 90, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0x238FB7, 0x9D6E23, 0xF3C91);
		EquipItem(Pocket.Hair, 0xBD0, 0xB78B68, 0xB78B68, 0xB78B68);
		EquipItem(Pocket.Armor, 0x3AC2, 0xFFC3BF, 0xFFFFFF, 0xFFFFFF);
		EquipItem(Pocket.Glove, 0x3E8B, 0xFFFFFF, 0x6D696C, 0xD9EEE5);
		EquipItem(Pocket.Shoe, 0x426F, 0x702639, 0x9F5B0D, 0x5F6069);
		EquipItem(Pocket.Head, 0x465E, 0xFFDBC5, 0x9B7685, 0x736A4B);

		SetLocation(region: 31, x: 10774, y: 15796);

		SetDirection(197);
		SetStand("human/anim/female_natural_sit_02");
        
		Phrases.Add("*Cough* *Cough*");
		Phrases.Add("Are you... lonely, too?");
		Phrases.Add("Daddy...");
		Phrases.Add("I think I have a fever...");
		Phrases.Add("I wish I can gain some weight, too...");
		Phrases.Add("I'm lonely...");
		Phrases.Add("I'm tired of being sick...");
		Phrases.Add("Maybe I messed up on this one...");
		Phrases.Add("Mom...");
		Phrases.Add("Sigh...");
		Phrases.Add("There are so many people with such mysterious items...");
		Phrases.Add("What does the afterlife look like...? I wish I had a friend...");
		Phrases.Add("Where is Sion?");

	}
}
