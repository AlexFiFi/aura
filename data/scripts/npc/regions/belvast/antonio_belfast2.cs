using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Antonio_belfast2Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_antonio_belfast2");
		SetRace(10002);
		SetBody(height: 1.4f, fat: 1f, upper: 1.23f, lower: 0.95f);
		SetFace(skin: 19, eye: 145, eyeColor: 27, lip: 49);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1341, 0x438C, 0xCFE034, 0x9CB263);
		EquipItem(Pocket.Hair, 0x102D, 0x202020, 0x202020, 0x202020);
		EquipItem(Pocket.Armor, 0x3DFC, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 4005, x: 44737, y: 34996);

		SetDirection(227);
		SetStand("chapter4/human/male/anim/male_c4_npc_antonio");
	}
}
