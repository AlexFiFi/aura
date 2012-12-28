using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Courcleinhabitant5Script : Courcleinhabitant_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_courcleinhabitant5");
		SetRace(10002);
		SetFace(skin: 27, eye: 5, eyeColor: 23, lip: 2);

		EquipItem(Pocket.Face, 0x1324, 0x5D29, 0xF28FB2, 0x7B16A);
		EquipItem(Pocket.Hair, 0xFF9, 0xFFD7B5, 0xFFD7B5, 0xFFD7B5);
		EquipItem(Pocket.Armor, 0x3B8F, 0x33517A, 0x544060, 0x9F6EAC);

		SetLocation(region: 3300, x: 252714, y: 184405);

		SetDirection(99);
	}
}
