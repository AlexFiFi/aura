using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Sailor_belfast_baseScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetRace(10002);

		EquipItem(Pocket.Hair, 0x1779, 0x1B1D28, 0x1B1D28, 0x1B1D28);
		EquipItem(Pocket.RightHand1, 0x9E16, 0x0, 0x0, 0x0);

		SetLocation(region: 4005, x: 46670, y: 27420);

		SetDirection(64);

		Phrases.Add("Cheers for the next journey!");
		Phrases.Add("Cheers!");
		Phrases.Add("Don't mess with Barry...you'll be sorry!");
		Phrases.Add("It's on me tonight!");
		Phrases.Add("Kya! What a taste!");
		Phrases.Add("When you go to Barry's pub, you MUST check out his seafood dishes.");
		Phrases.Add("Where will we go next...?");
		Phrases.Add("Whew, I needed that.");
		Phrases.Add("Whoa, I feel great now.");
	}
}
