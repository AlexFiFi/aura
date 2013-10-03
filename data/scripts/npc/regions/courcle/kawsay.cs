using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class KawsayScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_kawsay");
		SetRace(10002);
		SetBody(height: 1.15f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 27, eye: 0, eyeColor: 135, lip: 0);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x132F, 0x769177, 0x737171, 0xF6EE61);
		EquipItem(Pocket.Hair, 0xFFD, 0xE7DDD1, 0xE7DDD1, 0xE7DDD1);
		EquipItem(Pocket.Armor, 0x3B88, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x46F7, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand1, 0xABE2, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 3300, x: 252790, y: 189150);

		SetDirection(213);
		SetStand("human/male/anim/male_natural_stand_npc_kawsay");
        
		Phrases.Add("Admire the grandeur of the nature that surrounds and dwarfs us.");
		Phrases.Add("Always be thankful for the blessings of Great Spirit of Irinid.");
		Phrases.Add("Dear the Great Spirit of Irinid, please listen to my prayer. Show me your grace through your silent blue sky.");
		Phrases.Add("Death is where life begins.");
		Phrases.Add("Everything comes and goes, and nothing lives forever.");
		Phrases.Add("Frogs never drink up all the water where they dwell.");
		Phrases.Add("Mother Nature doesn't discriminate anyone; the sun shines and rain showers on both good and evil.");
		Phrases.Add("We Humans are ants to the mountains, and are as mountains to the ants.");
	}
}
