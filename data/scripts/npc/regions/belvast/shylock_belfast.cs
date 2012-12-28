using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Shylock_belfastScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_shylock_belfast");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 0.8f, upper: 0.8f, lower: 0.8f);
		SetFace(skin: 17, eye: 146, eyeColor: 30, lip: 45);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x133E, 0x6E6D5C, 0x4A0017, 0xF89F3B);
		EquipItem(Pocket.Hair, 0x102C, 0x4E4F4A, 0x4E4F4A, 0x4E4F4A);
		EquipItem(Pocket.Armor, 0x3DFB, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9E14, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 4005, x: 26481, y: 33528);

		SetDirection(65);
		SetStand("chapter4/human/male/anim/male_c4_npc_salirock");
	}
}
