using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class SianiScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_siani");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 29, eyeColor: 47, lip: 36);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0xDB018A, 0xF36279, 0x6FBB);
		EquipItem(Pocket.Hair, 0xC2C, 0xD18B00, 0xD18B00, 0xD18B00);
		EquipItem(Pocket.Armor, 0x3C1B, 0xC5880B, 0x0, 0x765008);
		EquipItem(Pocket.Shoe, 0x4274, 0x292B35, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x46F0, 0x252A2F, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9E2A, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 52, x: 29109, y: 42874);

		SetDirection(197);
		SetStand("chapter4/human/male/anim/male_c4_npc_dollmaker02");
	}
}
