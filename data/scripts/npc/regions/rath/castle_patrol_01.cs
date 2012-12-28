using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Castle_patrol_01Script : Castle_guard_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_castle_patrol_01");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1.1f, upper: 1.4f, lower: 1.1f);
		SetFace(skin: 17, eye: 104, eyeColor: 29, lip: 4);

		EquipItem(Pocket.Face, 0x1324, 0xF79825, 0xF69B2D, 0x3868AF);
		EquipItem(Pocket.Hair, 0xFB0, 0x544223, 0x544223, 0x544223);
		EquipItem(Pocket.RightHand2, 0x9C4C, 0xB0B0B0, 0x47A8E5, 0x676661);

		SetLocation(region: 410, x: 23634, y: 8821);

		SetDirection(63);
		SetStand("");
	}
}
