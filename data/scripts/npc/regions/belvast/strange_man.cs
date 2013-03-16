using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Strange_manScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_strange_man");
		SetRace(25);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 147, eyeColor: 126, lip: 46);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1324, 0x7B8396, 0xF9A948, 0xF2CCE0);
		EquipItem(Pocket.Hair, 0xC22, 0xA7A59D, 0xA7A59D, 0xA7A59D);
		EquipItem(Pocket.Armor, 0x3E02, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Glove, 0x4214, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Shoe, 0x4337, 0x2B2C35, 0x62404D, 0x66685A);
		EquipItem(Pocket.Robe, 0x4ABA, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 4005, x: 23285, y: 41349);

		SetDirection(240);
		SetStand("");
	}
}
