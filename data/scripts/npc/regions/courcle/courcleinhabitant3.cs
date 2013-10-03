using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Courcleinhabitant3Script : Courcleinhabitant_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_courcleinhabitant3");
		SetRace(10002);
		SetFace(skin: 27, eye: 8, eyeColor: 82, lip: 20);

		EquipItem(Pocket.Face, 0x1324, 0x85880D, 0xD59A5C, 0x87A7D8);
		EquipItem(Pocket.Hair, 0xFAE, 0x211407, 0x211407, 0x211407);
		EquipItem(Pocket.Armor, 0x3B8F, 0xCED3BA, 0x162561, 0x7982A6);

		SetLocation(region: 3300, x: 254379, y: 186844);

		SetDirection(103);
	}
}
