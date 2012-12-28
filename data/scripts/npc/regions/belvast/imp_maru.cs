using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Imp_maruScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_imp_maru");
		SetRace(321);
		SetBody(height: 0.8f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 247, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0x1A8888;
		NPC.ColorB = 0x775C5C;
		NPC.ColorC = 0xC76917;		



		SetLocation(region: 4005, x: 30812, y: 37375);

		SetDirection(53);
		SetStand("chapter4/monster/anim/imp/imp_c4_npc_shy");

		Phrases.Add("Ah... I want to fall in love...");
		Phrases.Add("If you know anyone, introduce her to me.");
		Phrases.Add("I'm not too shabby in the looks department, you know.");
		Phrases.Add("Will that kind of love come to me also?");
	}
}
