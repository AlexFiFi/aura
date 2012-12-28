using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Wardens_BScript : Wardens_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_wardens_B");
		SetRace(10002);
		SetFace(skin: 22, eye: 23, eyeColor: 29, lip: 21);

		EquipItem(Pocket.Face, 0x1324, 0xB5C2, 0x5E000A, 0xFFEBA3);
		EquipItem(Pocket.Hair, 0x100E, 0x367071, 0x367071, 0x367071);

		SetLocation(region: 4014, x: 33891, y: 44839);

		SetDirection(191);
		SetStand("elf/male/anim/elf_npc_granites_stand_friendly");
	}
}
