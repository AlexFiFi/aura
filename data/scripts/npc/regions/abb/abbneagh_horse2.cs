using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Abbneagh_horse2Script : Abbneagh_horse_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_abbneagh_horse2");

		NPC.ColorA = 0x808780;
		NPC.ColorB = 0x180C0C;
		NPC.ColorC = 0x808080;		



		SetLocation(region: 302, x: 126808, y: 86900);

		SetDirection(113);
		SetStand("pet/anim/horse/pet_horse_natural_stand_friendly");
	}
}
