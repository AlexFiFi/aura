// Aura Script
// --------------------------------------------------------------------------
// Castle Patrol Guards
// Inheriting: tara/castle_guards.cs.
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class RathCastlePatrol1Script : _TaraCastleGuardBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_castle_patrol_01");
		SetBody(height: 1.1f, fat: 1.1f, upper: 1.4f, lower: 1.1f);
		SetFace(skin: 17, eye: 104, eyeColor: 29, lip: 4);
		SetLocation(410, 23634, 8821, 63);

		EquipItem(Pocket.Face, 4900, 0xF79825, 0xF69B2D, 0x3868AF);
		EquipItem(Pocket.Hair, 4016, 0x544223, 0x544223, 0x544223);
		EquipItem(Pocket.RightHand2, 40012, 0xB0B0B0, 0x47A8E5, 0x676661);
	}
}

public class RathCastlePatrol2Script : _TaraCastleGuardBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_castle_patrol_02");
		SetBody(height: 0.9f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 18, eye: 0, eyeColor: 28, lip: 0);
		SetLocation(410, 23226, 8839, 63);

		EquipItem(Pocket.Face, 4951, 0x240049, 0x7E47, 0xF59C30);
		EquipItem(Pocket.Hair, 4016, 0x544223, 0x544223, 0x544223);
		EquipItem(Pocket.RightHand2, 40012, 0xB0B0B0, 0x47A8E5, 0x676661);
	}
}

public class RathCastlePatrol3Script : _TaraCastleGuardBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_castle_patrol_03");
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 20, eye: 5, eyeColor: 8, lip: 0);
		SetLocation(410, 14148, 22118, 255);

		EquipItem(Pocket.Face, 4900, 0x7C0011, 0x51419B, 0x3EB28F);
		EquipItem(Pocket.Hair, 4016, 0x544223, 0x544223, 0x544223);
		EquipItem(Pocket.RightHand2, 40012, 0xB0B0B0, 0x47A8E5, 0x676661);
	}
}

public class RathCastlePatrol4Script : _TaraCastleGuardBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_castle_patrol_04");
		SetBody(height: 1.1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 18, eye: 26, eyeColor: 82, lip: 1);
		SetLocation(410, 9580, 14114);

		EquipItem(Pocket.Face, 4900, 0x412275, 0xB1AC12, 0xDEF0E9);
		EquipItem(Pocket.Hair, 4016, 0x544223, 0x544223, 0x544223);
		EquipItem(Pocket.RightHand2, 40012, 0xB0B0B0, 0x47A8E5, 0x676661);
	}
}
