using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Aven_madamScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_aven_madam");
		SetRace(10001);
		SetBody(height: 0.9f, fat: 1.1f, upper: 0.9f, lower: 1.1f);
		SetFace(skin: 15, eye: 72, eyeColor: 27, lip: 1);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0xF3C, 0xF69664, 0xEE0060, 0x15B9B);
		EquipItem(Pocket.Hair, 0xBD0, 0xCECB68, 0xCECB68, 0xCECB68);
		EquipItem(Pocket.Armor, 0x3B1B, 0xDDE3E0, 0x818F4E, 0x719861);
		EquipItem(Pocket.Shoe, 0x426F, 0xB8C172, 0x5EBCFA, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9C51, 0x854E2D, 0x96DFC5, 0x78AD05);

		SetLocation(region: 4005, x: 30439, y: 32638);

		SetDirection(114);
		SetStand("human/anim/tool/female_tool_Bhand_M01_playing_lute_01");

		Phrases.Add("Ha ha!");
		Phrases.Add("How was the piece I just played?");
		Phrases.Add("Oh dear, something like that really happened?");
		Phrases.Add("Thank you for the entertaining story!");
		Phrases.Add("Times like this call for good music!");
	}
}
