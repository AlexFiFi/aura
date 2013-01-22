using Common.Constants;
using Common.Events;
using Common.Tools;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class RabScript : NPCScript
{
    private byte lastHour;
    private bool lastVisible;
    
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_rab");
		SetRace(20);
		SetBody(height: 0.9f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x404040;
		NPC.ColorC = 0xC0C0C0;

        lastHour = 255;
        lastVisible = false;
                
        ErinnTimeTick(null, new TimeEventArgs((byte)((DateTime.Now.Ticks / 900000000) % 24) ,0)); //Initialize Rab

		SetDirection(118);
		SetStand("");
        
		Phrases.Add("...");
		Phrases.Add("....");
		Phrases.Add("......");
		Phrases.Add("Ruff");
		Phrases.Add("Ruff, ruff");
		Phrases.Add("Whimper");
	}
    
    
    protected override void ErinnTimeTick(object sender, TimeEventArgs e)
    {
        if (sender != null)
            base.ErinnTimeTick(sender, e);
    
        if (lastHour == e.Hour)
            return;
        
        lastHour = e.Hour;
        
        if ((9 <= e.Hour && e.Hour < 11) || (15 <= e.Hour && e.Hour < 17) || (19 <= e.Hour && e.Hour < 21))
        {
            if ((sender == null || lastVisible == false))
            {
                WarpNPC(region: 53, x: 103263, y: 110129);
                lastVisible = true;
            }
        }
        else if (sender == null || lastVisible == true)
        {
            WarpNPC(region: 15, x: 400, y: 0);
            lastVisible = false;
        }
    }
}
