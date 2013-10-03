using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Tara_guardScript : Tara_guard_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_tara_guard");
		SetFace(skin: 20, eye: 5, eyeColor: 8, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0x755600, 0xBDDFB0, 0xFA9464);
		EquipItem(Pocket.Hair, 0xFA4, 0x9C5D42, 0x9C5D42, 0x9C5D42);

		SetLocation(region: 401, x: 82183, y: 122645);

		SetDirection(144);
	}
}
