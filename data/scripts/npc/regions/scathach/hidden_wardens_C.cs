using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Hidden_wardens_CScript : Wardens_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_hidden_wardens_C");
		SetRace(8001);
		SetFace(skin: 22, eye: 72, eyeColor: 76, lip: 33);

		EquipItem(Pocket.Face, 0x1EDC, 0x74238F, 0xD4A463, 0x39247A);
		EquipItem(Pocket.Hair, 0x1B5D, 0x401D69, 0x401D69, 0x401D69);

		SetLocation(region: 4014, x: 66900, y: 53800);

		SetDirection(253);
		SetStand("");
	}
}
