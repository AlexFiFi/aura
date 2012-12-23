using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class BrendaScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_brenda");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 0, eyeColor: 49, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF48, 0x68962F, 0x558E, 0x1E2C7C);
		EquipItem(Pocket.Hair, 0xC1E, 0x822222, 0x822222, 0x822222);
		EquipItem(Pocket.Armor, 0x3BE7, 0xC46751, 0xC99E92, 0x1C2468);
		EquipItem(Pocket.Shoe, 0x427D, 0x1C2468, 0xC1A201, 0x808080);

		SetLocation(region: 300, x: 212557, y: 189817);

		SetDirection(68);
		SetStand("chapter3/human/female/anim/female_c3_npc_brenda");
	}
}
