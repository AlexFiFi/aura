using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Castle_road_patrol_02Script : Castle_guard_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_castle_road_patrol_02");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 18, eye: 26, eyeColor: 82, lip: 1);

		EquipItem(Pocket.Face, 0x1324, 0x5A38, 0xF79827, 0x769083);
		EquipItem(Pocket.Hair, 0xFB0, 0x544223, 0x544223, 0x544223);
		EquipItem(Pocket.RightHand2, 0x9C4C, 0xB0B0B0, 0x47A8E5, 0x676661);

		SetLocation(region: 401, x: 104602, y: 109538);

		SetDirection(60);
		SetStand("");
	}
}
