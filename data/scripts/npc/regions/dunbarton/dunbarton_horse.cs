using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Dunbarton_horseScript : CommerceHorseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_dunbarton_horse");

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x120303;
		NPC.ColorC = 0x808080;		

		SetLocation(region: 14, x: 43324, y: 46434);

		SetDirection(66);
	}
}
