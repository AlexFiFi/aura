using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Carys_to_iria_modelScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_carys_to_iria_model");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1.2f, upper: 1.1f, lower: 1.25f);
		SetFace(skin: 26, eye: 9, eyeColor: 46, lip: 18);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0x1A715D, 0xBA0371, 0xF2365F);
		EquipItem(Pocket.Hair, 0xFB5, 0xFFE9DBC2, 0xFFE9DBC2, 0xFFE9DBC2);
		EquipItem(Pocket.Armor, 0x3A9F, 0xFF021520, 0xFF06182D, 0xFF000000);
		EquipItem(Pocket.Shoe, 0x429F, 0xFF030B12, 0xFF000000, 0xFFFFFF);

		SetLocation(region: 23, x: 35759, y: 30738);

		SetDirection(93);
		SetStand("");
	}
}
