using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Taillteann_repairman1Script : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_repairman1");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 20, eye: 5, eyeColor: 8, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0xA8730B, 0xF5916B, 0x5E5200);
		EquipItem(Pocket.Hair, 0xFA4, 0x9C5D42, 0x9C5D42, 0x9C5D42);
		EquipItem(Pocket.Armor, 0x3BEC, 0x785C3E, 0x633C31, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9C90, 0x808080, 0x6F6F6F, 0x0);

		SetLocation(region: 300, x: 211480, y: 200810);

		SetDirection(219);
		SetStand("");
        
		Phrases.Add("...");
		Phrases.Add("Have you come to purchase weapons?");
	}
}
