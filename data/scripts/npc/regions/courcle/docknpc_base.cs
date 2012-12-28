using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Docknpc_baseScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 25, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Hair, 0xFFC, 0x3A322F, 0x3A322F, 0x3A322F);
		EquipItem(Pocket.Armor, 0x3B89, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x46F8, 0x808080, 0x808080, 0x808080);

		SetStand("");
        
		Phrases.Add("Exchange your Stars with rewards.");
		Phrases.Add("May the Great Spirit of Irinid bless you on your journey.");
		Phrases.Add("Raft-drifting is prohibited at all times.");
		Phrases.Add("Water sports are good and all, but be careful while in and around water.");
	}
}
