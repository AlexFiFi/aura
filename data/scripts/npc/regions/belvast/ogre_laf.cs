using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Ogre_lafScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_ogre_laf");
		SetRace(323);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 205, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0x512957, 0x304265, 0xFFFFFF);



		SetLocation(region: 4005, x: 33184, y: 40825);

		SetDirection(0);
		SetStand("chapter4/monster/anim/ogre/ogre_c4_npc_diet");

		Phrases.Add("(Stomach growls.)");
		Phrases.Add("*Sniff sniff* I smell meat...");
		Phrases.Add("I'm hungry... *sniff sniff*");
		Phrases.Add("Meat... Give me meat...");
		Phrases.Add("My meat! Ah... Dream...");
	}
}
