using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Giant_familyguideScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_giant_familyguide");
		SetRace(8002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 50, eyeColor: 126, lip: 29);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x22C4, 0x997302, 0xA60027, 0xFBCDCF);
		EquipItem(Pocket.Hair, 0x1F52, 0x444449, 0x444449, 0x444449);
		EquipItem(Pocket.Armor, 0x3B6C, 0xACB4B6, 0x212628, 0x808080);
		EquipItem(Pocket.Shoe, 0x42C0, 0x9141A, 0x345695, 0x59BEFB);

		SetLocation(region: 3200, x: 290613, y: 213077);

		SetDirection(82);
		SetStand("");
	}
}
