using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Tara_guard1Script : Tara_guard_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_tara_guard1");
		SetFace(skin: 16, eye: 3, eyeColor: 126, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0xF4F7D8, 0x594A9F, 0xDEC7E2);
		EquipItem(Pocket.Hair, 0xFA3, 0x393839, 0x393839, 0x393839);

		SetLocation(region: 401, x: 82342, y: 122341);

		SetDirection(144);
	}
}
