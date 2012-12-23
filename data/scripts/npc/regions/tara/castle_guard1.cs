using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Castle_guard1Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_castle_guard1");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0x403F62, 0xB30042, 0x6F5353);
		EquipItem(Pocket.Hair, 0xFBE, 0x7683B0, 0x7683B0, 0x7683B0);
		EquipItem(Pocket.Armor, 0x3C4D, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x4754, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 401, x: 112391, y: 119227);

		SetDirection(160);
		SetStand("monster/anim/ghostarmor/natural/ghostarmor_natural_stand_friendly");
	}
}
