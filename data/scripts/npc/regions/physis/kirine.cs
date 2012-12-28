using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class KirineScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_kirine");
		SetRace(27);
		SetBody(height: 2.5f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.RightHand1, 0x9DA5, 0x704215, 0x735858, 0x6E6B69);

		SetLocation(region: 3200, x: 289702, y: 212703);

		SetDirection(82);
		SetStand("giant/female/anim/giant_npc_kirine");
        
		Phrases.Add("A legacy that will last an eternity.");
		Phrases.Add("A mirror that reflects the sun will never show the shadow of darkness.");
		Phrases.Add("Do you think we can start another war?");
		Phrases.Add("Ha, if you don't have bread, eat meat.");
		Phrases.Add("Haha, this is my secret recipe, poison mushroom stew.");
		Phrases.Add("I am greedier than you think.");
		Phrases.Add("I don't even know who I am.");
		Phrases.Add("I want to leave a legacy behind.");
		Phrases.Add("If the moon is too bright, you can't see the surrounding stars.");
		Phrases.Add("Seemingly insignificant events will eventually decide one's fate.");
		Phrases.Add("There's nothing more honest than a mirror.");
		Phrases.Add("You can't satisfy me with that.");
		Phrases.Add("You only get one chance to make a first impression.");
	}
}
