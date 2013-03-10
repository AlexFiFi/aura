using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Courcleinhabitant6Script : Courcleinhabitant_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_courcleinhabitant6");
		SetRace(10001);
		SetFace(skin: 26, eye: 10, eyeColor: 27, lip: 0);

		EquipItem(Pocket.Face, 0xF3C, 0xAC0054, 0xCFDE39, 0x509C3E);
		EquipItem(Pocket.Hair, 0xBC8, 0x606748, 0x606748, 0x606748);
		EquipItem(Pocket.Armor, 0x3B8E, 0x7A83A5, 0x401E00, 0x31231F);

		SetLocation(region: 3300, x: 248308, y: 182178);

		SetDirection(158);
	}
}
