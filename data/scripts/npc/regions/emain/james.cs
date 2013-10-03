using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class JamesScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_james");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1f, upper: 1.3f, lower: 1f);
		SetFace(skin: 17, eye: 7, eyeColor: 97, lip: 2);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1325, 0x5A6E, 0x199AF, 0x1D7643);
		EquipItem(Pocket.Hair, 0xFCA, 0xE0B056, 0xE0B056, 0xE0B056);
		EquipItem(Pocket.Armor, 0x3AE9, 0x594933, 0x696969, 0xFFFFFF);
		EquipItem(Pocket.Shoe, 0x4274, 0x505832, 0xFFFFFF, 0x4B4B4B);
		EquipItem(Pocket.Head, 0x46AB, 0x747474, 0xFFFFFF, 0xFFFFFF);

		SetLocation(region: 52, x: 40966, y: 29741);

		SetDirection(74);
		SetStand("");
        
		Phrases.Add("Everyone, church is not boring!");
		Phrases.Add("I better clean this place up!");
		Phrases.Add("I need to order new candles.");
		Phrases.Add("I wonder if Comgan is doing alright.");
		Phrases.Add("Lord, I pray for another meaningful day with you.");
		Phrases.Add("May every adventurer be blessed...");
		Phrases.Add("Oh dear, I need to wash my clothes.");
		Phrases.Add("Try to accomplish small things first. It'll make everything easier.");
		Phrases.Add("Who should I write to today...");
		Phrases.Add("Would you like some tea...?");
	}
}
