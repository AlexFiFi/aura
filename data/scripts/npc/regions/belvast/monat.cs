using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class MonatScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_monat");
		SetRace(8002);
		SetBody(height: 1.2f, fat: 1f, upper: 1f, lower: 1.3f);
		SetFace(skin: 24, eye: 107, eyeColor: 0, lip: 34);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x22C4, 0xF79520, 0xF29E39, 0xE40172);
		EquipItem(Pocket.Hair, 0x1F84, 0xC8C7BF, 0xC8C7BF, 0xC8C7BF);
		EquipItem(Pocket.Armor, 0x3BF6, 0x315775, 0xE1E4EA, 0xECEDE1);
		EquipItem(Pocket.Shoe, 0x4292, 0x1A1B1E, 0x43453A, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9DF4, 0x315775, 0xE1E4EA, 0xECEDE1);
		EquipItem(Pocket.RightHand2, 0x9C61, 0xB5B5B5, 0x5CC0F5, 0x5688B5);
		EquipItem(Pocket.LeftHand1, 0xB3C7, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.LeftHand2, 0x9C61, 0xB5B5B5, 0x5CC0F5, 0x5688B5);

		SetLocation(region: 4005, x: 54176, y: 25897);

		SetDirection(214);
		SetStand("chapter4/giant/male/anim/giant_c4_npc_baggagehandler");

		Phrases.Add("Ah...working is tough.");
		Phrases.Add("All right, just a little more!");
		Phrases.Add("Be careful with that!");
		Phrases.Add("Did we get everything?");
		Phrases.Add("Hmm...");
		Phrases.Add("Is this one...not ours?");
		Phrases.Add("Move it here!");
		Phrases.Add("Please leave that here.");
		Phrases.Add("You little runts!");
	}
}
