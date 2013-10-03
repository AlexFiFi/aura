using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Events;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class _FletaRabTimerScript : NPCScript
{
	protected bool _visible;

	public override void OnLoad()
	{
        OnErinnTimeTick(MabiTime.Now);
	}
	
	protected override void Subscribe()
	{
		EventManager.TimeEvents.ErinnTimeTick += OnErinnTimeTick;
	}
	
	protected override void Unsubscribe()
	{
		EventManager.TimeEvents.ErinnTimeTick -= OnErinnTimeTick;
	}
	
    protected void OnErinnTimeTick(MabiTime time)
    {
		var h = time.Hour;
	
		// Visible from 9-11, 15-17, and 19-21
		if((h >= 9 && h < 11) || (h >= 15 && h < 17) || (h >= 19 && h < 21))
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
	{
	}
}

public class FletaScript : _FletaRabTimerScript
{
	public override void OnLoad()
	{
		SetName("_fleta");
		SetRace(10001);
		SetBody(height: 0.1f, fat: 1.06f, upper: 1.09f, lower: 1f);
		SetFace(skin: 15, eye: 8, eyeColor: 155, lip: 2);
		SetDirection(240);

		EquipItem(Pocket.Face, 0xF3C, 0x470036, 0xF79622, 0xF46F81);
		EquipItem(Pocket.Hair, 0xBBC, 0xFFBC8B63, 0xFFBC8B63, 0xFFBC8B63);
		EquipItem(Pocket.Armor, 0x3AE6, 0xFF301D16, 0xFF1B100E, 0xFF6D5034);
		EquipItem(Pocket.Shoe, 0x426F, 0x151515, 0xFFFFFF, 0xFFFFFF);
        
		Phrases.Add("...Ah, I'm bored.");
		Phrases.Add("...Are you scared?");
		Phrases.Add("...Hehe");
		Phrases.Add("Chirp Chirp.");
		Phrases.Add("La la la.");
		Phrases.Add("Should I go play somewhere else...");
		
        base.OnLoad();
	}
	
	protected override void Warp(bool visible)
	{
		if(visible)
			WarpNPC(53, 104689, 109742);
		else
			WarpNPC(15, 300, 0);
	}
}

public class RabScript : _FletaRabTimerScript
{
    private int lastHour = 255;
    private bool lastVisible = false;
    
	public override void OnLoad()
	{
		SetName("_rab");
		SetRace(20);
		SetBody(height: 0.9f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);
		SetColor(0, 0x404040, 0xC0C0C0);
		SetDirection(118);

		Phrases.Add("...");
		Phrases.Add("....");
		Phrases.Add("......");
		Phrases.Add("Ruff");
		Phrases.Add("Ruff, ruff");
		Phrases.Add("Whimper");
		
        base.OnLoad();
	}
	
	protected override void Warp(bool visible)
	{
		if(visible)
			WarpNPC(53, 103263, 110129);
		else
			WarpNPC(15, 400, 0);
	}
}
