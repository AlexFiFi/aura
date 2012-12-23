using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class JocelinScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_jocelin");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 1, eyeColor: 47, lip: 1);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0x633C60, 0xDC002B, 0xA90045);
		EquipItem(Pocket.Hair, 0xBCD, 0x491622, 0x491622, 0x491622);
		EquipItem(Pocket.Armor, 0x3AC1, 0x75554A, 0x75554A, 0x69554F);
		EquipItem(Pocket.Shoe, 0x4270, 0x0, 0xD3AB64, 0xF29D38);

		SetLocation(region: 62, x: 2125, y: 1871);

		SetDirection(121);
		SetStand("");
	}
}
