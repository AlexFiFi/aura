using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class TelescopenpcScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_telescopenpc");
		SetRace(990002);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);



		SetLocation(region: 52, x: 43699, y: 36138);

		SetDirection(255);
		SetStand("");
	}
}
