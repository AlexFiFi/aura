using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Taillteann_horseScript : CommerceHorseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_horse");
        
		NPC.ColorA = 0x808780;
		NPC.ColorB = 0x180C0C;
		NPC.ColorC = 0x808080;		

		SetLocation(region: 300, x: 239713, y: 192902);

		SetDirection(200);
	}
}
