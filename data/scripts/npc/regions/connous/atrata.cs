using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class AtrataScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_atrata");
		SetRace(9001);
		SetFace(skin: 15, eye: 35, eyeColor: 238, lip: 2);
		SetLocation(3100, 379708, 421574, 181);
		SetStand("elf/female/anim/elf_npc_atrata_stand_friendly");

		EquipItem(Pocket.Face, 0x170C, 0x1B71A9, 0x6F4D00, 0x91066E);
		EquipItem(Pocket.Hair, 0x138A, 0xB141D, 0xB141D, 0xB141D);
		EquipItem(Pocket.Shoe, 0x4280, 0x3A142A, 0x5C9AFC, 0x47ABF1);
		EquipItem(Pocket.Head, 0x46D0, 0xFFFFFF, 0x979CF9, 0x4600D8);
		EquipItem(Pocket.Robe, 0x4A4E, 0xEF96A0, 0x5B0E2B, 0x17041F);
		EquipItem(Pocket.RightHand1, 0xB3C7, 0x3A142A, 0x5C9AFC, 0x47ABF1);
		SetHoodDown();

		Phrases.Add("Do you think that we can ever get used to the idea of death?");
		Phrases.Add("How long have you been sick?");
		Phrases.Add("I'm trying very hard to remember.");
		Phrases.Add("It should get better, little by little.");
		Phrases.Add("Just as we all look different and have different tastes, so are the differences in our personalities.");
		Phrases.Add("Somewhere, out there, there are always traces of one's memory.");
		Phrases.Add("The darker it gets outside, the brighter the light feels.");
		Phrases.Add("The most important thing is to have faith.");
		Phrases.Add("You should keep some of your memories. The good ones, for later.");
	}
}
