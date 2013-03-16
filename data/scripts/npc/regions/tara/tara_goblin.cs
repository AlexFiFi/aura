using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Tara_goblinScript : CommerceGoblinScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_tara_goblin");
		SetRace(322);
		SetFace(skin: 168, eye: 3, eyeColor: 7, lip: 2);

		SetColor(0xBA8E7F, 0x3A2D28, 0x286C81);

		SetLocation(region: 401, x: 74641, y: 128688);

		SetDirection(121);
	}
}
