using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class CorentinScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_corentin");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 105, eyeColor: 49, lip: 2);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0xFCBD54, 0xB70D27, 0xA0D5);
		EquipItem(Pocket.Hair, 0xC29, 0x202020, 0x202020, 0x202020);
		EquipItem(Pocket.Armor, 0x3CC9, 0x808080, 0xCD9048, 0xCD9048);
		EquipItem(Pocket.Shoe, 0x42F6, 0x454545, 0xA6A6A8, 0x7B1A5E);

		SetLocation(region: 421, x: 5692, y: 4294);

		SetDirection(117);
		SetStand("human/anim/uni_natural_stand_straight");
	}
}
