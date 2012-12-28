using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class EffieScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_effie");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 0.1f, upper: 1f, lower: 0.85f);
		SetFace(skin: 17, eye: 3, eyeColor: 39, lip: 2);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0x134142, 0xA8164, 0x3A67);
		EquipItem(Pocket.Hair, 0xBE6, 0xFFCAAE7B, 0xFFCAAE7B, 0xFFCAAE7B);
		EquipItem(Pocket.Armor, 0x3B39, 0xFF994322, 0xFF690C0C, 0xFFDA9343);
		EquipItem(Pocket.Shoe, 0x428E, 0xFF9E583A, 0xFFFFFFFF, 0xFFFFFFFF);

		SetLocation(region: 3001, x: 164496, y: 170646);

		SetDirection(213);
		SetStand("human/female/anim/female_stand_npc_iria_01");
        
		Phrases.Add("...");
		Phrases.Add("Am I missing anything...");
		Phrases.Add("Are you looking at some of the merchandise?...");
		Phrases.Add("I guess a lot of people will be leaving... for expeditions soon...");
		Phrases.Add("I wonder if the ship has docked yet...");
		Phrases.Add("I wonder where it is...");
		Phrases.Add("The wind is...");
		Phrases.Add("Welcome...");
	}
}
