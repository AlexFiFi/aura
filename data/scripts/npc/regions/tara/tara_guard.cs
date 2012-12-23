using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Tara_guardScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_tara_guard");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 20, eye: 5, eyeColor: 8, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0x755600, 0xBDDFB0, 0xFA9464);
		EquipItem(Pocket.Hair, 0xFA4, 0x9C5D42, 0x9C5D42, 0x9C5D42);
		EquipItem(Pocket.Armor, 0x3BEC, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9C90, 0x808080, 0x6F6F6F, 0x0);

		SetLocation(region: 401, x: 82183, y: 122645);

		SetDirection(144);
		SetStand("monster/anim/ghostarmor/natural/ghostarmor_natural_stand_friendly");
	}
}
