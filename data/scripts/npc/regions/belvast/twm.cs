using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class TwmScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_twm");
		SetRace(144);
		SetBody(height: 0.1f, fat: 1.2f, upper: 1.3f, lower: 1.1f);
		SetFace(skin: 16, eye: 10, eyeColor: 23, lip: 1);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0xE78B75, 0x509C3C, 0xB8DA91);
		EquipItem(Pocket.Hair, 0x1774, 0x6B6C65, 0x6B6C65, 0x6B6C65);
		EquipItem(Pocket.Armor, 0x3A98, 0xADAEC6, 0x3399, 0x4B6A73);
		EquipItem(Pocket.Shoe, 0x4493, 0x5C6669, 0xB6ACAC, 0x6F8600);
		EquipItem(Pocket.RightHand1, 0x9C42, 0x7B2C10, 0x5E7F00, 0x4D98F9);

		SetLocation(region: 4005, x: 52076, y: 34384);

		SetDirection(63);
		SetStand("chapter4/human/female/anim/female_c4_npc_lonnie_stand2");

		Phrases.Add("Everyone retreat!");
		Phrases.Add("Hyah! Bring it on!");
		Phrases.Add("I'll take over this area next! Mwahahaha!");
		Phrases.Add("Shh, please keep this a secret.");
		Phrases.Add("Stop right there!");
	}
}
