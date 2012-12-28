using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class WyllowScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_wyllow");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 0.1f, upper: 0.94f, lower: 0.83f);
		SetFace(skin: 17, eye: 0, eyeColor: 31, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1357, 0x4B813E, 0x976D84, 0x5123);
		EquipItem(Pocket.Hair, 0x1358, 0x916B39, 0x916B39, 0x916B39);
		EquipItem(Pocket.Armor, 0x3AEB, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Shoe, 0x4271, 0x7B6642, 0xB4A67F, 0x842160);
		EquipItem(Pocket.Head, 0x46AC, 0x808080, 0xFFFFFF, 0xFFFFFF);

		SetLocation(region: 61, x: 6880, y: 5030);

		SetDirection(76);
		SetStand("");
        
		Phrases.Add("Bow down to the Lord...");
		Phrases.Add("Hmmm...");
		Phrases.Add("Let us pray...");
		Phrases.Add("Lord Lymilark...");
		Phrases.Add("These days, people just don't have that much faith...");
	}
}
