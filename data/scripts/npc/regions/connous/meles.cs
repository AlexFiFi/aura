using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class MelesScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_meles");
		SetRace(9001);
		SetBody(height: 0.8000003f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 37, eyeColor: 206, lip: 2);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x170C, 0xC5CF54, 0x1B6D2A, 0xD0DEF0);
		EquipItem(Pocket.Hair, 0x1389, 0xEBD8B5, 0xEBD8B5, 0xEBD8B5);
		EquipItem(Pocket.Shoe, 0x4288, 0x292C36, 0x2A3679, 0x6275AB);
		EquipItem(Pocket.Robe, 0x4A4F, 0xDF9329, 0x6D4125, 0xBE481F);
		EquipItem(Pocket.RightHand1, 0x9C4A, 0x292C36, 0x2A3679, 0x6275AB);

		SetLocation(region: 3100, x: 363460, y: 422632);

		SetDirection(205);
		SetStand("elf/female/anim/elf_npc_meles_stand_friendly");
        
		Phrases.Add("I wonder how it feels to be old?");
		Phrases.Add("If you think about it, I think there is meaning in everything in the Aura.World.");
		Phrases.Add("Is it that I'm maturing?");
		Phrases.Add("I've heard stories about Karu Forest.");
		Phrases.Add("One can be really sharp and yet turn thick-skinned over time.");
		Phrases.Add("The Tikka tree I saw in my dreams...");
		Phrases.Add("To know the sharpness of a blade, I'll have to learn about wounds.");
		Phrases.Add("Weapons are made for causing injuries.");
	}
}
