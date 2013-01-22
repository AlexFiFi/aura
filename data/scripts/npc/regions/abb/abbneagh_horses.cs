// Aura Script
// --------------------------------------------------------------------------
// Horses
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Common.Constants;
using Common.World;
using World.Network;
using World.Scripting;
using World.World;

public class AbbNeaghHorse1Script : NPCScript
{
	public override void OnLoad()
	{
		SetRace(422);
		SetName("_abbneagh_horse2");
		SetColor(0x808080, 0x120303, 0x808080);
		SetStand("pet/anim/horse/pet_horse_natural_sit_01");
		SetLocation(302, 126733, 86655, 108);
		
		AddPhrases("*Grunt*", "*Huff puff*", "Neigh!", "Neiiighhh!");
	}
}

public class AbbNeaghHorse2Script : AbbNeaghHorse1Script
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_abbneagh_horse2");
		SetColor(0x808780, 0x180C0C, 0x808080);
		SetStand("pet/anim/horse/pet_horse_natural_stand_friendly");
		SetLocation(302, 126808, 86900, 113);
	}
}
