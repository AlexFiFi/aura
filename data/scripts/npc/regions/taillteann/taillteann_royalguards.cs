using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class _TaillteannRoyalGuardBaseScript : NPCScript
{
	public override void OnLoad()
	{
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);

		EquipItem(Pocket.Armor, 0x3BEC, 0x785C3E, 0x633C31, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9C90, 0x808080, 0x6F6F6F, 0x0);
        
		Phrases.Add("I should be receiving a letter from home soon...");
		Phrases.Add("I was reminiscing about home and I didn't get much sleep last night.");
		Phrases.Add("Is it morning already...?");
		Phrases.Add("It hasn't been that long since I've eaten, but I'm hungry again...");
		Phrases.Add("Waking up to that bell every morning is a pain.");
		Phrases.Add("When is my shift going to end?");
	}
}

public class TaillteannRoyalGuard1Script : _TaillteannRoyalGuardBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_taillteann_human1");
		SetFace(skin: 16, eye: 5, eyeColor: 0, lip: 0);
		SetLocation(300, 212248, 200090, 200);

		EquipItem(Pocket.Face, 0x1324, 0x7038, 0xFFF218, 0x4CCAEF);
		EquipItem(Pocket.Hair, 0xFC7, 0x211C39, 0x211C39, 0x211C39);
	}
}

public class TaillteannRoyalGuard2Script : _TaillteannRoyalGuardBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_human2");
		SetFace(skin: 20, eye: 32, eyeColor: 162, lip: 0);
		SetLocation(300, 211712, 200090, 200);

		EquipItem(Pocket.Face, 0x1324, 0xE38E71, 0x4B4B63, 0x717372);
		EquipItem(Pocket.Hair, 0x135C, 0x663333, 0x663333, 0x663333);
	}
}
