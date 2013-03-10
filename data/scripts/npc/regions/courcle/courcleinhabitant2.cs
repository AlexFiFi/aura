using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Courcleinhabitant2Script : Courcleinhabitant_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_courcleinhabitant2");
		SetRace(10002);
		SetFace(skin: 25, eye: 4, eyeColor: 29, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0xFBC15B, 0x7DADA2, 0x701369);
		EquipItem(Pocket.Hair, 0xFC2, 0xC3A123, 0xC3A123, 0xC3A123);
		EquipItem(Pocket.Armor, 0x3B99, 0x1B0626, 0x282795, 0x4868CF);

		SetLocation(region: 3300, x: 254648, y: 186331);

		SetDirection(99);
	}
}
