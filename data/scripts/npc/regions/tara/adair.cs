using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class AdairScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_adair");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 0.1f, upper: 1f, lower: 0.85f);
		SetFace(skin: 17, eye: 3, eyeColor: 39, lip: 2);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0xBCDFC1, 0xEDDD50, 0xFDCF4B);
		EquipItem(Pocket.Hair, 0xBE6, 0xFFCAAE7B, 0xFFCAAE7B, 0xFFCAAE7B);
		EquipItem(Pocket.Armor, 0x32D2, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Glove, 0x4090, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Shoe, 0x445F, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x484D, 0xEFEFEF, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9D60, 0xEFEFEF, 0xFF690C0C, 0xFFDA9343);

		SetLocation(region: 428, x: 66980, y: 108362);

		SetDirection(43);
		SetStand("");

		Phrases.Add("Do knights train these days?");
		Phrases.Add("Do you even know how to hold a lance?");
		Phrases.Add("Where is all the talent?");
	}
}
