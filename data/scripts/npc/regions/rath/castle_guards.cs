// Aura Script
// --------------------------------------------------------------------------
// Castle Guards
// Inheriting: tara/castle_guards.cs
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class RathCastleGuard1Script : _TaraCastleGuardBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_castle_guard3");
		SetBody(height: 1f, fat: 1.1f, upper: 1.1f, lower: 1.1f);
		SetFace(skin: 246, eye: 8, eyeColor: 82, lip: 38);
		SetLocation(411, 11362, 8402, 126);

		EquipItem(Pocket.Face, 4900, 0x434F65, 0xB14026, 0x711F83);
		EquipItem(Pocket.Hair, 4103, 0x923B5D, 0x923B5D, 0x923B5D);
	}
}

public class RathCastleGuard2Script : _TaraCastleGuardBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_castle_guard4");
		SetFace(skin: 15, eye: 0, eyeColor: 0, lip: 0);
		SetLocation(411, 11368, 7714, 125);

		EquipItem(Pocket.Face, 4900, 0xFCD1C5, 0xF9A434, 0x34004B);
		EquipItem(Pocket.Hair, 4103, 0x6E06A9, 0x6E06A9, 0x6E06A9);
	}
}
