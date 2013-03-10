using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Taillteann_healerScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_healer");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 22, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0xA80074, 0x9C891D, 0x470015);
		EquipItem(Pocket.Hair, 0xBE0, 0x5D5447, 0x5D5447, 0x5D5447);
		EquipItem(Pocket.Armor, 0x3C03, 0x6E5340, 0x0, 0xFFE3B5);

		SetLocation(region: 300, x: 214376, y: 203308);

		SetDirection(169);
		SetStand("");
	}
}
