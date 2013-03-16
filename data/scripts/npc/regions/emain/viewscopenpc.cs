using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class ViewscopenpcScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_viewscopenpc");
		SetRace(990004);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);



		SetLocation(region: 52, x: 43713, y: 36412);

		SetDirection(194);
		SetStand("");
	}
}
