// Aura Script
// --------------------------------------------------------------------------
// Workers
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class _WorkerBelfastBaseScript : NPCScript
{
	public override void OnLoad()
	{
		SetRace(123);
		SetColor(0x808080, 0x808080, 0x808080);	

		Phrases.Add("Heave ho...");
		Phrases.Add("Hmm, I think we are missing some...");
		Phrases.Add("I need a break...");
		Phrases.Add("I need to find a way to make some money...");
		Phrases.Add("I need to work hard and save up...");
		Phrases.Add("I should be able to get a job once the dock is open, right?");
		Phrases.Add("I used to be really successful... Then the pirates came.");
		Phrases.Add("I want to be rich as soon as possible.");
		Phrases.Add("I want to be rich, too.");
		Phrases.Add("I want to set sail on the sea...");
		Phrases.Add("If I work hard, I can save up a lot, right?");
		Phrases.Add("I'm not scared of those ruddy pirates!");
		Phrases.Add("It's so hard to save up money.");
		Phrases.Add("Just a quick break...I'll work harder afterwards!");
		Phrases.Add("Let's move this!");
		Phrases.Add("Move this here...");
		Phrases.Add("The weather is so nice!");
		Phrases.Add("Those blasted pirates!");
		Phrases.Add("Unemployment is so frustrating!");
		Phrases.Add("When will I be done with this?");
		Phrases.Add("When will the construction at the dock end?");
		Phrases.Add("Working is tough, but it can be fun.");
	}
}

public class WorkerBelfast1Script : _WorkerBelfastBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_worker_belfast_01");
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 20, eye: 30, eyeColor: 0, lip: 0);
		SetLocation(4005, 54358, 23439, 193);

		EquipItem(Pocket.Face, 0x1324, 0x29C0BE, 0x8ADAA, 0x9A518F);
		EquipItem(Pocket.Hair, 0xFCB, 0x506A59, 0x506A59, 0x506A59);
		EquipItem(Pocket.Armor, 0x3AC4, 0x698C6D, 0x313A58, 0x4F91F4);
		EquipItem(Pocket.Shoe, 0x4493, 0x9F663F, 0xF6ECE5, 0x5F9CFC);
		EquipItem(Pocket.RightHand1, 0x9DFB, 0x808080, 0x808080, 0x808080);
	}
}

public class WorkerBelfast2Script : _WorkerBelfastBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_worker_belfast_02");
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1.4f);
		SetFace(skin: 20, eye: 21, eyeColor: 0, lip: 2);
		SetLocation(4005, 54700, 24291, 63);

		EquipItem(Pocket.Face, 0x1324, 0xDA9B59, 0xFA8E3D, 0x93B396);
		EquipItem(Pocket.Hair, 0x1773, 0x0, 0x0, 0x0);
		EquipItem(Pocket.Armor, 0x3AC4, 0x9FA9CD, 0x4A6290, 0x6C9368);
		EquipItem(Pocket.Shoe, 0x4493, 0x4792B0, 0x6B5737, 0xD6B2C3);
		EquipItem(Pocket.RightHand1, 0x9DFB, 0x808080, 0x808080, 0x808080);
	}
}

public class WorkerBelfast3Script : _WorkerBelfastBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetRace(10002);
		SetName("_worker_belfast_03");
		SetFace(skin: 19, eye: 26, eyeColor: 196, lip: 1);
		SetStand("human/anim/male_natural_sit_02");
		SetLocation(4005, 54285, 22677, 147);

		EquipItem(Pocket.Face, 0x1324, 0xF09D4C, 0x7E6936, 0x7B3441);
		EquipItem(Pocket.Hair, 0x1775, 0xCC8D46, 0xCC8D46, 0xCC8D46);
		EquipItem(Pocket.Armor, 0x3BFC, 0x8194, 0xBAC2AE, 0x422FB7);
		EquipItem(Pocket.Shoe, 0x4493, 0x94546F, 0x4D6A8E, 0xA6F4FF);
	}
}

public class WorkerBelfast4Script : _WorkerBelfastBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetRace(10002);
		SetName("_worker_belfast_04");
		SetFace(skin: 19, eye: 31, eyeColor: 134, lip: 2);
		SetStand("human/anim/male_natural_sit_01");
		SetLocation(4005, 54029, 22732, 206);
		
		EquipItem(Pocket.Face, 0x1324, 0x380026, 0xFCC157, 0x98198C);
		EquipItem(Pocket.Hair, 0xFB9, 0x4F2727, 0x4F2727, 0x4F2727);
		EquipItem(Pocket.Armor, 0x3BFC, 0x8598BD, 0x525A70, 0x7589B1);
		EquipItem(Pocket.Shoe, 0x4493, 0x6B4D4B, 0xE6D9C8, 0x4A1896);
	}
}
