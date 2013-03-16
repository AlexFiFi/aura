using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Portia_epilogueScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_portia_epilogue");
		SetRace(10001);
		SetBody(height: 0.6999999f, fat: 0.85f, upper: 1.15f, lower: 0.85f);
		SetFace(skin: 16, eye: 148, eyeColor: 135, lip: 43);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0xF51, 0xB03B1F, 0xAD151E, 0x34C4E4);
		EquipItem(Pocket.Hair, 0xC3A, 0xFADE9C, 0xFADE9C, 0xFADE9C);
		EquipItem(Pocket.Armor, 0x3DFE, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 3106, x: 3734, y: 3616);

		SetDirection(188);
		SetStand("chapter4/human/female/anim/female_c4_npc_posser");
	}
}
