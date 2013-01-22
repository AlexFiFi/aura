// Aura Script
// --------------------------------------------------------------------------
// Castle Guards
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Common.Constants;
using Common.World;
using World.Network;
using World.Scripting;
using World.World;

public class _TaraCastleGuardBaseScript : NPCScript
{
	public override void OnLoad()
	{
		SetRace(10002);
		SetColor(0x808080, 0x808080, 0x808080);
		SetStand("monster/anim/ghostarmor/natural/ghostarmor_natural_stand_friendly");

		EquipItem(Pocket.Armor, 0x3C4D, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x4754, 0x808080, 0x808080, 0x808080);
	}
}

public class TaraCastleGuard1Script : _TaraCastleGuardBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_castle_guard1");
		SetFace(skin: 15, eye: 0, eyeColor: 0, lip: 0);
		SetLocation(401, 112391, 119227, 160);

		EquipItem(Pocket.Face, 0x1324, 0x403F62, 0xB30042, 0x6F5353);
		EquipItem(Pocket.Hair, 0xFBE, 0x7683B0, 0x7683B0, 0x7683B0);
	}
}

public class TaraCastleGuard2Script : _TaraCastleGuardBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_castle_guard2");
		SetFace(skin: 246, eye: 8, eyeColor: 82, lip: 38);
		SetLocation(401, 112800, 118794, 160);

		EquipItem(Pocket.Face, 0x1324, 0x704E50, 0x737374, 0x5DAB);
		EquipItem(Pocket.Hair, 0xFFA, 0x923B5D, 0x923B5D, 0x923B5D);
	}
}
