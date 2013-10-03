using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Courcleinhabitant8Script : Courcleinhabitant_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_courcleinhabitant8");
		SetRace(10001);
		SetFace(skin: 25, eye: 7, eyeColor: 23, lip: 1);

		EquipItem(Pocket.Face, 0xF3C, 0x9C318F, 0xB20829, 0x9AD089);
		EquipItem(Pocket.Hair, 0xBC7, 0x196240, 0x196240, 0x196240);
		EquipItem(Pocket.Armor, 0x3B8E, 0x5D6962, 0x213547, 0x5A6A6A);

		SetLocation(region: 3300, x: 255696, y: 184995);

		SetDirection(250);
	}
}
