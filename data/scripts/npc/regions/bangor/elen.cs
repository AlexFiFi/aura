using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class ElenScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_elen");
		SetRace(10001);
		SetBody(height: 0.6000001f, fat: 1f, upper: 1.1f, lower: 1.1f);
		SetFace(skin: 25, eye: 3, eyeColor: 54, lip: 1);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0x2C6B74, 0xF25CA0, 0xB5901E);
		EquipItem(Pocket.Hair, 0xBBD, 0xFFE680, 0xFFE680, 0xFFE680);
		EquipItem(Pocket.Armor, 0x3AB5, 0xFFFFFF, 0x942370, 0xEFE1C2);
		EquipItem(Pocket.Shoe, 0x427B, 0x2B6280, 0x67676C, 0x5DAA);
		EquipItem(Pocket.Head, 0x4668, 0x7D2224, 0xFFFFFF, 0x88CD);
		EquipItem(Pocket.RightHand1, 0x9C58, 0xFACB5F, 0x4F3C26, 0xFAB052);

		SetLocation(region: 31, x: 11353, y: 12960);

		SetDirection(15);
		SetStand("human/female/anim/female_natural_stand_npc_elen");
        
		Phrases.Add("Come over here if you are interested in blacksmith work.");
		Phrases.Add("Grandpa worries too much.");
		Phrases.Add("Heh. That boy over there is kind of cute. I'd get along with him really well.");
		Phrases.Add("How about some excitement in this town?");
		Phrases.Add("If my beauty mesmerizes you, at least have the guts to come and tell me so.");
		Phrases.Add("I'm not too bad at blacksmith work myself, you know.");
		Phrases.Add("It's rather slow today...");
		Phrases.Add("Lets see... I still have some left...");
		Phrases.Add("Mom always neglects me...");
		Phrases.Add("Nothing is free!");
		Phrases.Add("The real fun is in creating, not repairing.");
	}
}
