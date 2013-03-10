using Aura.Shared.Const;
using Aura.World.Events;
using Aura.Shared.Util;
using System;
using System.Collections.Generic;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class RuaScript : NPCScript
{
    private List<bool> working;
    private DateTime ruaEpoch = DateTime.Parse("Mar 23, 2008 22:21:00 GMT");
    private bool currentlyWorking;
    
    const int TIME_PER_ERINN_MINUTE = 1500; // 1.5 s
    const int TIME_PER_ERINN_HOUR   = TIME_PER_ERINN_MINUTE * 60; // 1 min 30 s
    const int TIME_PER_ERINN_DAY    = TIME_PER_ERINN_HOUR * 24; // 36 min

	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_rua");
		SetRace(16);
		SetBody(height: 1.1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

        working = new List<bool>()
        {
            false, false, false, true, false, false, false, false, true, false, false, true, false, true, true, false, true, true, true, false, false, false, false, false, false, false, true, false, true, false, false, true, false, false, false, true, false, false, false, true, false, false, true
        };
        
        ServerEvents.Instance.ErinnMidnightTick += OnMidnightTick;
        
        currentlyWorking = !IsWorking(DateTime.Now); //Make sure we move her on startup
        OnMidnightTick(null, null);

		SetDirection(82);
		SetStand("");
        
        Phrases.Add("Anyone care to join me for a cocktail?");
        Phrases.Add("Hmm...I'm a bit thirsty.");
        Phrases.Add("I don't like boring stories.");
        Phrases.Add("My, where are your eyes wandering to right now?");

	}
    
    public override void Dispose()
    {
        ServerEvents.Instance.ErinnMidnightTick += OnMidnightTick;
        base.Dispose();
    }
    
    private void OnMidnightTick(object sender, TimeEventArgs e)
    {
        bool next = IsWorking(DateTime.Now);
        
        if (next == currentlyWorking)
            return;
            
        currentlyWorking = next;
        
        if (next)
            WarpNPC(region: 57, x: 6981, y: 4998);
        else
            WarpNPC(region: 15, x: 200, y: 0);
            
        //Logger.Info("Rua changed to " + (next ? "working" : "resting"));
    }

    private bool IsWorking(DateTime serverTime)
    {
       var index = (int)Math.Floor((serverTime - ruaEpoch).TotalMilliseconds / TIME_PER_ERINN_DAY) % working.Count;
        while (index < 0)
            index += working.Count;
            
        return working[index];
    }
}
