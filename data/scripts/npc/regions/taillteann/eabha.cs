using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class EabhaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_eabha");
		SetRace(10002);
		SetBody(height: 0.2f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 0, eyeColor: 54, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1333, 0x95920F, 0xFFE3A6, 0x4C005F);
		EquipItem(Pocket.Hair, 0x1003, 0x85775F, 0x85775F, 0x85775F);
		EquipItem(Pocket.Armor, 0x3BE5, 0xC36426, 0x7D2D15, 0xF7F3DE);

		SetLocation(region: 300, x: 219342, y: 199777);

		SetDirection(181);
		SetStand("chapter3/human/male/anim/male_c3_npc_eabha");
	}
}
