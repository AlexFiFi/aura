using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Alchemist_DScript : Scathach_alchemist_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_alchemist_D");
		SetRace(9001);
		SetFace(skin: 19, eye: 41, eyeColor: 192, lip: 1);

		EquipItem(Pocket.Face, 0x1713, 0x3D3B, 0x725F48, 0x88488);
		EquipItem(Pocket.Hair, 0x138D, 0xFF9FA4, 0xFF9FA4, 0xFF9FA4);

		SetLocation(region: 4014, x: 33049, y: 42353);

		SetDirection(126);
	}
}
