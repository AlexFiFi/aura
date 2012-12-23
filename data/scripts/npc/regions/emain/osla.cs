using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class OslaScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_osla");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 2, eyeColor: 47, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0xF5A73F, 0xF69C35, 0xF98838);
		EquipItem(Pocket.Hair, 0xBDD, 0xE29B45, 0xE29B45, 0xE29B45);
		EquipItem(Pocket.Armor, 0x32DF, 0xD6C5BA, 0x947E6B, 0x0);
		EquipItem(Pocket.Shoe, 0x4466, 0x61534E, 0xF6D493, 0x644356);

		SetLocation(region: 52, x: 37398, y: 42382);

		SetDirection(28);
		SetStand("human/male/anim/male_natural_stand_npc_bryce");
	}
}
