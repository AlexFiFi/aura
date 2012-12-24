using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Abbneagh_horse_baseScript : NPCScript
{
	public override void OnLoad()
	{
		SetRace(422);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);
		Phrases.Add("*Grunt*");
		Phrases.Add("*Huff puff*");
		Phrases.Add("Neigh!");
		Phrases.Add("Neiiighhh!");

	}
}
