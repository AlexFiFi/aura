using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Ogre_lafScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_ogre_laf");
		SetRace(323);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 205, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x512957;
		NPC.ColorB = 0x304265;
		NPC.ColorC = 0xFFFFFF;		



		SetLocation(region: 4005, x: 33184, y: 40825);

		SetDirection(0);
		SetStand("chapter4/monster/anim/ogre/ogre_c4_npc_diet");
	}
}
