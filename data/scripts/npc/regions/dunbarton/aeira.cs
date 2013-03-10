using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class AeiraScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_aeira");
		SetRace(10001);
		SetBody(height: 0.8000003f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 16, eye: 2, eyeColor: 27, lip: 1);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0xAB6523, 0x6C696C, 0x650800);
		EquipItem(Pocket.Hair, 0xBCE, 0x664444, 0x664444, 0x664444);
		EquipItem(Pocket.Armor, 0x3AC2, 0xEBAE98, 0x354E34, 0xE3E4EE);
		EquipItem(Pocket.Shoe, 0x4280, 0xA0505E, 0xF8784F, 0x6E41);
		EquipItem(Pocket.Head, 0x466C, 0x746C54, 0xC0C0C0, 0x7C8C);

		SetLocation(region: 14, x: 44978, y: 43143);

		SetDirection(158);
		SetStand("human/female/anim/female_natural_stand_npc_Aeira");
        
        Phrases.Add("*cough* The books are too dusty...");
		Phrases.Add("*Whistle*");
		Phrases.Add("Hahaha.");
		Phrases.Add("Hmm. I can't really see...");
		Phrases.Add("Hmm. The Bookstore is kind of small.");
		Phrases.Add("I wonder if this book would sell?");
		Phrases.Add("I wonder what Stewart is up to?");
		Phrases.Add("Kristell... She's unfair.");
		Phrases.Add("Oh, hello!");
		Phrases.Add("Umm... So...");
		Phrases.Add("Whew... I should just finish up the transcription.");
	}
}
