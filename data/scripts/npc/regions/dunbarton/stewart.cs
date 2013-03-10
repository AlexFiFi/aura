using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class StewartScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_stewart");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 16, eye: 3, eyeColor: 120, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0x49C2AF, 0x609F, 0x447045);
		EquipItem(Pocket.Hair, 0xFAA, 0x997744, 0x997744, 0x997744);
		EquipItem(Pocket.Armor, 0x3A9A, 0xF7941D, 0xA0927D, 0xB80026);
		EquipItem(Pocket.Shoe, 0x4274, 0xB80026, 0x4F548D, 0x904959);
		EquipItem(Pocket.Head, 0x466D, 0x625F44, 0xC1C1C1, 0xCEA96B);
		EquipItem(Pocket.Robe, 0x4A3B, 0x993333, 0x221111, 0x664444);

		SetLocation(region: 18, x: 2671, y: 1771);

		SetDirection(99);
		SetStand("");
        
		Phrases.Add("Hmm... I'll have to talk with Kristell about this.");
		Phrases.Add("Hmm... There aren't enough textbooks available.");
		Phrases.Add("I wonder if Aeira has prepared all the books.");
		Phrases.Add("It's not going to work like this.");
		Phrases.Add("Maybe I should ask Aranwen...");
		Phrases.Add("More and more people are not showing up...");
		Phrases.Add("Oh dear! I've already run out of magic materials.");
		Phrases.Add("Perhaps there's something wrong with my lecture?");

	}
}
