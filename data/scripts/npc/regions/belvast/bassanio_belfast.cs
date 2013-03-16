using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Bassanio_belfastScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_bassanio_belfast");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1f, upper: 0.9f, lower: 0.9f);
		SetFace(skin: 16, eye: 24, eyeColor: 32, lip: 50);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1342, 0x345, 0x670018, 0xBC0076);
		EquipItem(Pocket.Hair, 0x102E, 0xE8B686, 0xE8B686, 0xE8B686);
		EquipItem(Pocket.Armor, 0x3DFD, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 4005, x: 44845, y: 34914);

		SetDirection(82);
		SetStand("chapter4/human/male/anim/male_c4_npc_augustine");
	}
}
