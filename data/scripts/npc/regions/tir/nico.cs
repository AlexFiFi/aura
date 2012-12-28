using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class NicoScript : CommerceGoblinScript
{
	public override void OnLoad()
	{
		base.OnLoad();

		SetName("_tircho_goblin");
		SetFace(skin: 23, eye: 3, eyeColor: 7, lip: 2);
		SetColor(0x811E1E, 0x3A2D28, 0xDDAD1F);

		SetLocation(region: 1, x: 6221, y: 17173);

		SetDirection(56);
	}
}
