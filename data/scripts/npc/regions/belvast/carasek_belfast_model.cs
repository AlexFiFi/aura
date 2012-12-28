using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Carasek_belfast_modelScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_carasek_belfast_model");
		SetRace(10002);
		SetBody(height: 1.2f, fat: 1f, upper: 1.29f, lower: 1f);
		SetFace(skin: 26, eye: 9, eyeColor: 46, lip: 18);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0x94AAD8, 0xF6992B, 0x606B53);
		EquipItem(Pocket.Hair, 0xFB5, 0xFFE9DBC2, 0xFFE9DBC2, 0xFFE9DBC2);
		EquipItem(Pocket.Armor, 0x3A9F, 0xFF021520, 0xFF06182D, 0xFF000000);
		EquipItem(Pocket.Shoe, 0x429F, 0xFF030B12, 0xFF000000, 0xFFFFFF);

		SetLocation(region: 4005, x: 59045, y: 28404);

		SetDirection(197);
		SetStand("");
	}
}
