using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class CobhworkerScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_cobhworker");
		SetRace(123);
		SetBody(height: 1f, fat: 1.1f, upper: 1.1f, lower: 1f);
		SetFace(skin: 15, eye: 20, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0xFFF78A, 0x673F60, 0x747);
		EquipItem(Pocket.Hair, 0x1027, 0x585454, 0x585454, 0x585454);
		EquipItem(Pocket.Armor, 0x3ACB, 0xB5C8D0, 0x4F5251, 0x325757);
		EquipItem(Pocket.Shoe, 0x427C, 0x52697C, 0x888C00, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9DFB, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 23, x: 33031, y: 37178);

		SetDirection(5);
		SetStand("");
	}
}
