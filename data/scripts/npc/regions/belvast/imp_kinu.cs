using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Imp_kinuScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_imp_kinu");
		SetRace(321);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 247, eye: 3, eyeColor: 7, lip: 2);

		SetColor(0x817895, 0xAB8E8E, 0x4C4C6B);



		SetLocation(region: 4005, x: 46469, y: 39990);

		SetDirection(238);
		SetStand("chapter4/monster/anim/imp/mon_c4_imp_commerce");

		Phrases.Add("Even that Ogre can't hurt me now!");
		Phrases.Add("I'll work hard today, too!");
		Phrases.Add("I'm a strong Imp!");
		Phrases.Add("Money is the best.");
		Phrases.Add("You can do anything with money.");
	}
}
