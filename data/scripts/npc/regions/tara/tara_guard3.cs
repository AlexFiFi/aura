using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Tara_guard3Script : Tara_guard_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_tara_guard3");
		SetFace(skin: 16, eye: 3, eyeColor: 126, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0x496D72, 0x486770, 0xECA721);
		EquipItem(Pocket.Hair, 0xFA3, 0x393839, 0x393839, 0x393839);

		SetLocation(region: 401, x: 101191, y: 101540);

		SetDirection(200);
	}
}
