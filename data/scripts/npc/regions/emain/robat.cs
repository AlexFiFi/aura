using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class RobatScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_robat");
		SetRace(8002);
		SetBody(height: 0.9999999f, fat: 1.3f, upper: 1.2f, lower: 1f);
		SetFace(skin: 21, eye: 149, eyeColor: 8, lip: 29);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x22C4, 0x729E, 0xF58030, 0x58AFB1);
		EquipItem(Pocket.Hair, 0x1F65, 0x758289, 0x758289, 0x758289);
		EquipItem(Pocket.Armor, 0x3CFE, 0x706653, 0xDBE6FA, 0x0);
		EquipItem(Pocket.Shoe, 0x4603, 0x292B35, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x47A2, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 52, x: 30630, y: 43882);

		SetDirection(123);
		SetStand("chapter4/giant/male/anim/giant_c4_npc_guest");
        
		Phrases.Add("(He glances over his shoulder.)");
		Phrases.Add("Hmm...");
		Phrases.Add("Is this a new model...?");
		Phrases.Add("Let's see...");
		Phrases.Add("Mmm...");
		Phrases.Add("Very nice, very nice.");
	}
}
