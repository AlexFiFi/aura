using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class AndrasScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_andras");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 77, eyeColor: 134, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF49, 0x8F888C, 0x116390, 0xF46E7A);
		EquipItem(Pocket.Hair, 0xC1D, 0xD6CEB3, 0xD6CEB3, 0xD6CEB3);
		EquipItem(Pocket.Armor, 0x3BE9, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9CAC, 0x808080, 0xA4A4A4, 0x46290F);

		SetLocation(region: 300, x: 212000, y: 200400);

		SetDirection(200);
		SetStand("chapter3/human/female/anim/female_c3_npc_andras");
        
		Phrases.Add("A new mission has arrived.");
		Phrases.Add("For King Ethur Mac Cuill!");
		Phrases.Add("I hope the soldiers don't get injured during this mission...");
		Phrases.Add("Something is amiss in the Shadow Realm.");
		Phrases.Add("Supply Guard, we need more equipment.");
		Phrases.Add("The supplies should arrive soon...");
		Phrases.Add("There's been an increase in volunteer soldiers with alchemist backgrounds. Interesting...");
	}
}
