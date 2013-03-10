using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class EmainrichmanScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_emainrichman");
		SetRace(10002);
		SetBody(height: 0.9400001f, fat: 1.58f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 8, eyeColor: 27, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1356, 0xDEEFE0, 0x673131, 0xA5D489);
		EquipItem(Pocket.Hair, 0xFF4, 0xAC9D64, 0xAC9D64, 0xAC9D64);
		EquipItem(Pocket.Armor, 0x3AE2, 0x85673A, 0x0, 0xFFFFFF);
		EquipItem(Pocket.Glove, 0x3E80, 0xFDF8EE, 0xFFFFFF, 0xFFFFFF);
		EquipItem(Pocket.Shoe, 0x4292, 0x1D241A, 0xFFFFFF, 0xFFFFFF);
		EquipItem(Pocket.Head, 0x4672, 0x2B685F, 0xFFFFFF, 0xFFFFFF);

		SetLocation(region: 52, x: 40591, y: 39213);

		SetDirection(92);
		SetStand("");
        
		Phrases.Add("Do I have to carry around only traveler's checks...?");
		Phrases.Add("Hmmm hmmm");
		Phrases.Add("I should get a bigger wallet");
		Phrases.Add("Is this town full of beggars...?");
		Phrases.Add("Money's flowing everywhere...");
		Phrases.Add("Oh no, I dropped the money again");
		Phrases.Add("Oh, I dropped a coin");
		Phrases.Add("Oh, the change fell out");
		Phrases.Add("Why does money keep falling out of my pocket?");
	}
}
