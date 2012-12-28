using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class AodhanScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_aodhan");
		SetRace(10002);
		SetBody(height: 1.3f, fat: 1f, upper: 1.2f, lower: 1f);
		SetFace(skin: 20, eye: 12, eyeColor: 98, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0x17ABF, 0x78B264, 0x666C52);
		EquipItem(Pocket.Hair, 0xFCB, 0xBCD5AE, 0xBCD5AE, 0xBCD5AE);
		EquipItem(Pocket.Armor, 0x32E0, 0xCEBFB5, 0x3F524E, 0x161D29);
		EquipItem(Pocket.RightHand2, 0x9C61, 0xB7B6B8, 0xC48246, 0x9AAFA2);

		SetLocation(region: 52, x: 34544, y: 46247);

		SetDirection(225);
		SetStand("monster/anim/ghostarmor/Tequip_C/ghostarmor_Tequip_C01_stand_friendly");
        
        Phrases.Add("...");
		Phrases.Add("......Did the patrol officer come back?");
		Phrases.Add("Another peaceful day.");
		Phrases.Add("Is the quality of the trainees getting worse...?");
		Phrases.Add("No monsters in sight, Sir.");
		Phrases.Add("This seems too easy...");
		Phrases.Add("We should make night training twice as hard.");
		Phrases.Add("You need to get permission in order to enter the castle.");
	}
}
