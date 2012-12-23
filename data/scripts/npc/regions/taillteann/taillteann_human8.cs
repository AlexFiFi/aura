using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Taillteann_human8Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_taillteann_human8");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 16, eye: 3, eyeColor: 126, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0xF46369, 0x6E4D9E, 0xFBBC58);
		EquipItem(Pocket.Hair, 0xFA3, 0x393839, 0x393839, 0x393839);
		EquipItem(Pocket.Armor, 0x3BEC, 0x785C3E, 0x633C31, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9C90, 0x808080, 0x6F6F6F, 0x0);

		SetLocation(region: 300, x: 202173, y: 199698);

		SetDirection(126);
		SetStand("monster/anim/ghostarmor/natural/ghostarmor_natural_stand_friendly");
	}
}
