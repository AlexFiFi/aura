using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Tara_guard1Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_tara_guard1");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 16, eye: 3, eyeColor: 126, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0xF4F7D8, 0x594A9F, 0xDEC7E2);
		EquipItem(Pocket.Hair, 0xFA3, 0x393839, 0x393839, 0x393839);
		EquipItem(Pocket.Armor, 0x3BEC, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9C90, 0x808080, 0x6F6F6F, 0x0);

		SetLocation(region: 401, x: 82342, y: 122341);

		SetDirection(144);
		SetStand("monster/anim/ghostarmor/natural/ghostarmor_natural_stand_friendly");
	}
}
