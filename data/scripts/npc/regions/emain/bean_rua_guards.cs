using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Events;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class _BeanRuaGuardBaseScript : NPCScript
{
    private bool _visible = false;

	public override void OnLoad()
	{
		SetRace(10002);
		SetBody(height: 1.26f, fat: 1.09f, upper: 1.26f, lower: 1f);

		EquipItem(Pocket.Armor, 0x3AA6, 0x0, 0x0, 0x715B44);
		EquipItem(Pocket.Glove, 0x3E86, 0x2E231F, 0xFFFFFF, 0xFFFFFF);
		EquipItem(Pocket.Shoe, 0x4272, 0x0, 0xFFFFFF, 0xFFFFFF);
		
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
        
        this.OnErinnTimeTick(null, new TimeEventArgs(MabiTime.Now)); // Initialize Guard
	}
    
    protected override void OnErinnTimeTick(object sender, TimeEventArgs e)
    {
		// Call base for phrases.
        if (sender != null)
            base.OnErinnTimeTick(sender, e);
    
		if(e.Time.Hour >= 17 || e.Time.Hour < 6)
		{
			if(!_visible)
				Warp(_visible = true);
		}
		else
		{
			if(_visible)
				Warp(_visible = false);
		}
    }
    
    protected virtual void Warp(bool visible)
	{}
}

public class BeanRuaGuard1Script : _BeanRuaGuardBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_beanruaguard01");
		SetFace(skin: 15, eye: 9, eyeColor: 167, lip: 0);
		SetDirection(183);

		EquipItem(Pocket.Face, 0x1324, 0xFFFAE0, 0xCBEDF9, 0xB8588C);
		EquipItem(Pocket.Hair, 0xFBE, 0x612314, 0x612314, 0x612314);
	}
    
    protected override void Warp(bool visible)
    {
        if (visible)
            WarpNPC(52, 47115, 47272, false);
        else
            WarpNPC(15, 500, 0, false);
    }
}

public class BeanRuaGuard2Script : _BeanRuaGuardBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_beanruaguard02");
		SetFace(skin: 15, eye: 4, eyeColor: 32, lip: 0);
		SetDirection(155);

		EquipItem(Pocket.Face, 0x1324, 0x8346, 0xF99F48, 0xF46F3F);
		EquipItem(Pocket.Hair, 0xFC6, 0xAA7840, 0xAA7840, 0xAA7840);
	}
    
    protected override void Warp(bool visible)
    {
        if (visible)
            WarpNPC(52, 48270, 48122, false);
        else
            WarpNPC(15, 600, 0, false);
    }
}
