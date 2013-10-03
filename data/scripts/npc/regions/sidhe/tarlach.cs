// Aura Script
// --------------------------------------------------------------------------
// Tarlach - Druid
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Events;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class TarlachScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_tarlach");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 0.9f, upper: 0.4f, lower: 0.6f);
		SetFace(skin: 15, eye: 4, eyeColor: 54, lip: 0);
		SetStand("human/male/anim/male_natural_stand_npc_Duncan");
		SetLocation("sidhe_south", 11100, 30400, 167);

		EquipItem(Pocket.Face, 4901, 13020774, 4930657, 16399);
		EquipItem(Pocket.Hair, 4021, 268435491, 268435491, 268435491);
		EquipItem(Pocket.Head, 18028, 6446916, 12632256, 6296681);
		EquipItem(Pocket.Armor, 15002, 12058662, 12058662, 5729677);
		EquipItem(Pocket.Shoe, 17009, 5648913, 16571605, 8235060);
		EquipItem(Pocket.Robe, 19004, 13269812, 11817009, 14335111);
		SetHoodDown();

		Phrases.Add("...I'll just wait a little longer...");
		Phrases.Add("Ah...");
		Phrases.Add("(Cough, Cough)");
		Phrases.Add("...Is it not morning yet?");
		Phrases.Add("Sigh...");
		Phrases.Add("...It's definitely cold at night...");
		Phrases.Add("...My head...");
		Phrases.Add("...I can take it...");
		Phrases.Add("I guess not yet...");
	}
	
	protected override void Subscribe()
	{
		EventManager.TimeEvents.ErinnDaytimeTick += OnErinnDaytimeTick;
	}

	protected override void Unsubscribe()
	{
		EventManager.TimeEvents.ErinnDaytimeTick -= OnErinnDaytimeTick;
	}

	private void OnErinnDaytimeTick(MabiTime time)
	{
		if(time.IsNight)
			WarpNPC(48, 11100, 30400);
		else
			WarpNPC(15, 0, 0);
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c, Options.FaceAndName, "A man wearing a light brown robe silently glares this way.<br/>He has wavy blonde hair and white skin with a well defined chin that gives off a gentle impression.<br/>Behind his thick glasses, however, are his cold emerald eyes filled with silent gloom.");
		Msg(c, "...Mmm...", Button("Start Conversation", "@talk"));
		
		var r = Select(c);
		if(r == "@talk")
		{
			Msg(c, "(Cough, Cough)...<br/>So you have made it through the barrier and have reached this desolate place.");
			
		L_Keywords:
			Msg(c, Options.Name, "(Tarlach is looking at me.)");
			ShowKeywords(c);
			
			var keyword = Select(c);
			
			Msg(c, "Can we change the subject?");
			goto L_Keywords;
		}
	}

	public override void OnEnd(WorldClient c)
	{
		Close(c, "(You ended your conversation with Tarlach.)");
	}
}
