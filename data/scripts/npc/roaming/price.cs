// Aura Script
// --------------------------------------------------------------------------
// Price - Traveling Merchant
// The timetable we have here is basically the NA one, but where Price
// appears depends on the server's timezone.
// --------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using Aura.Shared.Const;
using Aura.Shared.Util;
using Aura.World.Events;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class PriceScript : NPCScript
{
	const bool _broadcastLocation = false;
	
	DateTime _epoch = DateTime.Parse("Mar 24, 2008 00:00:00 GMT");
	List<Tuple<uint, uint, uint, string>> _locations = new List<Tuple<uint, uint, uint, string>>()
	{
		new Tuple<uint, uint, uint, string>(1,  17360, 33370,  "Tir"),
		new Tuple<uint, uint, uint, string>(16, 24200, 63540,  "Dungald"),
		new Tuple<uint, uint, uint, string>(14, 48970, 37300,  "Dunbarton Field"),
		new Tuple<uint, uint, uint, string>(30, 43940, 48460,  "Dragon ruins"),
		new Tuple<uint, uint, uint, string>(31, 15722, 8155,   "Bangor Bar"),
		new Tuple<uint, uint, uint, string>(53, 95407, 110140, "Sen Mag"),
		new Tuple<uint, uint, uint, string>(52, 34887, 41805,  "Emain Alley"),
		new Tuple<uint, uint, uint, string>(56, 8154,  9973,   "Ceo Island"),
		new Tuple<uint, uint, uint, string>(52, 44866, 24701,  "Emain Island"),
		new Tuple<uint, uint, uint, string>(53, 95407, 110140, "Sen Mag"),
		new Tuple<uint, uint, uint, string>(30, 43940, 48460,  "Dragon Ruins"),
		new Tuple<uint, uint, uint, string>(31, 13840, 14746,  "Barri"),
		new Tuple<uint, uint, uint, string>(14, 42600, 37900,  "Dunbarton School"),
		new Tuple<uint, uint, uint, string>(16, 24200, 63540,  "Dungald")
	};
	
	public override void OnLoad()
	{
		SetName("_price");
		SetRace(10002);
		SetBody(height: 1.29f, fat: 1.12f, upper: 1.32f, lower: 1.06f);
		SetFace(skin: 19, eye: 4, eyeColor: 90, lip: 13);
		SetStand("human/male/anim/male_natural_stand_npc_Piaras");

		EquipItem(Pocket.Face, 4901, 0xD8DDF2, 0xEF8A7A, 0x3F4D);
		EquipItem(Pocket.Hair, 4955, 0xC1C298, 0xC1C298, 0xC1C298);
		EquipItem(Pocket.Armor, 15052, 0x986C4B, 0x181E13, 0xC2B39E);
		EquipItem(Pocket.Shoe, 17044, 0x74562E, 0xFFFFFF, 0xFFFFFF);
		EquipItem(Pocket.Head, 18024, 0x4E7271, 0x24312F, 0xFFFFFF);

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
		
		OnMidnightTick(MabiTime.Now); // Move Price when he loads
	}
	
	protected override void Subscribe()
	{
		EventManager.TimeEvents.ErinnMidnightTick += OnMidnightTick;
	}
	
	protected override void Unsubscribe()
	{
		EventManager.TimeEvents.ErinnMidnightTick += OnMidnightTick;
	}
	
	private void OnMidnightTick(MabiTime time)
	{
		var next = GetLocation(DateTime.Now);
		WarpNPC(next.Item1, next.Item2, next.Item3);
		if(_broadcastLocation)
			Broadcast("[Price] I'm at " + next.Item4 + " now!");
	}
	
	private Tuple<uint, uint, uint, string> GetLocation(DateTime serverTime)
	{
		var index = (int)Math.Floor((serverTime - _epoch).TotalMilliseconds / 2160000) % _locations.Count;
		if(index < 0)
			index += _locations.Count;
			
		return _locations[index];
	}
}
