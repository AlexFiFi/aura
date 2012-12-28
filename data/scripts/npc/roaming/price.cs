using Common.Constants;
using Common.Events;
using Common.Tools;
using Common.World;
using System;
using System.Collections.Generic;
using World.Network;
using World.Scripting;
using World.World;

public class PriceScript : NPCScript
{

    List<Tuple<uint, uint, uint, string>> locations;
    DateTime priceEpoch = DateTime.Parse("Mar 24, 2008 00:00:00 GMT");
    
    const int TIME_PER_ERINN_MINUTE = 1500; // 1.5 s
    const int TIME_PER_ERINN_HOUR   = TIME_PER_ERINN_MINUTE * 60; // 1 min 30 s
    const int TIME_PER_ERINN_DAY    = TIME_PER_ERINN_HOUR * 24; // 36 min


	public override void OnLoad()
	{
		SetName("_price");
		SetRace(10002);
		SetBody(height: 1.29f, fat: 1.12f, upper: 1.32f, lower: 1.06f);
		SetFace(skin: 19, eye: 4, eyeColor: 90, lip: 13);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1325, 0xD8DDF2, 0xEF8A7A, 0x3F4D);
		EquipItem(Pocket.Hair, 0x135B, 0xC1C298, 0xC1C298, 0xC1C298);
		EquipItem(Pocket.Armor, 0x3ACC, 0x986C4B, 0x181E13, 0xC2B39E);
		EquipItem(Pocket.Shoe, 0x4294, 0x74562E, 0xFFFFFF, 0xFFFFFF);
		EquipItem(Pocket.Head, 0x4668, 0x4E7271, 0x24312F, 0xFFFFFF);
                
        locations = new List<Tuple<uint, uint, uint, string>>()
        {
            new Tuple<uint, uint, uint, string>(1,17360,33370,"Tir"),
            new Tuple<uint, uint, uint, string>(16,24200,63540,"Dungald"),
            new Tuple<uint, uint, uint, string>(14,48970,37300,"Dunbarton Field"),
            new Tuple<uint, uint, uint, string>(30,43940,48460,"Dragon ruins"),
            new Tuple<uint, uint, uint, string>(31,15722,8155,"Bangor Bar"),
            new Tuple<uint, uint, uint, string>(53,95407,110140,"Sen Mag"),
            new Tuple<uint, uint, uint, string>(52,34887,41805,"Emain Alley"),
            new Tuple<uint, uint, uint, string>(56,8154,9973,"Ceo Island"),
            new Tuple<uint, uint, uint, string>(52,44866,24701,"Emain Island"),
            new Tuple<uint, uint, uint, string>(53,95407,110140,"Sen Mag"),
            new Tuple<uint, uint, uint, string>(30,43940,48460,"Dragon Ruins"),
            new Tuple<uint, uint, uint, string>(31,13840,14746,"Barri"),
            new Tuple<uint, uint, uint, string>(14,42600,37900,"Dunbarton School"),
            new Tuple<uint, uint, uint, string>(16,24200,63540,"Dungald")
        };
        
        ServerEvents.Instance.ErinnMidnightTick += OnMidnightTick;
        
        OnMidnightTick(null, null); //Move Price when he loads

		SetDirection(80);
		SetStand("human/male/anim/male_natural_stand_npc_Piaras");
        
        Phrases.Add("...");
        Phrases.Add("...Maybe it's time for me to move to another town...");
        Phrases.Add("...The sales aren't like they used to be...");
        Phrases.Add("Hmm, Hmm...");
        Phrases.Add("I recognize that face...");
        Phrases.Add("Let's see...");
        Phrases.Add("So many beautiful ladies here. Haha.");
        Phrases.Add("Today's sales stink.");
        Phrases.Add("You don't need other merchants besides me.");
        Phrases.Add("You're not even going to buy...");

	}
    
    public override void Dispose()
    {
        ServerEvents.Instance.ErinnMidnightTick += OnMidnightTick;
        base.Dispose();
    }
    
    private void OnMidnightTick(object sender, TimeEventArgs e)
    {
        Tuple<uint, uint, uint, string> next = GetLocation(DateTime.Now);
        
        Warp(region: next.Item1, x: next.Item2, y: next.Item3);
        
        Logger.Info("Price has moved to " + next.Item4);
    }
    
    private Tuple<uint, uint, uint, string> GetLocation(DateTime serverTime)
    {
        var index = (int)Math.Floor((serverTime - priceEpoch).TotalMilliseconds / TIME_PER_ERINN_DAY) % locations.Count;
        while (index < 0)
            index += locations.Count;
            
        return locations[index];
    }
    
}
