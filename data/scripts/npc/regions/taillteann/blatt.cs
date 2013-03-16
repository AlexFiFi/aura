using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class BlattScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_blatt");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 12, eyeColor: 25, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1324, 0x6D7540, 0x88595, 0x8D0020);
		EquipItem(Pocket.Hair, 0xFB7, 0x655231, 0x655231, 0x655231);
		EquipItem(Pocket.Armor, 0x3ACC, 0x996633, 0x796A70, 0x211C39);
		EquipItem(Pocket.Shoe, 0x42AC, 0x292B35, 0x484B9C, 0x808080);
		EquipItem(Pocket.Head, 0x466D, 0x0, 0xC4C4C4, 0xC7D3FD);

		SetLocation(region: 300, x: 220640, y: 188425);

		SetDirection(121);
		SetStand("");
	}
}
