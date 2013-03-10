using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class CommerceHorseScript : NPCScript
{
	public override void OnLoad()
	{
		SetRace(378);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);
		SetStand("pet/anim/horse/pet_carriage_solo_horse_stand_friendly");
        
        Phrases.Add("*Grunt*");
        Phrases.Add("*Huff puff*");
        Phrases.Add("Neigh!");
        Phrases.Add("Neiiighhh!");
	}
}
