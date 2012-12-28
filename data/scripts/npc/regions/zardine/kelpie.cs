using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class KelpieScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_kelpie");
		SetRace(10010);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 3, eyeColor: 161, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0x5F3A00, 0x97185B, 0xE5A64E);
		EquipItem(Pocket.Hair, 0xFFE, 0x857E66, 0x857E66, 0x857E66);
		EquipItem(Pocket.Armor, 0x3BC4, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 3400, x: 330869, y: 175715);

		SetDirection(186);
		SetStand("human/male/anim/male_natural_stand_npc_bryce");
        
		Phrases.Add("As long as I'm alive, there's hope.");
		Phrases.Add("Be careful of the Wyverns.");
		Phrases.Add("Most of my memories have disappeared.");
		Phrases.Add("The Blue Dragon, Legatus, saved my life.");
		Phrases.Add("The Hot-Air Balloon has been useful for the explorations.");
		Phrases.Add("You don't think I've lost all my memories, do you?");
	}
}
