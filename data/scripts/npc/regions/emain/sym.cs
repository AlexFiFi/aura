using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class SymScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_sym");
		SetRace(10002);
		SetBody(height: 0.1000001f, fat: 1.2f, upper: 1.3f, lower: 1.1f);
		SetFace(skin: 17, eye: 167, eyeColor: 162, lip: 54);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0xF262A2, 0x5B3F60, 0x6C81);
		EquipItem(Pocket.Hair, 0x1774, 0x6C6955, 0x6C6955, 0x6C6955);
		EquipItem(Pocket.Armor, 0x3B03, 0x304E48, 0x8B9C65, 0x282795);
		EquipItem(Pocket.Shoe, 0x427C, 0x7D99AF, 0x808080, 0x808080);

		SetLocation(region: 52, x: 30954, y: 43159);

		SetDirection(110);
		SetStand("chapter4/human/male/anim/male_c4_npc_cry_boy");
	}
}
