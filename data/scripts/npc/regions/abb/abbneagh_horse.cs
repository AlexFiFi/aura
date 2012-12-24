using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Abbneagh_horseScript : Abbneagh_horse_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_abbneagh_horse");

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x120303;
		NPC.ColorC = 0x808080;		



		SetLocation(region: 302, x: 126733, y: 86655);

		SetDirection(108);
		SetStand("pet/anim/horse/pet_horse_natural_sit_01");
	}
}
