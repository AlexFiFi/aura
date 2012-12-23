using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Courcleinhabitant8Script : NPCScript
{
	public override void OnLoad()
	{
		SetName("_courcleinhabitant8");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 25, eye: 7, eyeColor: 23, lip: 1);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0x9C318F, 0xB20829, 0x9AD089);
		EquipItem(Pocket.Hair, 0xBC7, 0x196240, 0x196240, 0x196240);
		EquipItem(Pocket.Armor, 0x3B8E, 0x5D6962, 0x213547, 0x5A6A6A);

		SetLocation(region: 3300, x: 255696, y: 184995);

		SetDirection(250);
		SetStand("");
	}
}
