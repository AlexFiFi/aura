using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Castle_guard1Script : Castle_guard_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_castle_guard1");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 0, eyeColor: 0, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0x403F62, 0xB30042, 0x6F5353);
		EquipItem(Pocket.Hair, 0xFBE, 0x7683B0, 0x7683B0, 0x7683B0);

		SetLocation(region: 401, x: 112391, y: 119227);

		SetDirection(160);
	}
}
