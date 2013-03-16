using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Elf_familyguideScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_elf_familyguide");
		SetRace(9002);
		SetBody(height: 0.6999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 34, eyeColor: 170, lip: 1);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1AF4, 0x626168, 0xD0AC53, 0xFFEA6E);
		EquipItem(Pocket.Hair, 0x1774, 0x0, 0x0, 0x0);
		EquipItem(Pocket.Armor, 0x3B4C, 0xEFDFD5, 0xE8D7DF, 0xC29B5E);
		EquipItem(Pocket.Shoe, 0x42AA, 0x0, 0x5C5A63, 0x2B99F7);

		SetLocation(region: 3100, x: 368001, y: 429410);

		SetDirection(205);
		SetStand("");
	}
}
