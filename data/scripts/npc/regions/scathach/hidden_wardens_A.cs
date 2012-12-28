using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Hidden_wardens_AScript : Wardens_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_hidden_wardens_A");
		SetRace(8002);
		SetFace(skin: 22, eye: 150, eyeColor: 28, lip: 26);

		EquipItem(Pocket.Face, 0x22C4, 0x576869, 0x9355, 0x18B2D1);
		EquipItem(Pocket.Hair, 0x1F46, 0x1F3C1B, 0x1F3C1B, 0x1F3C1B);

		SetLocation(region: 4014, x: 61500, y: 41200);

		SetDirection(17);
		SetStand("");
	}
}
