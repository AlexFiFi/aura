using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Taillteann_repairman3Script : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_repairman3");
		SetRace(8002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 20, eye: 49, eyeColor: 126, lip: 27);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x22C4, 0xFAAFB8, 0xF8AD5D, 0xF5A33B);
		EquipItem(Pocket.Hair, 0x1F4F, 0x663333, 0x663333, 0x663333);
		EquipItem(Pocket.Armor, 0x3BF0, 0x2A2719, 0x8E8B78, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9CF0, 0x232323, 0x615739, 0x0);

		SetLocation(region: 300, x: 205912, y: 190960);

		SetDirection(73);
		SetStand("");
        
		Phrases.Add("Do you need a weapon for Giants?");
	}
}
