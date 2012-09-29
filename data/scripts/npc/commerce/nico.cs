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

		NPC.ColorA = 0x811E1E;
		NPC.ColorB = 0x3A2D28;
		NPC.ColorC = 0xDDAD1F;

		EquipItem(Pocket.LeftHand1, 0x9DF4, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand1, 0xB3C7, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 1, x: 6221, y: 17173);

		SetDirection(56);
	}
}
