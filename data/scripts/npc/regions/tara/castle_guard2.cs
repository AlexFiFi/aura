using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Castle_guard2Script : Castle_guard_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_castle_guard2");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1.1f, upper: 1.1f, lower: 1.1f);
		SetFace(skin: 246, eye: 8, eyeColor: 82, lip: 38);

		EquipItem(Pocket.Face, 0x1324, 0x704E50, 0x737374, 0x5DAB);
		EquipItem(Pocket.Hair, 0xFFA, 0x923B5D, 0x923B5D, 0x923B5D);

		SetLocation(region: 401, x: 112800, y: 118794);

		SetDirection(160);
	}
}
