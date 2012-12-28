using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Beanrua_baseScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetRace(10001);
        
        NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;

		Phrases.Add("Good to see you!");
		Phrases.Add("Welcome!");        
    }
}
