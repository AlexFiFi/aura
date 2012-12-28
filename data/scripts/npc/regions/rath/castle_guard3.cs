using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Castle_guard3Script : Castle_guard_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_castle_guard3");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1.1f, upper: 1.1f, lower: 1.1f);
		SetFace(skin: 246, eye: 8, eyeColor: 82, lip: 38);

		EquipItem(Pocket.Face, 0x1324, 0x434F65, 0xB14026, 0x711F83);
		EquipItem(Pocket.Hair, 0xFFA, 0x923B5D, 0x923B5D, 0x923B5D);

		SetLocation(region: 411, x: 11362, y: 8402);

		SetDirection(126);
	}
}
