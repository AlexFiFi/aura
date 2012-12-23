using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class AlexinaScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_alexina");
		SetRace(10001);
		SetBody(height: 1.13f, fat: 1f, upper: 1.1f, lower: 1f);
		SetFace(skin: 19, eye: 31, eyeColor: 4, lip: 2);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0x5C52A3, 0x7E8109, 0xE36027);
		EquipItem(Pocket.Hair, 0xBDF, 0xFF492512, 0xFF492512, 0xFF492512);
		EquipItem(Pocket.Armor, 0x3B37, 0xFF88532F, 0xFF6D512E, 0xFFF4E3B7);

		SetLocation(region: 3001, x: 165482, y: 168943);

		SetDirection(65);
		SetStand("human/female/anim/female_stand_npc_emain_06");
	}
}
