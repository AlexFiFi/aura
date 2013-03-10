using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Taillteann_npc4Script : Taillteann_npc_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_npc4");
		SetRace(10001);
		SetFace(skin: 15, eye: 33, eyeColor: 0, lip: 23);

		EquipItem(Pocket.Face, 0xF3C, 0xDCDD45, 0xA37507, 0x5D2E92);
		EquipItem(Pocket.Hair, 0xBE3, 0x173900, 0x173900, 0x173900);
		EquipItem(Pocket.Armor, 0x3AC1, 0xFFFFFF, 0x0, 0xFFFFFF);
		EquipItem(Pocket.Shoe, 0x4274, 0x0, 0x0, 0x808080);

		SetLocation(region: 300, x: 212476, y: 192867);

		SetDirection(114);
	}
}
