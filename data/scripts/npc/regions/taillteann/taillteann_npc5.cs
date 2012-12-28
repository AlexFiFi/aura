using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Taillteann_npc5Script : Taillteann_npc_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_npc5");
		SetRace(10002);
		SetFace(skin: 19, eye: 23, eyeColor: 76, lip: 15);

		EquipItem(Pocket.Face, 0x1324, 0xB257, 0xFEE3D7, 0xF54230);
		EquipItem(Pocket.Hair, 0xFC8, 0xFFCC00, 0xFFCC00, 0xFFCC00);
		EquipItem(Pocket.Armor, 0x3A9B, 0x94C384, 0xBF8941, 0x4E3578);
		EquipItem(Pocket.Shoe, 0x429A, 0x666633, 0x666699, 0x808080);
		EquipItem(Pocket.Robe, 0x4A39, 0x666666, 0xEDB300, 0x5727F4);

		SetLocation(region: 300, x: 210462, y: 193062);

		SetDirection(177);
	}
}
