using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Ogre_tobeScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_ogre_tobe");
		SetRace(323);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 131, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x706464;
		NPC.ColorB = 0x748274;
		NPC.ColorC = 0x37322D;		



		SetLocation(region: 4005, x: 37773, y: 29562);

		SetDirection(76);
		SetStand("chapter4/monster/anim/ogre/ogre_c4_npc_health");

		Phrases.Add("Beautiful muscles, everyone love!");
		Phrases.Add("Gonna arm-wrestle like a pro!");
		Phrases.Add("Mmm... hmm... Muscles, so beautiful.");
		Phrases.Add("My muscles are the best.");
		Phrases.Add("My muscles, they are awesome.");
		Phrases.Add("The world is beautiful. My muscles are beautiful. World beautiful like muscles.");
	}
}
