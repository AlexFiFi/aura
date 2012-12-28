using Common.Constants;
using Common.Events;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Beanruaguard_baseScript : NPCScript
{
    private byte lastHour;
    private bool lastVisible;

	public override void OnLoad()
	{
		base.OnLoad();
		SetRace(10002);
		SetBody(height: 1.26f, fat: 1.09f, upper: 1.26f, lower: 1f);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		SetStand("");
        
		Phrases.Add("Alright, alright, single line!");
		Phrases.Add("Alright, go in.");
		Phrases.Add("Are you looking for Bean Rua? It's right here.");
		Phrases.Add("Did your guild reserve a spot?");
		Phrases.Add("Don't hesitate, just walk in!");
		Phrases.Add("Full house today....");
		Phrases.Add("Hey hey, beautiful, right here!");
		Phrases.Add("Hmmm... she's too hot to just send her up to Lucas.");
		Phrases.Add("How many in your party?");
		Phrases.Add("Hurry up and go in!");
		Phrases.Add("It's okay, just walk right in.");
		Phrases.Add("It's open!!");
		Phrases.Add("Line up, please!");
		Phrases.Add("Mr. Lucas, a fine lady is looking for you!");
		Phrases.Add("Oh my gosh, I'll give you a call later...");
		Phrases.Add("Okay okay, stop forcing your way in...");
		Phrases.Add("Okay, you're in!");
		Phrases.Add("Please check your coat! We're now open!");
		Phrases.Add("Please come in.");
		Phrases.Add("Please wait for a second! I'm sorry!");
		Phrases.Add("Right this way. There's a seat available.");
		Phrases.Add("She's pretty cute...hehe");
		Phrases.Add("This is Bean Rua. We're now accepting people.");
		Phrases.Add("Welcome to Bean Rua.");
		Phrases.Add("Welcome welcome!");
		Phrases.Add("We're open now. Line up!");
		Phrases.Add("You may want to come back later if you want to go in.");
        
        lastHour = 255;
        lastVisible = false;
        
        ErinnTimeTick(null, new TimeEventArgs((byte)((DateTime.Now.Ticks / 900000000) % 24) ,0)); //Initialize Guard
	}
    
    protected override void ErinnTimeTick(object sender, TimeEventArgs e)
    {
        if (sender != null)
            base.ErinnTimeTick(sender, e);
    
        if (lastHour == e.Hour)
            return;
        
        lastHour = e.Hour;
        
        if (17 <= e.Hour || e.Hour < 6)
        {
            if ((sender == null || lastVisible == false))
            {
                Warp(true);
                lastVisible = true;
            }
        }
        else if (sender == null || lastVisible == true)
        {
            Warp(false);
            lastVisible = false;
        }
    }
    
    protected virtual void Warp(bool visible)
    {
    
    }
    
}
