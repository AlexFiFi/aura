// Aura Script
// --------------------------------------------------------------------------
// Castle Road Patrol Guards
// Inheriting: tara/castle_guards.cs
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class _CastleRoadPatrolBaseScript : _TaraCastleGuardBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 18, eye: 26, eyeColor: 82, lip: 1);
		SetDirection(60);

		EquipItem(Pocket.Hair, 4016, 0x544223, 0x544223, 0x544223);
		EquipItem(Pocket.RightHand2, 40012, 0xB0B0B0, 0x47A8E5, 0x676661);
	}
}

public class CastleRoadPatrol1Script : _CastleRoadPatrolBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_castle_road_patrol_01");
		SetLocation(401, 104602, 109538);

		EquipItem(Pocket.Face, 4900, 0x25257D, 0x0FAD6E, 0x004162);
	}
}

public class CastleRoadPatrol2Script : _CastleRoadPatrolBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_castle_road_patrol_02");
		SetLocation(401, 104602, 109538);

		EquipItem(Pocket.Face, 4900, 0x5A38, 0xF79827, 0x769083);
	}
}
