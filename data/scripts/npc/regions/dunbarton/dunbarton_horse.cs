using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

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
