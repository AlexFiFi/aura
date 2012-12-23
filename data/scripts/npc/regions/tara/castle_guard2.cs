using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Castle_guard2Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_castle_guard2");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1.1f, upper: 1.1f, lower: 1.1f);
		SetFace(skin: 246, eye: 8, eyeColor: 82, lip: 38);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0x704E50, 0x737374, 0x5DAB);
		EquipItem(Pocket.Hair, 0xFFA, 0x923B5D, 0x923B5D, 0x923B5D);
		EquipItem(Pocket.Armor, 0x3C4D, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x4754, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 401, x: 112800, y: 118794);

		SetDirection(160);
		SetStand("monster/anim/ghostarmor/natural/ghostarmor_natural_stand_friendly");
	}
}
