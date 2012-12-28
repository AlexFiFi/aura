using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Castle_guard4Script : Castle_guard_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_castle_guard4");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 0, eyeColor: 0, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0xFCD1C5, 0xF9A434, 0x34004B);
		EquipItem(Pocket.Hair, 0x1007, 0x6E06A9, 0x6E06A9, 0x6E06A9);

		SetLocation(region: 411, x: 11368, y: 7714);

		SetDirection(125);
	}
}
