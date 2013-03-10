using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Taillteann_goblinScript : CommerceGoblinScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_goblin");
		SetRace(322);
		SetFace(skin: 168, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0x4A1D61;
		NPC.ColorB = 0x4A1D61;
		NPC.ColorC = 0xE3E3E3;		

		SetLocation(region: 300, x: 240278, y: 192797);

		SetDirection(196);
	}
}
