using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Goblin_gogoScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_goblin_gogo");
		SetRace(348);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 168, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0xE7E5E5;
		NPC.ColorB = 0x567386;
		NPC.ColorC = 0x67869B;		



		SetLocation(region: 4005, x: 30801, y: 37503);

		SetDirection(172);
		SetStand("chapter4/monster/anim/goblin/npc_c4_gogo");
	}
}
