using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class KusinaScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_kusina");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 26, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF46, 0x8377, 0xF99C4D, 0xFAB154);
		EquipItem(Pocket.Hair, 0xC1B, 0x37231D, 0x37231D, 0x37231D);
		EquipItem(Pocket.Armor, 0x3B8C, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 3300, x: 258180, y: 188820);

		SetDirection(183);
		SetStand("human/female/anim/female_stand_npc_iria_01");
	}
}
