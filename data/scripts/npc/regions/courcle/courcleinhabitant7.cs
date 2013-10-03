using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Courcleinhabitant7Script : Courcleinhabitant_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_courcleinhabitant7");
		SetRace(10001);
		SetFace(skin: 25, eye: 24, eyeColor: 148, lip: 2);

		EquipItem(Pocket.Face, 0xF3C, 0x6C6F6F, 0xE4847E, 0x2BB76E);
		EquipItem(Pocket.Hair, 0xBD0, 0xC67139, 0xC67139, 0xC67139);
		EquipItem(Pocket.Armor, 0x3B8E, 0x272D63, 0x5B6A60, 0x737C9F);

		SetLocation(region: 3300, x: 256333, y: 184246);

		SetDirection(130);
	}
}
