using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Beanrua_baseScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetRace(10001);
        
        SetColor(0x0, 0x0, 0x0);

		Phrases.Add("Good to see you!");
		Phrases.Add("Welcome!");        
    }
}
