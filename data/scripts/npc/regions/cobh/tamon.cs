using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class TamonScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_tamon");
		SetRace(128);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 92, eyeColor: 238, lip: 33);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1EDC, 0x37B35D, 0xFDD7C9, 0xD1E16E);
		EquipItem(Pocket.Hair, 0x1B5E, 0xC61400, 0xC61400, 0xC61400);
		EquipItem(Pocket.Armor, 0x3C88, 0xC44842, 0x511E1E, 0x7F0A0A);
		EquipItem(Pocket.Shoe, 0x435F, 0x0, 0x5FB9C9, 0x7A6C00);
		EquipItem(Pocket.RightHand2, 0x9CF2, 0xBBBCC3, 0x504462, 0x816B2F);

		SetLocation(region: 23, x: 27728, y: 37861);

		SetDirection(173);
		SetStand("");
        
		Phrases.Add("Be careful around Madoc. He's a thief.");
		Phrases.Add("Being in a storm at sea is a feeling beyond words.");
		Phrases.Add("Have you heard anything about my ship?");
		Phrases.Add("I'd like to teach those brigands a lesson!");
		Phrases.Add("If you're selling, I'm buying!");
		Phrases.Add("Madoc is a swindler.");
		Phrases.Add("My ship should be safe.");
		Phrases.Add("My ship should be safe... right?");
		Phrases.Add("Pirates make me so nervous.");
		Phrases.Add("Pirates need to just vanish from the seas.");
		Phrases.Add("That's very expensive, so keep it safe.");
	}
}
