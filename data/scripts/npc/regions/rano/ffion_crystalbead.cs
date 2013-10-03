using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Ffion_crystalbeadScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_ffion_crystalbead");
		SetRace(990005);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0xFFFFFF, 0x43587C, 0x808080);



		SetLocation(region: 3001, x: 164944, y: 160203);

		SetDirection(31);
		SetStand("");
	}
}
