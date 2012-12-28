using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class HansScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_hans");
		SetRace(25);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 83, eyeColor: 23, lip: 35);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1339, 0x406763, 0x75CFF4, 0x14E9C);
		EquipItem(Pocket.Hair, 0x1009, 0xCC9966, 0xCC9966, 0xCC9966);
		EquipItem(Pocket.Armor, 0x3C20, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Shoe, 0x42AB, 0x5C4F3C, 0x7AA88C, 0x808080);

		SetLocation(region: 401, x: 99675, y: 89865);

		SetDirection(80);
		SetStand("chapter3/human/male/anim/male_c3_npc_hans_standfriendly");
	}
}
