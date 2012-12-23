using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Courcleinhabitant4Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_courcleinhabitant4");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 27, eye: 4, eyeColor: 82, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0x531316, 0x82C45E, 0xFCA372);
		EquipItem(Pocket.Hair, 0xFC8, 0x364810, 0x364810, 0x364810);
		EquipItem(Pocket.Armor, 0x3B8F, 0x33517A, 0x544060, 0x9F6EAC);

		SetLocation(region: 3300, x: 249139, y: 183244);

		SetDirection(71);
		SetStand("");
	}
}
