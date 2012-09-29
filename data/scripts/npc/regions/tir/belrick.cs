using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class BelrickScript : CommerceElephantScript
{
	public override void OnLoad()
	{
		base.OnLoad();

		SetName("_tircho_elephant");
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		SetLocation(region: 1, x: 6811, y: 17049);

		SetDirection(56);
	}
}
