using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Taillteann_npc6Script : Taillteann_npc_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_npc6");
		SetRace(10001);
		SetFace(skin: 15, eye: 7, eyeColor: 0, lip: 24);

		EquipItem(Pocket.Face, 0xF3C, 0x6F6C6E, 0x714856, 0x3AB5C);
		EquipItem(Pocket.Hair, 0xBD8, 0x9C5D42, 0x9C5D42, 0x9C5D42);
		EquipItem(Pocket.Armor, 0x3AEA, 0xEFE3B5, 0x663300, 0xE3E3FC);
		EquipItem(Pocket.Shoe, 0x426F, 0x996600, 0xC7E8FF, 0x808080);

		SetLocation(region: 300, x: 213822, y: 196933);

		SetDirection(121);
	}
}
