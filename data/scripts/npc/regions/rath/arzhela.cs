using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class ArzhelaScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_arzhela");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 101, eyeColor: 27, lip: 35);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF44, 0x169AD, 0x8D91, 0x8A003C);
		EquipItem(Pocket.Hair, 0xC26, 0x9F936C, 0x9F936C, 0x9F936C);
		EquipItem(Pocket.Armor, 0x3C0E, 0x4C3D63, 0xD2E0E4, 0x2E3137);
		EquipItem(Pocket.Shoe, 0x4290, 0x3D190F, 0x808080, 0x808080);

		SetLocation(region: 417, x: 3117, y: 3027);

		SetDirection(193);
		SetStand("chapter3/human/female/anim/female_c3_npc_dorren");
	}
}
