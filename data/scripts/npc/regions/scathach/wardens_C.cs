using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Wardens_CScript : Wardens_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_wardens_C");
		SetRace(8001);
		SetFace(skin: 22, eye: 72, eyeColor: 76, lip: 33);

		EquipItem(Pocket.Face, 0x1EDC, 0x5AC56, 0x562488, 0xF79B62);
		EquipItem(Pocket.Hair, 0x1B64, 0xD01D09, 0xD01D09, 0xD01D09);

		SetLocation(region: 4014, x: 33561, y: 42889);

		SetDirection(101);
		SetStand("chapter3/giant/female/anim/giant_female_c3_npc_karpfen");
	}
}
